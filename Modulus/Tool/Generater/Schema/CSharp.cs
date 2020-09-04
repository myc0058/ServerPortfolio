using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generater.Schema
{
    public class CSharp : Protobuf.Buildable
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

        public override void UnitTest(string output)
        {
            var project = output;
            string ns = project.Replace('/', '.') + ".UnitTest";

            var path = Path.Combine(Generater.Protobuf.Root , project , "UnitTest");
            Directory.CreateDirectory(path);

            foreach (var m in modules)
            {

                string fileWithoutExtension = Path.GetFileNameWithoutExtension(m.Source);

                var splits = m.Package.Split('.');

                string lastModule = string.Empty;

                var filepath = path;

                if (splits != null && splits.Length > 0)
                {
                    filepath = Path.Combine(path, splits[splits.Length - 1]);
                    Directory.CreateDirectory(filepath);
                    lastModule = splits[splits.Length - 1];
                }

                foreach (var e in m.Messages)
                {
                    if (File.Exists(Path.Combine(filepath, e.Key + ".test.cs")) == true) { continue; }
                    using (var file = File.CreateText(Path.Combine(filepath, e.Key + ".test.cs")))
                    {
                        string test = "using System;\r\n\r\n";

                        test += $"namespace Application.StandAlone.UnitTest";
                        if (string.IsNullOrEmpty(lastModule) == false)
                        {
                            test += $".{lastModule}";
                        }

                        test += "\r\n{\r\n";



                        test += $"\tstatic public class {e.Key}\r\n\t{{\r\n";
                        test += "\t\tstatic public void UnitTest()\r\n\t\t{\r\n";
                        test += "\t\t\t//TODO:\r\n\r\n";
                        test += "\t\t}\r\n";
                        test += "\t}\r\n";

                        test += "}";
                        file.Write(test);
                    }
                }


            }

        }
        public override void Parse(string input, string output)
        {

            // search .code file
            //string[] files = Directory.GetFiles(input, "*.proto", SearchOption.TopDirectoryOnly);

            //List<Protobuf.Dependency> modules = new List<Protobuf.Dependency>();


            //foreach (var e in files)
            //{
            //    var dependency = new Protobuf.Dependency();
            //    var lexer = new Protobuf.Lexer();
            //    lexer.Read(e);

            //    dependency.Package = lexer.GetPackage();
            //    dependency.Source = e;

            //    while (true)
            //    {
            //        var import = lexer.GetImport();
            //        if (string.IsNullOrEmpty(import) == true)
            //        {
            //            break;
            //        }
            //        dependency.Dependencies.Add(import);
            //    }
            //    modules.Add(dependency);
            //}

            //modules.Sort();
            //List<Message> allMessages = new List<Message>();


            // Directory.CreateDirectory(Path.Combine(output, "Handler"));

            //int mid = 0;
            //foreach (var m in modules)
            //{
            //    string fileWithoutExtension = Path.GetFileNameWithoutExtension(m.Source);
            //    string handler = "";
            //    handler += string.Format("namespace Schema.Protobuf {{\r\n");
            //    handler += string.Format("\t}}\r\n");
            //    handler += string.Format("}}\r\n");


            //    string proto = "using System;\r\n";
            //    proto += string.Format("namespace {0} {{\r\n", m.Package);

            //    foreach (var e in m.Messages)
            //    {
            //        proto += string.Format("\tpublic sealed partial class {0} {{ }}\r\n", e.Key);
            //    }


            //    proto += "}\r\n";


            //    using (var file = File.CreateText(Path.Combine(output, "Handler", fileWithoutExtension + ".m.cs")))
            //    {
            //        file.Write(proto);
            //        file.Write(handler);
            //    }

            //    foreach (var h in Implements)
            //    {
            //        {
            //            var project = h.Project;

            //            foreach (var info in h.Infos)
            //            {

            //                var path = Generater.Protobuf.Root + "/" + project + "/" + info.Item1 + "/Handler/" + info.Item2;
            //                string ns = project.Replace('/', '.') + ".";
            //                ns += info.Item1.Replace('/', '.');
            //                Directory.CreateDirectory(path);


            //                path += "/" + fileWithoutExtension;
            //                Directory.CreateDirectory(path);


            //                foreach (var e in m.Messages)
            //                {
            //                    if (File.Exists(path + "/" + e.Key + ".m.cs") == false)
            //                    {
            //                        string imp = string.Format("using System;\r\n\r\n");
            //                        imp += string.Format("namespace {0} {{\r\n", info.Item3);
            //                        imp += string.Format("\tpublic partial class {0} {{\r\n", info.Item2);

            //                        imp += string.Format("\t\tpublic void OnMessage(Engine.Framework.INotifier notifier, global::{0}.{1} msg) {{\r\n", m.Package, e.Key);
            //                        imp += string.Format("\r\n\t\t}}\r\n");
            //                        imp += string.Format("\t}}\r\n");
            //                        imp += string.Format("}}\r\n");

            //                        Build(path, e.Key, imp);
            //                    }


            //                }
            //            }

            //            //pu.Update();

            //        }

            //    }


            //}

            //{
            //    string cosmos = "using System;\r\n";
            //    //cosmos += $"namespace Schema.Protobuf {{";


            //    foreach (var m in modules)
            //    {
            //        cosmos += $"namespace {m.Package} {{\r\n";

            //        foreach (var e in m.Messages)
            //        {
            //            cosmos += $"\tpublic sealed partial class {e.Key} {{\r\n";
            //            cosmos += $"\t\tpublic string id {{ get; set; }}\r\n";
            //            cosmos += $"\t\tpublic string _etag {{ get; set; }}\r\n";
            //            cosmos += $"\t\tpublic int _ts {{ get; set; }}\r\n";
            //            cosmos += $"\t}}\r\n";
            //        }

            //        cosmos += $"}}\r\n";
            //    }


            //    Build(output, "Cosmos", cosmos);

            //}

            {
                string api = "using System;\r\n";
                api += "using static Engine.Framework.Api;\r\n";
                api += string.Format("namespace Schema.Protobuf {{\r\n\r\n");
                api += string.Format("\tpublic static partial class Api {{\r\n");

                //api += string.Format("\t\tprivate delegate global::Engine.Framework.AsyncCallback Binder(global::Schema.Protobuf.IHandler handler, Engine.Framework.INotifier notifier, Google.Protobuf.CodedInputStream stream);\r\n");
                api += string.Format("\t\tpublic delegate void RuntimeBindException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e, dynamic handler, dynamic notifier, Type mag);\r\n");
                api += string.Format("\t\tpublic delegate void RuntimeBinderInternalCompilerException(Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e, dynamic handler, Type mag);\r\n");
                api += string.Format("\t\tpublic static RuntimeBindException RuntimeBindExceptionCallback = (e, handler, notifier, msg) => {{\r\n");
                api += string.Format("\t\t\tLogger.Info(string.Format(\"'{{0}}' has not handler for '{{1}}''{{2}}'\", handler.GetType(), notifier.GetType(), msg));\r\n");
                api += string.Format("\t\t}};\r\n");
                api += string.Format("\t\tpublic static RuntimeBinderInternalCompilerException RuntimeBinderInternalCompilerExceptionCallback = (e, handler, msg) => {{\r\n");
                api += string.Format("\t\t\tLogger.Info(string.Format(\"RuntimeBinderInternalCompilerException from '{{0}}'\", msg));\r\n");
                api += string.Format("\t\t\tLogger.Info(e);\r\n");
                api += string.Format("\t\t}};\r\n");


                api += string.Format("\t\tprivate delegate global::Google.Protobuf.IMessage Deserializer(global::System.IO.MemoryStream stream);\r\n");
                api += string.Format("\t\tprivate delegate global::Engine.Framework.AsyncCallback Binder(dynamic handler, dynamic notifier, global::System.IO.Stream stream);\r\n");
                api += string.Format("\t\tprivate static global::System.Collections.Generic.Dictionary<int, Deserializer> deserilizer = new System.Collections.Generic.Dictionary<int, Deserializer>();\r\n");
                api += string.Format("\t\tprivate static global::System.Collections.Generic.Dictionary<int, Binder> Binders = new global::System.Collections.Generic.Dictionary<int, Binder>();\r\n");
                api += string.Format("\t\tprivate static global::System.Collections.Generic.Dictionary<int, Type> types = new global::System.Collections.Generic.Dictionary<int, Type>();\r\n");
                api += string.Format("");
                api += string.Format("\t\tstatic public void StartUp() {{\r\n");

                foreach (var e in allMessages)
                {
                    api += string.Format("\t\t\tEngine.Framework.Id<{0}.{1}>.Value = 0x{2};  // {3}\r\n", e.Namespace, e.Name, e.Code.ToString("X8"), e.Code);
                }

                api += string.Format("\r\n");

                foreach (var e in allMessages)
                {
                    api += string.Format("\t\t\tBinders.Add({0}, (handler, notifier, stream) =>\r\n\t\t\t{{\r\n", e.Code);
                    api += string.Format("\t\t\t\tvar msg = {0}.{1}.Parser.ParseFrom(stream);\r\n", e.Namespace, e.Name);
                    api += string.Format("\t\t\t\treturn () => {{\r\n");
                    api += string.Format("\t\t\t\t\ttry {{ handler.OnMessage(notifier, msg); }}\r\n");
                    api += string.Format("\t\t\t\t\tcatch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {{\r\n");
                    api += string.Format("\t\t\t\t\t\tRuntimeBindExceptionCallback(e, handler, notifier, msg.GetType());\r\n");
                    api += string.Format("\t\t\t\t\t}}\r\n");
                    api += string.Format("\t\t\t\t\tcatch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {{\r\n");
                    api += string.Format("\t\t\t\t\t\tRuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());\r\n");
                    api += string.Format("\t\t\t\t\t}}\r\n");
                    api += string.Format("\t\t\t\t\tcatch {{ throw; }}\r\n");

                    api += string.Format("\t\t\t\t}};\r\n");
                    api += string.Format("\t\t\t}});\r\n");

                    //api += string.Format("\t\t\tBinders.Add(0x70000000 | {0}, (handler, notifier, stream) =>\r\n\t\t\t{{\r\n", e.Code);
                    //api += string.Format("\t\t\t\tvar msg = {0}.{1}.Parser.ParseFrom(stream);\r\n", e.Namespace, e.Name);
                    //api += string.Format("\t\t\t\treturn () => {{\r\n");
                    //api += string.Format("\t\t\t\t\ttry {{ handler.OnDelegate(notifier, msg); }}\r\n");
                    //api += string.Format("\t\t\t\t\tcatch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e) {{\r\n");
                    //api += string.Format("\t\t\t\t\t\tRuntimeBindExceptionCallback(e, handler, msg.GetType());\r\n");
                    //api += string.Format("\t\t\t\t\t}}\r\n");
                    //api += string.Format("\t\t\t\t\tcatch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderInternalCompilerException e) {{\r\n");
                    //api += string.Format("\t\t\t\t\t\tRuntimeBinderInternalCompilerExceptionCallback(e, handler, msg.GetType());\r\n");
                    //api += string.Format("\t\t\t\t\t}}\r\n");
                    //api += string.Format("\t\t\t\t\tcatch {{ throw; }}\r\n");

                    //api += string.Format("\t\t\t\t}};\r\n");
                    //api += string.Format("\t\t\t}});\r\n");

                    api += string.Format("\t\t\tdeserilizer.Add({0}, (stream) => {{\r\n", e.Code);
                    api += string.Format("\t\t\t\tvar msg = {0}.{1}.Parser.ParseFrom(stream);\r\n", e.Namespace, e.Name);
                    api += string.Format("\t\t\t\treturn msg;\r\n");
                    api += string.Format("\t\t\t}});\r\n");
                    api += $"\t\t\ttypes.Add({e.Code}, typeof({e.Namespace}.{e.Name}));{Environment.NewLine}";


                }

                api += string.Format("\t\t}}\r\n\r\n");

                api += string.Format("\t\tpublic static global::Engine.Framework.AsyncCallback Bind(dynamic handler, dynamic notifier, int code, global::System.IO.Stream stream) {{\r\n\r\n");

                api += string.Format("\t\t\tBinder binder = null;\r\n");
                api += string.Format("\t\t\tif (Binders.TryGetValue(code, out binder) == false) return () => {{ Console.WriteLine($\"Can not find code {{code}} binder.\"); }};\r\n\r\n");
                api += string.Format("\t\t\treturn binder(handler, notifier, stream);\r\n\r\n");

                api += string.Format("\t\t}}\r\n");

                //api += string.Format("\t\tpublic static global::Engine.Framework.AsyncCallback Delegate(dynamic handler, global::Engine.Framework.INotifier notifier, int code, global::System.IO.Stream stream) {{\r\n\r\n");

                //api += string.Format("\t\t\tBinder binder = null;\r\n");
                //api += string.Format("\t\t\tif (Binders.TryGetValue(0x70000000 | code, out binder) == false) return () => {{ Console.WriteLine($\"Can not find code {{code}} delegator.\"); }};\r\n\r\n");
                //api += string.Format("\t\t\treturn binder(handler, notifier, stream);\r\n\r\n");

                //api += string.Format("\t\t}}\r\n");

                api += string.Format("\t\tpublic static global::Google.Protobuf.IMessage Deserialize(int code, global::System.IO.MemoryStream stream) {{\r\n\r\n");
                api += string.Format("\t\t\tif (deserilizer.TryGetValue(code, out Deserializer callback) == true) {{\r\n");
                api += string.Format("\t\t\t\treturn callback(stream);\r\n");
                api += string.Format("\t\t\t}}\r\n");
                api += string.Format("\t\t\treturn null;\r\n");
                api += string.Format("\t\t}}\r\n");

                api += string.Format("\t\tpublic static Type CodeToType(int code) {{\r\n\r\n");
                api += string.Format("\t\t\tif (types.TryGetValue(code, out Type type) == true) {{\r\n");
                api += string.Format("\t\t\t\treturn type;\r\n");
                api += string.Format("\t\t\t}}\r\n");
                api += string.Format("\t\t\treturn null;\r\n");
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
