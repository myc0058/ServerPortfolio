﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Schema.Protobuf
{
    public static partial class Api
    {
        public delegate void AsyncCallback<T>(T msg);
        public delegate void AsyncCallback();

        public interface ISerializable
        {
            void Serialize(System.IO.Stream output);
            int Length { get; }
        }

        public class ProtobufParser<T> where T : Google.Protobuf.IMessage<T>
        {
#pragma warning disable RECS0108 // 제네릭 형식의 정적 필드에 대해 경고합니다.
            internal static readonly System.Reflection.PropertyInfo field = typeof(T).GetProperty("Parser", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
#pragma warning restore RECS0108 // 제네릭 형식의 정적 필드에 대해 경고합니다.
            public static readonly Google.Protobuf.MessageParser<T> Parser = (Google.Protobuf.MessageParser<T>)field.GetValue(null);
            public Google.Protobuf.MessageParser GetParser()
            {
                return Parser;
            }
        }

        static public string PublicIp { get; private set; }
        static public string PrivateIp { get; private set; }
        public static long Idx { get; set; }
        public static bool DelegatorHeardBeat { get; set; } = true;
        
        static public Google.Protobuf.ByteString ToByteStringWithCode<T>(this T msg) where T : Google.Protobuf.IMessage<T>
        {
            return ToByteString(Id<T>.Value, msg);
        }

        static public Google.Protobuf.ByteString ToByteString<T>(int code, T msg) where T : Google.Protobuf.IMessage
        {
            var stream = new MemoryStream(4096);
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                bw.Write(code);
                using (var co = new Google.Protobuf.CodedOutputStream(stream, true))
                {
                    msg.WriteTo(co);
                }

                bw.Seek(0, SeekOrigin.Begin);
                return Google.Protobuf.ByteString.FromStream(stream);
            }
                
        }

        static public string GetPublicIp()
        {

            while (true)
            {
                try
                {
                    string result = new WebClient().DownloadString("https://checkip.amazonaws.com/");
                    result = result.Replace("\n", "");
                    Console.WriteLine($"My Public Ip : {result}");
                    return result;
                }
                catch
                {
                    Console.WriteLine($"Cannot Public IP");
                    continue;
                }
            }
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
    }
}
