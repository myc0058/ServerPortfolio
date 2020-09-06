using Engine.Framework;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Framework
{
    public static class Extension
    {
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        public static string Intern(this string value)
        {
            return string.Intern(value);
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, Engine.Framework.Dice.Throw(i, list.Count));
        }

        public static long ToByte(this string value)
        {
            if (byte.TryParse(value, out byte parsed) == true) { return parsed; }
            return 0;
        }

        public static long ToSByte(this string value)
        {
            if (sbyte.TryParse(value, out sbyte parsed) == true) { return parsed; }
            return 0;
        }

        public static short ToInt16(this string value)
        {
            if (short.TryParse(value, out short parsed) == true) { return parsed; }
            return 0;
        }

        public static ushort ToUInt16(this string value)
        {
            if (ushort.TryParse(value, out ushort parsed) == true) { return parsed; }
            return 0;
        }

        public static int ToInt32(this string value)
        {
            if (int.TryParse(value, out int parsed) == true) { return parsed; }
            return 0;
        }

        public static int ToInt32(this object value)
        {
            return (int)value;
        }
        public static int ToInt32(this char value)
        {
            if (int.TryParse(new string(value, 1), out int parsed) == true) { return parsed; }
            return 0;
        }
        public static short ToInt16(this object value)
        {
            return (short)value;
        }

        public static long ToInt64(this object value)
        {
            return Convert.ToInt64(value);
        }

        public static int Code<T>(this T msg) where T : class
        {
            if (msg is null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            return Engine.Framework.Id<T>.Value;
        }

        public static T ToProtobuf<T>(this string value) where T : global::Google.Protobuf.IMessage, new()
        {
            return global::Google.Protobuf.JsonParser.Default.Parse<T>(value);
        }

        public static string ToBase64UrlEncode(this string value)
        {
            return Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(value);
        }

        public static string ToBase64UrlEncode(this byte[] value)
        {
            return Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(value);
        }

        public static string ToBase64String(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static string ToBase64String(this string value)
        {
            return value.GetBytes().ToBase64String();
        }

        public static byte[] FromBase64String(this string value)
        {
            return Convert.FromBase64String(value);
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }


        public static string ToJson(this global::Google.Protobuf.IMessage value)
        {
            return global::Google.Protobuf.JsonFormatter.Default.Format(value);
        }

        public static global::Google.Protobuf.CodedInputStream ToCodedInputStream(this global::Google.Protobuf.IMessage msg)
        {
            var buf = new byte[msg.CalculateSize()];
            using (var os = new global::Google.Protobuf.CodedOutputStream(buf))
            {
                msg.WriteTo(os);
            }

            return new global::Google.Protobuf.CodedInputStream(buf);

        }

        public static void Serialize(this global::Google.Protobuf.IMessage msg, Stream to, bool leaveOpen)
        {
            lock (msg)
            {
                using (var co = new global::Google.Protobuf.CodedOutputStream(to, true))
                {
                    msg.WriteTo(co);
                }
            }
        }

        public static MemoryStream ToMemoryStream(this global::Google.Protobuf.IMessage msg)
        {
            int size = msg.CalculateSize();
            MemoryStream stream = new MemoryStream(size);
            using (var co = new global::Google.Protobuf.CodedOutputStream(stream, true))
            {
                msg.WriteTo(co);
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        //public static void MergeFrom<T>(this T msg, MemoryStream stream) where T : global::Google.Protobuf.IMessage<T>
        //{
        //    msg.MergeFrom(new global::Google.Protobuf.CodedInputStream(stream.GetBuffer(), 0, (int)stream.Length));
        //}

        public static void MergeFrom<T>(this T msg, Stream stream) where T : global::Google.Protobuf.IMessage<T>
        {
            msg.MergeFrom(new global::Google.Protobuf.CodedInputStream(stream));
        }

        public static int Id<T>(this T msg) where T : global::Google.Protobuf.IMessage<T>
        {
            return Engine.Framework.Id<T>.Value;
        }

        public static global::Google.Protobuf.CodedOutputStream ToCodedOutputStream(this global::Google.Protobuf.IMessage msg)
        {
            return new global::Google.Protobuf.CodedOutputStream(new byte[msg.CalculateSize()]);
        }

        public static DateTime ConvertTimeFromUtc(this DateTime UtcNow, string TimeZone)
        {
            var timezone = TimeZoneConverter.TZConvert.GetTimeZoneInfo(TimeZone);
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(UtcNow, timezone);
            return cstTime;
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static uint ToUInt32(this object value)
        {
            return (uint)value;
        }

        public static uint ToUInt32(this string value)
        {
            if (uint.TryParse(value, out uint parsed) == true) { return parsed; }
            return 0;
        }

        public static long ToInt64(this string value)
        {
            if (long.TryParse(value, out long parsed) == true) { return parsed; }
            return 0;
        }

        public static ulong ToUInt64(this string value)
        {
            if (ulong.TryParse(value, out ulong parsed) == true) { return parsed; }
            return 0;
        }


        public static bool ToBoolean(this string value)
        {
            if (bool.TryParse(value, out bool parsed) == true) { return parsed; }
            return true;
        }

        public static float ToFloat(this string value)
        {
            if (float.TryParse(value, out float parsed) == true) { return parsed; }
            return 0;
        }

        public static double ToDouble(this string value)
        {
            if (double.TryParse(value, out double parsed) == true) { return parsed; }
            return 0;
        }

        public static DateTime LastWeek(this DateTime value)
        {
            return Api.DateHelper.GetFirstDateTimeOfWeek(value, Api.DateHelper.FirstDayOfWeek).AddDays(-7);
        }

        public static DateTime CurrentWeek(this DateTime value)
        {
            return Api.DateHelper.GetFirstDateTimeOfWeek(value, Api.DateHelper.FirstDayOfWeek);
        }

        public static DateTime LastMonth(this DateTime value)
        {
            return Api.DateHelper.GetFirstDateTimeOfMonth(value).AddMonths(-1);
        }

        public static DateTime CurrentMonth(this DateTime value)
        {
            return Api.DateHelper.GetFirstDateTimeOfMonth(value);
        }
    }

    static public partial class Api
    {
        public abstract class Singleton<T> where T : new()
        {
            private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            public static T Instance
            {
                get
                {
                    return _instance.Value;
                }
            }


        }

        public static long UniqueKey
        {
            get
            {
                var key = System.Threading.Interlocked.Increment(ref uniqueKey);
                return (long)Offset | ((long)key << 32);
            }
        }

        public static long Offset = 0;
        private static int uniqueKey = 1;

        static public int ThreadCount { get => threadCount; }
        private static readonly int threadCount = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0));

        internal class Nortifier : Engine.Framework.INotifier
        {
            internal static Nortifier Instance = new Nortifier();
            public void Response<T>(T msg)
            {

            }
            public void Notify<T>(T msg)
            {
            }
            public void Serialize(Stream output) { }
        }

        public static class Coupon
        {

            static char[] keys = { '2', '3', '4', '6', '7', '9', 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            static private int GetValue(char c)
            {

                for (int i = 0; i < keys.Length; ++i)
                {
                    if (keys[i] == c)
                    {
                        return i;
                    }
                }

                return -1;

            }

            static public List<string> Generate(ushort index, ushort count)
            {

                List<string> coupons = new List<string>();

                for (ushort i = 0; i < count; ++i)
                {

                    uint value = (uint)(index) << 16 | i;

                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] btKey = ASCIIEncoding.ASCII.GetBytes("$modulus");
                    des.Key = btKey;
                    des.IV = btKey;
                    ICryptoTransform desencrypt = des.CreateEncryptor();

                    byte[] encripted;
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desencrypt, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(value);
                            }
                            encripted = msEncrypt.ToArray();
                        }
                    }

                    string coupon = "";
                    for (int j = 0; j < 8; ++j)
                    {

                        byte c = encripted[j];

                        char h = (char)(c / keys.Length);
                        char l = (char)(c % keys.Length);

                        coupon += keys[h];
                        coupon += keys[l];

                    }

                    coupons.Add(coupon);
                }


                return coupons;
            }
            static public void Decript(string key, out ushort index, out ushort count)
            {

                char[] cuponKey = key.ToCharArray();
                byte[] buffer = new byte[8];
                for (int i = 0; i < 16; i += 2)
                {

                    char c = cuponKey[i];
                    int h = GetValue(c);

                    c = cuponKey[i + 1];
                    int l = GetValue(c);

                    byte v = (byte)(h * keys.Length + l);
                    buffer[i / 2] = v;

                }

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] btKey = ASCIIEncoding.ASCII.GetBytes("$modulus");
                des.Key = btKey;
                des.IV = btKey;

                string plaintext;
                ICryptoTransform desdecrypt = des.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, desdecrypt, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                uint value = Convert.ToUInt32(plaintext);

                index = (ushort)(value >> 16);
                count = (ushort)(value & 0x0000FFFF);

            }


            public static byte[] Key { get; set; }

            public static byte[] IV { get; set; }

            public static void Generate(ushort index, ushort count, StreamWriter fw)
            {

                for (ushort i = 0; i < count; ++i)
                {

                    uint value = (uint)(index) << 16 | i;

                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] btKey = ASCIIEncoding.ASCII.GetBytes("$modulus");
                    des.Key = btKey;
                    des.IV = btKey;
                    ICryptoTransform desencrypt = des.CreateEncryptor();

                    byte[] encripted;
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desencrypt, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(value);
                            }
                            encripted = msEncrypt.ToArray();
                        }
                    }

                    string coupon = "";
                    for (int j = 0; j < 8; ++j)
                    {

                        byte c = encripted[j];

                        char h = (char)(c / keys.Length);
                        char l = (char)(c % keys.Length);

                        coupon += keys[h];
                        coupon += keys[l];

                    }

                    fw.WriteLine(coupon);
                }

            }
        }

        public static void Add(Engine.Framework.Layer layer)
        {
            lock (waitLayers)
            {
                waitLayers.Remove(layer);
                waitLayers.Add(layer);
            }

        }
        private static readonly List<Engine.Framework.Layer> layers = new List<Layer>();
        private static readonly List<Engine.Framework.Layer> waitLayers = new List<Layer>();
        public class Watcher
        {
            private FileSystemWatcher watcher;
            protected Queue<string> changed = new Queue<string>();
            protected Queue<string> deleted = new Queue<string>();
            protected Queue<string> errors = new Queue<string>();
            protected bool forceUpdate = false;
            public string Path { get; set; }
            public string Filter { get; set; }
            protected Dictionary<string, DateTime> deletedLastWriteTime = new Dictionary<string, DateTime>();
            protected Dictionary<string, DateTime> changedLastWriteTime = new Dictionary<string, DateTime>();


            protected void AddCreateOrChange(string path)
            {
                lock (this)
                {
                    if (changedLastWriteTime.ContainsKey(path) == true)
                    {
                        changedLastWriteTime[path] = DateTime.UtcNow;
                    }
                    else
                    {
                        changedLastWriteTime.Add(path, DateTime.UtcNow);
                    }
                }
            }

            protected void AddDeleteOrRename(string path)
            {
            }
            protected virtual void OnCallback(string path, bool ret) { }
            protected virtual void OnDeleted(object sender, FileSystemEventArgs e)
            {

                lock (this)
                {
                    FileInfo fi = new FileInfo(e.FullPath);
                    if (fi == null) { return; }

                    Console.WriteLine("Deleted : " + e.FullPath + " - " + DateTime.UtcNow);

                    lock (deletedLastWriteTime)
                    {
                        if (deletedLastWriteTime.ContainsKey(e.FullPath) == true)
                        {
                            deletedLastWriteTime[e.FullPath] = DateTime.UtcNow;
                        }
                        else
                        {
                            deletedLastWriteTime.Add(e.FullPath, DateTime.UtcNow);
                        }
                    }
                }

            }
            protected virtual void OnCreated(object sender, FileSystemEventArgs e)
            {
                FileInfo fi = new FileInfo(e.FullPath);
                if (fi == null) { return; }
                AddCreateOrChange(e.FullPath);
            }
            protected virtual void OnRenamed(object sender, RenamedEventArgs e)
            {
                lock (this)
                {
                    FileInfo fi = new FileInfo(e.FullPath);
                    if (fi == null) { return; }

                    Console.WriteLine("Renamed : " + e.FullPath + " - " + DateTime.UtcNow);

                }
            }
            protected virtual void OnChanged(object sender, FileSystemEventArgs e)
            {
                FileInfo fi = new FileInfo(e.FullPath);
                if (fi == null) { return; }
                AddCreateOrChange(e.FullPath);
            }
            internal void Run()
            {
                if (watcher != null) { return; }
                watcher = new FileSystemWatcher();

                Directory.CreateDirectory(Path);
                watcher.Path = Path;
                /* Watch for changes in LastAccess and LastWrite times, and
                   the renaming of files or directories. */
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                // Only watch text files.
                watcher.Filter = Filter;

                // Add event handlers.
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnCreated);
                watcher.Deleted += new FileSystemEventHandler(OnDeleted);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = true;
            }
            public virtual void Update()
            {
            }

            internal void Refresh()
            {

                DateTime now = DateTime.UtcNow;
                lock (this)
                {

                    foreach (var e in changedLastWriteTime)
                    {
                        var diff = now.Subtract(e.Value).TotalMilliseconds;
                        if (diff >= 5000)
                        {
                            changed.Enqueue(e.Key);
                        }
                    }

                    foreach (var e in changed)
                    {
                        changedLastWriteTime.Remove(e);
                    }

                    foreach (var e in deletedLastWriteTime)
                    {
                        var diff = now.Subtract(e.Value).TotalMilliseconds;
                        if (diff >= 5000)
                        {
                            deleted.Enqueue(e.Key);
                        }
                    }

                    foreach (var e in deleted)
                    {
                        deletedLastWriteTime.Remove(e);
                    }

                    if (changed.Count > 0 || deleted.Count > 0)
                    {
                        Update();
                    }

                }

            }


            public bool IsError()
            {
                return errors.Count > 0;
            }

            public bool IsClear()
            {
                if (changed.Count > 0 || deleted.Count > 0)
                {
                    return false;
                }
                return true;
            }
        }

        static public void AddWatcher(Watcher watcher)
        {

            lock (Api.watchers)
            {
                if (Api.watchers.ContainsKey(watcher.Path) == true) { return; }
                Api.watchers.Add(watcher.Path, watcher);
                watcher.Run();
            }
        }

        public static string DesEncrypt(string value, string key)
        {
            //키 유효성 검사
            byte[] btKey = ASCIIEncoding.ASCII.GetBytes(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            //소스 문자열
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desencrypt = des.CreateEncryptor();

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desencrypt, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(value);
                    }
                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }

        }
        public static string DesDecrypt(string value, string key)
        {
            //키 유효성 검사
            byte[] btKey = ASCIIEncoding.ASCII.GetBytes(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desdecrypt = des.CreateDecryptor();

            byte[] buffer = Convert.FromBase64String(value);
            using (MemoryStream msDecrypt = new MemoryStream(buffer))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, desdecrypt, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string plaintext = srDecrypt.ReadToEnd();
                        return plaintext;
                    }
                }
            }

        }//end of func DesDecrypt

        public static string Compress(MemoryStream stream)
        {
            stream.Position = 0;
            MemoryStream compressedStream = new MemoryStream();
            using (GZipStream compressionStream = new GZipStream(compressedStream,
                               CompressionMode.Compress))
            {
                stream.CopyTo(compressionStream);
            }

            return Convert.ToBase64String(compressedStream.ToArray());
        }

        public static MemoryStream Decompress(string base64)
        {
            byte[] array = Convert.FromBase64String(base64);
            MemoryStream compressed = new MemoryStream(array);

            MemoryStream original = new MemoryStream();
            using (GZipStream decompressionStream = new GZipStream(compressed, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(original);
            }
            return new MemoryStream(original.ToArray());
        }



  //      public class ScheduleLayer : Engine.Framework.Layer
  //      {
  //          public ScheduleLayer()
  //          {
  //              for (int i = 0; i < Engine.Framework.Api.ThreadCount; ++i)
  //              {
  //                  schedulers.TryAdd(i, new ConcurrentQueue<Task>());
  //              }
  //          }
		//	public override int OnUpdate()
		//	{
  //              return base.OnUpdate();
		//	}

  //          internal ConcurrentDictionary<int, ConcurrentQueue<Task>> schedulers = new ConcurrentDictionary<int, ConcurrentQueue<Task>>();

  //          ConcurrentQueue<Task> tasks = null;
  //          new internal void Post(Task e)
  //          {
  //              if (schedulers.TryGetValue(e.Strand, out tasks) == false)
  //              {
  //                  return;
  //              }
  //              tasks.Enqueue(e);
  //          }

  //          internal override int Update()
  //          {
  //              base.OnUpdate();

  //              long now = DateTime.UtcNow.Ticks;
  //              Parallel.ForEach(schedulers, options, (tasks) => {


  //                  Task task = null;

  //                  int max = tasks.Value.Count;
  //                  while (max > 0)
  //                  {
  //                      --max;
  //                      if (tasks.Value.TryDequeue(out task) == false)
  //                      {
  //                          break;
  //                      }

  //                      Scheduler scheduler = task as Scheduler;
  //                      if (scheduler == null || scheduler.IsClose() == true) continue;
  //                      if (scheduler.interval < 0)
  //                      {
  //                          continue;
  //                      }
                        

  //                      if (scheduler.Next > now)
  //                      {
  //                          Post(task);
  //                          continue;
  //                      }

  //                      if (scheduler.Pause == true)
  //                      {
  //                          scheduler.Next = DateTime.UtcNow.AddMilliseconds(scheduler.interval).Ticks;
  //                          Post(task);
  //                          continue;
  //                      }

  //                      scheduler.Next = DateTime.UtcNow.AddMilliseconds(scheduler.interval).Ticks;
  //                      Layer.currentStrand.Value = tasks.Key;
  //                      CurrentTask.Value = task;
  //                      if (scheduler.interval >= 0)
  //                      {
  //                          scheduler.OnSchedule();
  //                          Post(task);
  //                      }

  //                  }
  //              });

  //              return 0;
  //          }
		//}

        static Thread mainThread = null;
        static Thread subThread = null;

        static internal bool isOpen = false;

        static internal Dictionary<string, Api.Watcher> watchers = new Dictionary<string, Api.Watcher>();


        protected delegate void OverrideCallback();
        private static OverrideCallback OnOverride = null;
        
        public static ManualResetEvent MainThreadResetEvent = new ManualResetEvent(false);

        private static void UpdateLayer()
        {

            //Engine.Framework.Layers.Action actionLayer = new Engine.Framework.Layers.Action();
            //Engine.Framework.Layers.Entity entityLayer = new Engine.Framework.Layers.Entity();
            //Engine.Framework.Layers.Mediator mediatorLayer = new Engine.Framework.Layers.Mediator();
            //Engine.Framework.Layers.Job jobLayer = new Engine.Framework.Layers.Job();
            //Engine.Framework.Layers.Rpc rpcLayer = new Engine.Framework.Layers.Rpc();
            

            while (isOpen)
            {
                int wait = 0;
                //lock (layers)
                {
                    try
                    {
                        foreach (var e in layers)
                        {
                            wait += e.Update();
                        }

                        
                    }
                    catch
                    {

                    }
                    
                }

                if (waitLayers.Count > 0)
                {
                    try
                    {
                        lock (waitLayers)
                        {
                            layers.AddRange(waitLayers);
                            waitLayers.Clear();
                        }
                    }
                    catch
                    {

                    }
                    
                }


                lock (watchers)
                {
                    foreach (var watcher in watchers)
                    {

                        try
                        {
                            watcher.Value.Refresh();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("watcher.Value.Update() Exception\n" + e);
                        }
                    }
                }

                try
                {
                    OnOverride?.Invoke();
                    OnOverride = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnOverride?.Invoke()\r\n" + e);
                }

                if (wait > 0)
                {
                }
                else
                {
                    Api.MainThreadResetEvent.WaitOne();
                    Api.MainThreadResetEvent.Reset();

                    //global::System.Threading.Thread.Yield();

                    ////GC.Collect();
                    //long totalMemory = GC.GetTotalMemory(false);
                    //Console.WriteLine("totalMemory : " + (totalMemory / 1024) / 1024);
                }

            }

            return;
        }


        public static void Override()
        {
            OnOverride = () =>
            {
                void Error(string value)
                {
                    string now = string.Format("{0:yyyy-MM-dd_hh-mm-ss}.log", DateTime.UtcNow);
                    string path = Directory.GetCurrentDirectory() + "/Override/Error/";
                    Directory.CreateDirectory(path);
                    using (var file = File.CreateText(path + now))
                    {
                        file.WriteLine(value);
                        file.Flush();
                        file.Close();
                    }
                }

                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.GenerateExecutable = false;
                parameters.IncludeDebugInformation = true;


                parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");


                foreach (global::System.Reflection.Assembly b in AppDomain.CurrentDomain.GetAssemblies())
                {

                    foreach (var m in b.Modules)
                    {

                        try
                        {
                            var dll = global::System.IO.Path.GetExtension(m.Name);
                            if (dll == null) { continue; }
                            if (dll.ToLower() != ".dll" && dll.ToLower() != ".exe") { continue; }

                            if (Engine.Framework.Attributes.Override.IsContain(m.Name.ToLower()) == true) { continue; }

                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine("Override Exception " + e);
                            Error(e.ToString());
                            continue;
                        }

                        if (m.Name == "Microsoft.VisualStudio.HostingProcess.Utilities.dll" ||
                            m.Name == "Microsoft.VisualStudio.HostingProcess.Utilities.Sync.dll" ||
                            m.Name == "Microsoft.VisualStudio.Debugger.Runtime.dll" ||
                            m.Name == "mscorlib.resources.dll" ||
                            m.Name == "System.EnterpriseServices.Wrapper.dll" ||
                            m.Name == "(알 수 없음)" ||
                            m.Name == "<알 수 없음>" ||
                            m.Name == "<Unknown>" ||
                            m.Name == "<In Memory Module>" ||
                            m.Name == "<메모리 모듈>")
                        {
                            continue;
                        }
                        parameters.ReferencedAssemblies.Add(m.Name);
                        //Console.WriteLine(m.Name);
                    }

                }


                Engine.Framework.Attributes.Override.AddReference(parameters);
                string overrideAssemblePath = System.IO.Path.Combine(Directory.GetCurrentDirectory());
                var files = Directory.GetFiles(System.IO.Path.Combine(overrideAssemblePath, "Override"), "*.cs", SearchOption.AllDirectories);


                try
                {
                    CSharpCodeProvider codeProvider = new CSharpCodeProvider();

                    //parameters.OutputAssembly = string.Format($"{overrideAssemblePath}/{"test"}.dll");

                    CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, files.ToArray());
                    Assembly assembly = null;

                    codeProvider.Dispose();


                    if (!results.Errors.HasErrors)
                    {
                        assembly = results.CompiledAssembly;
                        Console.WriteLine("Override Success");
                    }
                    else
                    {
                        Console.WriteLine("Override Compile Error - ");
                        string error = "";
                        for (int i = 0; i < results.Output.Count; i++)
                        {
                            error += results.Output[i];
                            error += "\r\n";
                            Console.WriteLine(results.Output[i]);
                        }

                        Error(error);
                    }

                    var classes = (from type in assembly.GetTypes() where type.IsClass select type);

                    foreach (var c in classes)
                    {

                        var overrided = c.GetCustomAttribute(typeof(Engine.Framework.Attributes.Override), false);
                        if (overrided == null) { continue; }



                        {
                            var method = c.GetMethod("Override");
                            if (method != null)
                            {
                                Console.WriteLine($"Override {c.FullName}");
                                method.Invoke(null, new object[] { });
                            }

                        }



                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Override Exception " + e);
                    Error(e.ToString());
                }
            };
            


        }

        public class OverrideWatcher : Api.Watcher
        {
            protected DateTime configLastWriteTime = DateTime.UtcNow;
            protected bool configChanged = false;
            protected string configPath = "";

            protected override void OnCreated(object sender, FileSystemEventArgs e)
            {

                FileInfo fi = new FileInfo(e.FullPath);
                if (fi == null) { return; }

                if (System.IO.Path.GetFileName(e.FullPath).ToLower() == "config.xml")
                {
                    lock (this)
                    {
                        configPath = e.FullPath;
                        configChanged = true;
                        configLastWriteTime = DateTime.UtcNow;
                    }
                }
                else
                {
                    base.OnCreated(sender, e);
                }
            }
            protected override void OnChanged(object sender, FileSystemEventArgs e)
            {

                FileInfo fi = new FileInfo(e.FullPath);
                if (fi == null) { return; }

                if (System.IO.Path.GetFileName(e.FullPath).ToLower() == "config.xml")
                {
                    lock (this)
                    {
                        configPath = e.FullPath;
                        configChanged = true;
                        configLastWriteTime = DateTime.UtcNow;
                    }
                }
                else
                {
                    base.OnChanged(sender, e);
                }

            }

            static HashSet<string> buildedAssemblies = new HashSet<string>();
            private void Update(Queue<string> datas)
            {

                lock (datas)
                {

                    if (datas.Count == 0) { return; }

                    CompilerParameters parameters = new CompilerParameters();
                    parameters.GenerateInMemory = true;
                    parameters.GenerateExecutable = false;
                    parameters.IncludeDebugInformation = true;


                    parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");

                    foreach (var e in buildedAssemblies)
                    {
                        parameters.ReferencedAssemblies.Add(e);
                    }

                    foreach (global::System.Reflection.Assembly b in AppDomain.CurrentDomain.GetAssemblies())
                    {

                        foreach (var m in b.Modules)
                        {

                            try
                            {
                                var dll = global::System.IO.Path.GetExtension(m.Name);
                                if (dll == null) { continue; }
                                if (dll.ToLower() != ".dll" && dll.ToLower() != ".exe") { continue; }

                                if (Engine.Framework.Attributes.Override.IsContain(m.Name.ToLower()) == true) { continue; }

                            }
                            catch (ArgumentException)
                            {
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine("Override Exception " + e);
                                //Error(e.ToString());
                                errors.Enqueue(e.ToString());
                                continue;
                            }

                            if (m.Name == "Microsoft.VisualStudio.HostingProcess.Utilities.dll" ||
                                m.Name == "Microsoft.VisualStudio.HostingProcess.Utilities.Sync.dll" ||
                                m.Name == "Microsoft.VisualStudio.Debugger.Runtime.dll" ||
                                m.Name == "mscorlib.resources.dll" ||
                                m.Name == "System.EnterpriseServices.Wrapper.dll" ||
                                m.Name == "(알 수 없음)" ||
                                m.Name == "<알 수 없음>" ||
                                m.Name == "<Unknown>" ||
                                m.Name == "<In Memory Module>" ||
                                m.Name == "<메모리 모듈>")
                            {
                                continue;
                            }
                            parameters.ReferencedAssemblies.Add(m.Name);
                            //Console.WriteLine(m.Name);
                        }

                    }


                    Engine.Framework.Attributes.Override.AddReference(parameters);
                    string overrideAssemblePath = System.IO.Path.Combine(Directory.GetCurrentDirectory());
                    var files = Directory.GetFiles(System.IO.Path.Combine(overrideAssemblePath, "Override"), "*.cs", SearchOption.AllDirectories);


                    try
                    {
                        CSharpCodeProvider codeProvider = new CSharpCodeProvider();

                        parameters.OutputAssembly = string.Format($"{overrideAssemblePath}/{"test"}.dll");

                        CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, files.ToArray());
                        Assembly assembly = null;

                        codeProvider.Dispose();


                        if (!results.Errors.HasErrors)
                        {
                            assembly = results.CompiledAssembly;
                            Console.WriteLine("Success");

                            buildedAssemblies.Remove(parameters.OutputAssembly);
                            buildedAssemblies.Add(parameters.OutputAssembly);
                        }
                        else
                        {

                            Console.WriteLine("Override Compile Error - ");
                            string error = "";
                            for (int i = 0; i < results.Output.Count; i++)
                            {
                                error += results.Output[i];
                                error += "\r\n";
                                Console.WriteLine(results.Output[i]);
                            }

                            //Error(error);
                        }

                        var classes = (from type in assembly.GetTypes() where type.IsClass select type);

                        foreach (var c in classes)
                        {

                            var overrided = c.GetCustomAttribute(typeof(Engine.Framework.Attributes.Override), false);
                            if (overrided == null) { continue; }



                            {
                                var method = c.GetMethod("Override");
                                method?.Invoke(null, new object[] { });
                            }



                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Override Exception " + e);
                       // Error(e.ToString());
                    }
                    datas.Clear();
                }

            }

            
            public override void Update()
            {

                lock (this)
                {
                    if (configChanged == true)
                    {

                        if (DateTime.UtcNow.Subtract(configLastWriteTime).TotalMilliseconds >= 5000)
                        {
                            configChanged = false;
                            Update(configPath);
                        }

                    }
                }

                Update(changed);
            }

            private void Update(string configPath)
            {

                try
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(configPath);

                    Engine.Framework.Attributes.Override.Clear();

                    var root = doc.DocumentElement;
                    foreach (XmlElement e in root["Need"].ChildNodes)
                    {
                        Engine.Framework.Attributes.Override.AddReference(e.Attributes["Name"].Value);
                    }
                    foreach (XmlElement e in root["Ignore"].ChildNodes)
                    {
                        Engine.Framework.Attributes.Override.RemoveReference(e.Attributes["Name"].Value);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        };
        internal class OverrideConfigWatcher : Api.Watcher
        {
            private void Update(Queue<string> datas)
            {

                lock (datas)
                {

                    if (datas.Count == 0) { return; }

                    foreach (var fullPath in datas)
                    {
                        if (global::System.IO.Path.GetFileName(fullPath) == String.Empty)
                        {
                            continue;
                        }

                        Update(fullPath);
                    }
                    datas.Clear();
                }

            }
            public override void Update()
            {

                Update(changed);
            }

            internal static void Update(string configPath)
            {


                try
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(configPath);

                    Engine.Framework.Attributes.Override.Clear();

                    var root = doc.DocumentElement;
                    foreach (XmlElement e in root["Need"].ChildNodes)
                    {
                        Engine.Framework.Attributes.Override.AddReference(e.Attributes["Name"].Value);
                    }
                    foreach (XmlElement e in root["Ignore"].ChildNodes)
                    {
                        Engine.Framework.Attributes.Override.RemoveReference(e.Attributes["Name"].Value);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        };
        public class MetadataWatcher : Engine.Framework.Api.Watcher
        {
            public delegate void ReloadCallback(string path);

            static private Dictionary<string, ReloadCallback> WatchFiles = new Dictionary<string, ReloadCallback>();
            static private Dictionary<string, System.Type> WatchFilesByType = new Dictionary<string, System.Type>();
            static public void AddWatchFile(string path, ReloadCallback callback)
            {

                try
                {
                    WatchFiles.Add(global::System.IO.Path.GetFileName(path).ToLower(), callback);
                }
                catch (Exception e)
                {
                    Console.WriteLine(path + " " + e);
                }

            }
            protected void Update(Queue<string> datas)
            {

                lock (datas)
                {
                    if (datas.Count == 0) { return; }

                    ReloadCallback callback = null;
                    System.Type type = null;
                    int count = datas.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        var path = datas.Dequeue();

                        var filename = global::System.IO.Path.GetFileName(path);
                        if (WatchFiles.TryGetValue(filename, out callback) == true)
                        {
                            try
                            {
                                callback(path);
                                OnCallback(path, true);
                                Console.WriteLine(path + " - Success");
                            }
                            catch (IOException)
                            {
                                datas.Enqueue(path);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(path + " - Exception " + e);
                                OnCallback(path, false);
                            }
                        }
                        else if (WatchFilesByType.TryGetValue(filename, out type) == true)
                        {

                            try
                            {
                                foreach (var attribute in type.GetCustomAttributes(false))
                                {

                                    var metadata = attribute as Engine.Framework.Attributes.Metadata;
                                    if (metadata != null)
                                    {

                                        string loader = "LoadXml";

                                        if (metadata.type == Engine.Framework.Attributes.Metadata.Type.Json)
                                        {
                                            loader = "LoadJson";
                                        }

                                        var method = typeof(Repository).GetMethod(loader, new System.Type[] { typeof(string) });
                                        if (method.IsGenericMethod == true)
                                        {
                                            method = method.MakeGenericMethod(type);

                                        }
                                        Console.WriteLine("Load Matadata " + filename);
                                        
                                        method.Invoke(null, new object[] { System.IO.Path.Combine(this.Path, filename) });

                                        if (string.IsNullOrEmpty(metadata.Callback) == false)
                                        {
                                            method = type.GetMethod(metadata.Callback, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                                            if (method != null)
                                            {
                                                method.Invoke(null, null);
                                            }
                                        }

                                    }

                                }
                            }
                            catch (IOException)
                            {
                                datas.Enqueue(path);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(path + " - Exception " + e);
                                OnCallback(path, false);
                            }

                        }
                        else
                        {
                            Console.WriteLine(path + " - Fail");
                        }
                    }
                }
            }

            public override void Update()
            {
                //Update(created);
                Update(changed);
            }


            internal static void AddWatchFile(string path, Type c)
            {
                WatchFilesByType.Add(global::System.IO.Path.GetFileName(path), c);
            }
        };

        public static string BuildVersion4Digit(string before)
        {
            var date = DateTime.UtcNow.Date;
            int year = date.Year - 2000;
            int month = date.Month;
            int day = date.Day;
            int revision = 0;

            if (string.IsNullOrEmpty(before))
            {
                return $"{year.ToString("00")}.{month.ToString("00")}.{day.ToString("00")}.{revision.ToString("00")}";
            }

            var tokens = before.Split('.');
            if (tokens == null || tokens.Length != 4)
            {
                return $"{year.ToString("00")}.{month.ToString("00")}.{day.ToString("00")}.{revision.ToString("00")}";
            }

            revision = tokens[3].ToInt32();
            if (year == tokens[0].ToInt32() && month == tokens[1].ToInt32() && day == tokens[2].ToInt32())
            {
                revision += 1;
            }
            else
            {
                revision = 0;
            }
            
            return $"{year.ToString("00")}.{month.ToString("00")}.{day.ToString("00")}.{revision.ToString("00")}";
        }

        static public int GetWeekOfYear()
        {
            var cultureInfo = CultureInfo.CurrentCulture;

            CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;

            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            return cultureInfo.Calendar.GetWeekOfYear(DateTime.UtcNow, calendarWeekRule, firstDayOfWeek);
        }


        public static Stream Config { get; set; }
        public static bool Silence { get; set; }


        public interface ILog
        {
            void Error(object msg);
            void Warning(object msg);
            void Info(object msg);
            void Verbos(object msg);
        }


        public class Logger : Engine.Framework.Scheduler
        {
            public static bool TimeLine { get; set; } = true;
            private static long validate = 1;
            public static void Error(object msg)
            {
                if (TimeLine == true)
                {
                    Console.WriteLine($"[{KrNow.ToString("yyyy/MM/dd HH:mm:ss.fff")}][Error]{msg}");
                }
                else
                {
                    Console.WriteLine($"[Error]{msg}");
                }
                Interlocked.Increment(ref validate);
                
            }
            public static void Warning(object msg)
            {
                if (TimeLine == true)
                {
                    Console.WriteLine($"[{KrNow.ToString("yyyy/MM/dd HH:mm:ss.fff")}][Warning]{msg}");
                }
                else
                {
                    Console.WriteLine($"[Warning]{msg}");
                }
                Interlocked.Increment(ref validate);
            }
            public static void Info(object msg)
            {
                if (TimeLine == true)
                {
                    Console.WriteLine($"[{KrNow.ToString("yyyy/MM/dd HH:mm:ss.fff")}][Info]{msg}");
                }
                else
                {
                    Console.WriteLine($"[Info]{msg}");
                }
                Interlocked.Increment(ref validate);
            }

            public static void Verbose(object msg)
            {
                if (TimeLine == true)
                {
                    Console.WriteLine($"[{KrNow.ToString("yyyy/MM/dd HH:mm:ss.fff")}][Verbos]{msg}");
                }
                else
                {
                    Console.WriteLine($"[Verbos]{msg}");
                }
                Interlocked.Increment(ref validate);
            }

            protected internal override void OnSchedule(long deltaTicks)
            {
                Flush();
            }

            protected static int Sequence = 0;
            public static void Flush()
            {
                if (Interlocked.Exchange(ref validate, 0) == 0) { return; }
                var now = KrNow;
                try
                {
                    FileStream filestream = null;

                    string filename = string.Empty;

                    while (true)
                    {
                        int seq = Interlocked.Increment(ref Sequence);
                        filename = $"log_{seq}.txt";
                        if (File.Exists(filename) == true) { continue; }
                        break;
                    }

                    try
                    {
                        filestream = new FileStream(filename, FileMode.Create);
                    }
                    catch (Exception e)
                    {
                        Error(e);
                        return;
                    }

                    var streamwriter = new StreamWriter(filestream);
                    streamwriter.AutoFlush = true;
                    Console.SetOut(streamwriter);
                    Console.SetError(streamwriter);

                }
                catch (Exception e)
                {
                    Error(e);
                }
            }
        }

        public static DateTime KrNow
        {
            get
            {
                return DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time");
            }
        }

        public static int MainLoopSleepMS { get; set; } = 5;

        public static void StartUp()
        {
            

            if (isOpen == true)
                return;



            if (Silence == true)
            {
                Logger.Flush();
                Singleton<Logger>.Instance.Run(60000 * 5);
            }

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Offset = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);

            Engine.Framework.Api.Logger.Info("Engine.Framework.Api.StartUp");

            ThreadPool.SetMaxThreads(64, 32);
            ThreadPool.SetMinThreads(32, 16);

            Engine.Framework.Attributes.Override.StartUp();

            Engine.Framework.Api.Config = new FileStream("./Config.xml", FileMode.Open);

            AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;

            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            isOpen = true;
            mainThread = new Thread(new ThreadStart(UpdateLayer));
            mainThread.IsBackground = true;
            mainThread.Start();


            subThread = new Thread(new ThreadStart(() => 
            {
                while(isOpen)
                {
                    MainThreadResetEvent.Set();
                    System.Threading.Thread.Sleep(MainLoopSleepMS);
                }
            }));
            subThread.IsBackground = true;
            subThread.Start();

            var classes = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                           from type in asm.GetTypes()
                           where type.IsClass
                           select type);

            List<KeyValuePair<int, System.Type>> attributes = new List<KeyValuePair<int, Type>>();
            foreach (var c in classes)
            {
                try
                {
                    foreach (var attribute in c.GetCustomAttributes(false))
                    {

                        var startUp = attribute as Engine.Framework.Attributes.StartUp;
                        if (startUp != null)
                        {
                            attributes.Add(new KeyValuePair<int, System.Type>(startUp.Priority, c));
                        }
                    }
                }
                catch
                {

                }
            }



            Engine.Framework.Attributes.GenerateId.StartUp();
            Engine.Framework.Attributes.Initialize.StartUp();
            //Engine.Database.Attributes.Query.StartUp();

            var list = attributes.OrderBy(x => x.Key).ToList();
            foreach (var c in list)
            {
                c.Value.GetMethod("StartUp").Invoke(null, null);
            }

            //Add(Singleton<ScheduleLayer>.Instance);

            //Tasks.Rpc.HealthChecker.Instance.Update(3000);
        }

        private static void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            //e.IsTerminating = false;
            Logger.Error("UnhandledException " + e.ExceptionObject.ToString());
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            Logger.Error("OnUnobservedTaskException " + e.Exception);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {



        }

        static public void CleanUp()
        {
            //Tasks.Rpc.HealthChecker.Instance.Close();
            isOpen = false;
            mainThread.Join();
        }

        public static long AddressToInt64(string ip, ushort port)
        {
            string[] ips = ip.Split('.');
            long id = 0;
            for (int i = 0; i < 4; ++i)
            {
                id = id << 8;
                ushort n = ips[i].ToUInt16();
                id |= n;
            }

            id = id << 16;
            id |= port;
            return id;
        }
        public static uint IPAddressToUInt32(string ip)
        {
            string[] ips = ip.Split('.');
            uint id = 0;
            for (int i = 0; i < 4; ++i)
            {
                id = id << 8;
                ushort n = ips[i].ToUInt16();
                id |= n;
            }

            return id;
        }

        public static string UInt32ToIPAddress(uint value)
        {
            return $"{value >> 24}.{(value & 0x00FF0000) >> 16}.{(value & 0x0000FF00) >> 8}.{(value & 0x000000FF)}";
        }

        public static string Int64ToIPAddress(long value)
        {
            string ip = UInt32ToIPAddress((uint)(value >> 16));
            ushort port = (ushort)value;
            return $"{ip}:{port}";
        }

        public static string ToAddress(this uint value)
        {
            return UInt32ToIPAddress(value);
        }

        public static string ToAddress(this EndPoint endPoint)
        {

            if (endPoint == null)
            {
                return "";
            }

            if ((endPoint is IPEndPoint) == false)
            {
                return "";
            }

            if ((endPoint as IPEndPoint).Address == null)
            {
                return "";
            }

            return (endPoint as IPEndPoint).Address.ToString();
        }
    }
}
