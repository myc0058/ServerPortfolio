using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generater.Schema
{
    public class Unity : CSharp
    {
        List<Message> allMessages = new List<Message>();
        List<Protobuf.Dependency> modules = new List<Protobuf.Dependency>();

        public override void Initialize(string input)
        {
            string[] files = Directory.GetFiles(input, "*.proto", SearchOption.TopDirectoryOnly);


            foreach (var e in files)
            {
                var dependency = new Protobuf.Dependency();
                var lexer = new Protobuf.Lexer();
                lexer.Read(e);

                dependency.Package = lexer.GetPackage();
                dependency.Source = e;
                
                while (true)
                {
                    var import = lexer.GetImport();
                    if (string.IsNullOrEmpty(import) == true)
                    {
                        break;
                    }
                    dependency.Dependencies.Add(import);
                }
                modules.Add(dependency);
            }
            modules.Sort();

            int mid = 0;

            foreach (var m in modules)
            {
                ++mid;
                int pid = 1;
                string fileWithoutExtension = Path.GetFileNameWithoutExtension(m.Source);
                lexer = new Generater.Protobuf.Lexer();
                lexer.Read(m.Source);
                string package = lexer.GetPackage();

                m.Messages.Clear();

                while (true)
                {
                    string message = lexer.GetMessage();
                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }

                    m.Messages.Add(message, pid);
                    allMessages.Add(new Message() { Namespace = package, Name = message, Code = (mid << 16) | pid });

                    pid++;
                }
            }
        }

        public override void Parse(string input, string output)
        {
            Directory.CreateDirectory(output + "/Handler");

            foreach (var m in modules)
            {
                string handler = "";
                handler += string.Format("namespace Schema.Protobuf {{\r\n");

                handler += string.Format("\tpublic partial class Handler {{\r\n");

                foreach (var e in allMessages)
                {
                    if (e.Namespace != m.Package)
                    {
                        continue;
                    }
                    handler += string.Format("\t\tpublic virtual void OnMessage(INotifier notifier, {0}.{1} msg) {{}}\r\n", m.Package, e.Name);
                }

                handler += string.Format("\t}}\r\n");
                handler += string.Format("}}\r\n");


                string proto = "using System;\r\n";
                proto += string.Format("namespace {0} {{\r\n", m.Package);

                foreach (var e in allMessages)
                {
                    if (e.Namespace != m.Package)
                    {
                        continue;
                    }

                    proto += string.Format("\tpublic sealed partial class {0} {{ }}\r\n", e.Name);
                }


                proto += "}\r\n";


                using (var file = File.CreateText(output + "/Handler/" + Path.GetFileNameWithoutExtension(m.Source) + ".m.cs"))
                {
                    file.Write(proto);
                    file.Write(handler);
                }

            }

            {
                string api = "using System;\r\n";
                api += string.Format("namespace Schema.Protobuf {{\r\n\r\n");

                //
                api += string.Format("\tpublic interface INotifier\r\n");
                api += string.Format("\t{{\r\n");
                api += string.Format("\t\tvoid Response<T>(T msg) where T : global::Google.Protobuf.IMessage;\r\n");
                api += string.Format("\t\tvoid Notify<T>(T msg) where T : global::Google.Protobuf.IMessage;\r\n");
                api += string.Format("\t}}\r\n");
                api += string.Format("\tpublic delegate void Callback();\r\n");

                api += string.Format("\tpublic static partial class Api {{\r\n");

                api += string.Format("\t\tpublic static class Id<T> {{ public static int Value; }}\r\n");







                api += string.Format("\t\tprivate delegate Callback Binder(Handler handler, INotifier notifier, global::System.IO.Stream stream);\r\n");
                api += string.Format("\t\tprivate static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();\r\n");
                api += string.Format("");
                api += string.Format("\t\tstatic public void StartUp() {{\r\n");

                foreach (var e in allMessages)
                {
                    api += string.Format("\t\t\tId<{0}.{1}>.Value = 0x{2};  // {3}\r\n", e.Namespace, e.Name, e.Code.ToString("X8"), e.Code);
                }

                api += string.Format("\r\n");

                foreach (var e in allMessages)
                {
                    api += string.Format("\t\t\tBinders.Add({0}, (handler, notifier, stream) =>\r\n\t\t\t{{\r\n", e.Code);
                    api += string.Format("\t\t\t\tvar msg = {0}.{1}.Parser.ParseFrom(stream);\r\n", e.Namespace, e.Name);
                    //api += string.Format("\t\t\t\tvar stream.Dispose();\r\n");
                    //api += string.Format("\t\t\t\tmsg.MergeFrom(new Google.Protobuf.CodedInputStream(stream));\r\n");
                    //api += string.Format("\t\t\t\tmsg.MergeFrom(stream);\r\n");
                    api += string.Format("\t\t\t\treturn () => {{ handler.OnMessage(notifier, msg); }};\r\n\t\t\t}});\r\n");
                }

                api += string.Format("\t\t}}\r\n\r\n");

                api += string.Format("\t\tpublic static Callback Bind(Handler handler, INotifier notifier, int code, global::System.IO.Stream stream) {{\r\n\r\n");

                //api += string.Format("\t\t\tvar Handler = global::Engine.Framework.Singleton<T>.Instance;\r\n\r\n");
                api += string.Format("\t\t\tBinder binder = null;\r\n");
                api += string.Format("\t\t\tif (Binders.TryGetValue(code, out binder) == false) return () => {{ }};\r\n\r\n");
                api += string.Format("\t\t\treturn binder(handler, notifier, stream);\r\n\r\n");

                api += string.Format("\t\t}}\r\n");
                api += string.Format("\t}}\r\n");
                api += string.Format("}}\r\n");

                Build(output, "Api", api);
            }


        }

        private static void Build(string path, string filename, string imp)
        {
            int tryCount = 0;
            while (true)
            {
                try
                {
                    using (var file = File.CreateText(path + "/" + filename + ".m.cs"))
                    {
                        file.Write(imp);
                    }
                }
                catch
                {

                    if (tryCount++ < 3)
                    {
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }
                    Console.WriteLine("Can't open or create file : " + path + filename + ".m.cs");
                    Console.Write("Countine?[Y/N]");
                    var key = Console.ReadKey(false);

                    if (key.Key == ConsoleKey.Y)
                    {
                        tryCount = 0;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                break;
            }

        }
    }
}
