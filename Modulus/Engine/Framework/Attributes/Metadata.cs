using Engine.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Metadata : Attribute
    {
        public enum Type
        {
            Xml = 0,
            Json = 1,
            Csv = 2,
        }

        public Type type;
        public string extension;
        public string Callback { get; private set; }
        public string Filename { get; private set; }

        public static void Reload()
        {
            StartUp();
        }

        static public string Version { get; set; } = "0.0.0.0";
        public Metadata(Type type = Type.Xml, string filename = "", string callback = "", string extension = "")
        {
            this.type = type;
            this.Callback = callback;
            this.Filename = filename;
            this.extension = extension;
        }

        static public void StartUp()
        {
            Engine.Framework.Api.Logger.Info($"Load Metadata Version {Version}");
            var classes = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                           from type in asm.GetTypes()
                           where type.IsClass
                           select type);

            foreach (var c in classes)
            {
                try
                {

                    foreach (var attribute in c.GetCustomAttributes(false))
                    {

                        var metadata = attribute as Engine.Framework.Attributes.Metadata;
                        if (metadata != null)
                        {


                            string loader = "LoadXml";

                            if (metadata.type == Type.Json)
                            {
                                loader = "LoadJson";
                            }
                            else if (metadata.type == Type.Csv)
                            {
                                loader = "LoadCsv";
                            }

                            var method = typeof(Repository).GetMethod(loader, new System.Type[] { typeof(StreamReader) });
                            if (method.IsGenericMethod == true)
                            {
                                method = method.MakeGenericMethod(c);

                            }

                            string filename = metadata.Filename;
                            if (string.IsNullOrEmpty(filename) == true)
                            {
                                filename = c.Name;
                            }

                            string extension = ".xml";
                            if (metadata.type == Type.Json)
                            {
                                extension = ".json";
                            }
                            else if (metadata.type == Type.Csv)
                            {
                                extension = ".csv";
                            }

                            if (!string.IsNullOrEmpty(metadata.extension))
                            {
                                extension = "." + metadata.extension;
                                extension = Path.GetExtension(extension);
                            }


                            Engine.Framework.Api.Logger.Info($"Load Matadata [{filename}{extension}]");

                            
                            //myc0058 버젼에 맞게 파일을 로딩할수 있도록 수정해야 한다.
                            var path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../../../..", "Metadata", Version, $"{filename}{extension}");
                            path = Path.GetFullPath(path);
                            using (var stream = File.OpenRead(path))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                using (var sr = new StreamReader(stream))
                                {
                                    method.Invoke(null, new object[] { sr });
                                }
                            }

                            if (string.IsNullOrEmpty(metadata.Callback) == false)
                            {
                                method = c.GetMethod(metadata.Callback, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                                if (method != null)
                                {
                                    method.Invoke(null, null);
                                }
                            }

                        }


                    }
                }
                catch(Exception ex)
                {
                    Engine.Framework.Api.Logger.Error(ex.StackTrace);
                }
            }

        }


    }
}
