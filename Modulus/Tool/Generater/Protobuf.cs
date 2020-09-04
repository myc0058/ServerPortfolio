using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Generater
{
    public static class Protobuf
    {
        public static string Source = string.Empty;
        public abstract class Buildable
        {
            public virtual void Initialize(string input) { }
            public abstract void Parse(string input, string output);
            public virtual void UnitTest(string output) { }
            public class Message
            {
                public string Namespace { get; set; }
                public string Name { get; set; }
                public string Contents { get; set; }
                public int Code { get; set; }
            }

            protected Generater.Protobuf.Lexer lexer = new Generater.Protobuf.Lexer();
            public List<Protobuf.Implement> Implements = new List<Protobuf.Implement>();
        }
        public class Implement
        {
            public string Project;
            public List<Tuple<string, string, string>> Infos = new List<Tuple<string, string, string>>();
        }


        public static string Compiler = string.Empty;

        public static string Root { get; internal set; }

        public class Lexer
        {
            string[] lines;
            int line = 0;
            Queue<string> tokens = new Queue<string>();

            internal bool Read(string filename)
            {
                string text = "";
                using (var stream = File.OpenRead(filename))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        text = streamReader.ReadToEnd();
                    }
                }

                var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
                re = Regex.Replace(text, re, "$1");
                lines = new string[] { re };
                return true;
            }
            private void Tokenize()
            {
                while (tokens.Count == 0)
                {

                    if (lines.Length > line)
                    {
                        var splited = lines[line].Split(new char[] { ' ', '\t', '\r', '\n', ';' });
                        line++;
                        if (splited == null || splited.Length == 0)
                        {
                            continue;
                        }

                        foreach (var token in splited)
                        {
                            if (string.IsNullOrEmpty(token) == true)
                            {
                                continue;
                            }
                            tokens.Enqueue(token);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            internal string GetToken()
            {
                Tokenize();
                if (tokens.Count == 0) { return null; }
                return tokens.Dequeue();
            }

            internal string GetPackage()
            {
                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return null; }

                    switch (token)
                    {
                        case "package":
                            token = GetToken();
                            break;
                        default:
                            continue;
                    }
                    return token;
                }
            }

            internal string GetMessage()
            {
                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return null; }

                    switch (token)
                    {
                        case "message":
                            token = GetToken();
                            GoToEndOfScope();
                            return token;
                    }

                }
            }

            internal string GetEnum()
            {
                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return null; }

                    switch (token)
                    {
                        case "enum":
                            token = GetToken();
                            if (token == "class")
                            {
                                token = GetToken();
                            }
                            GoToBeginOfScope();
                            return token;
                    }

                }
            }

            internal string GetUnrealEnumElements()
            {
                string enums = "";
                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return null; }

                    
                    switch (token)
                    {
                        case "}":
                            return enums;
                        default:

                            var tokens = token.Split('=');


                            if (tokens != null && tokens.Length > 0)
                            {
                                foreach (var e in tokens)
                                {
                                    if (string.IsNullOrWhiteSpace(e) == true) { continue; }
                                    enums += e;
                                    enums += ";";
                                }
                            }
                            else
                            {
                                enums += token;
                            }

                            
                            
                            if (token.Contains(",") == false)
                            {
                                enums += ";";
                            }
                            break;
                    }
                }
            }

            internal List<string> GetProtobufEnumElements()
            {
                List<string> enums = new List<string>();

                int count = 0;

                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return null; }

                    switch (token)
                    {
                        case "}":
                            return enums;
                        default:
                            if (count == 0)
                            {
                                enums.Add(token);
                            }
                            count += 1;
                            if (count % 3 == 0)
                            {
                                count = 0;
                            }
                            break;
                    }
                }
            }
            public string LastTokens { get; private set; } = string.Empty;
            public bool HasNestMessage = false;
            internal void GoToEndOfScope()
            {
                int scope = 0;
                LastTokens = string.Empty;
                HasNestMessage = false;
                while (true)
                {
                    var token = GetToken();
                    if (string.IsNullOrEmpty(token)) { return; }

                    switch (token)
                    {
                        case "message":
                            {
                                LastTokens += $"{token}\n";
                                HasNestMessage = true;
                            }
                            break;
                        case "{":
                            ++scope;
                            break;
                        case "}":
                            --scope;
                            if (scope < 0)
                            {
                                return;
                            }
                            if (scope == 0)
                            {
                                return;
                            }

                            break;
                        default:
                            LastTokens += $"{token}\n";
                            break;

                    }
                }
            }
            internal void GoToBeginOfScope()
            {
                while (true)
                {
                    var token = GetToken();
                    if (string.IsNullOrEmpty(token)) { return; }

                    switch (token)
                    {
                        case "{":
                            return;
                    }
                }
            }

            internal string GetImport()
            {
                while (true)
                {
                    var token = GetToken();

                    if (string.IsNullOrEmpty(token)) { return string.Empty; }

                    if (token.ToLower() == "import")
                    {
                        token = GetToken();
                        return token;
                    }
                }
            }
        }

        public class Dependency : IComparer<Dependency>, IComparable<Dependency>
        {
            public string Source = string.Empty;
            public List<string> Dependencies = new List<string>();
            public string Package = string.Empty;
            public Dictionary<string, int> Messages = new Dictionary<string, int>();


            public int CompareTo(Dependency other)
            {
                if (Dependencies.Count == 0)
                {
                    if (other.Dependencies.Count == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (other.Dependencies.Count == 0)
                {
                    return 1;
                }

                foreach (var e in Dependencies)
                {
                    if (e == string.Format("\"{0}\"", Path.GetFileName(other.Source)))
                    {
                        // nead other;
                        // other is greater;
                        return 1;
                    }
                }

                foreach (var e in other.Dependencies)
                {
                    if (e == string.Format("\"{0}\"", Path.GetFileName(Source)))
                    {
                        // nead other;
                        // other is greater;
                        return -1;
                    }
                }

                return 0;

            }


            public int Compare(Dependency x, Dependency y)
            {
                if (x.Dependencies.Count == 0)
                {
                    if (y.Dependencies.Count == 0)
                    {
                        // equal;
                        return 0;
                    }
                    else
                    {
                        // y is greater;
                        return -1;
                    }
                }
                else if (y.Dependencies.Count == 0)
                {
                    if (x.Dependencies.Count == 0)
                    {
                        // equal;
                        return 0;
                    }
                    else
                    {
                        // x is greater;
                        return 1;
                    }
                }


                return 0;
                //return x.Dependencies.Count.CompareTo(y.Dependencies.Count);
            }
        }

        public class Output
        {
            public string Type = string.Empty;
            public string Path = string.Empty;
            public List<Implement> Handler = new List<Implement>();

            public string UnitTest { get; internal set; } = string.Empty;
        }

        public static void Compile(string input, List<Output> outputs)
        {
            var files = Directory.GetFiles(input, "*.proto", SearchOption.AllDirectories);

            List<Dependency> all = new List<Dependency>();

            foreach (var e in files)
            {
                var dependency = new Dependency();
                var lexer = new Lexer();
                lexer.Read(e);

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
                all.Add(dependency);
                Console.WriteLine("Add " + e);
            }

            List<System.Diagnostics.Process> processes = new List<Process>();

            all.Sort();

            processes.Clear();
            // for cs
            {
                foreach (var e in all)
                {

                    System.Diagnostics.Process ps = new System.Diagnostics.Process();
                    ps.StartInfo.Arguments = string.Format("--proto_path={0} --csharp_out={1} --csharp_opt=file_extension=.g.cs {2}", input, input, Path.GetFullPath(e.Source));
                    ps.StartInfo.UseShellExecute = false;
                    ps.StartInfo.RedirectStandardOutput = true;
                    ps.StartInfo.FileName = Compiler;
                    ps.OutputDataReceived += (object sender, DataReceivedEventArgs arg) =>
                    {
                        Console.WriteLine(arg.Data);
                    };

                    ps.Start();
                    processes.Add(ps);
                }
                while (true)
                {
                    foreach (var p in processes)
                    {
                        if (p.HasExited == false)
                        {
                            p.WaitForExit(100);
                            continue;
                        }
                    }

                    break;
                }

                foreach (var p in processes)
                {
                    var stream = p.StandardOutput;
                    stream = null;
                }

                files = Directory.GetFiles(input, "*.g.cs", SearchOption.AllDirectories);

                foreach (var f in files)
                {
                    var bytes = File.ReadAllText(f);

                    foreach (var o in outputs)
                    {
                        if (o.Path == input) { continue; }

                        Directory.CreateDirectory(o.Path);
                        Directory.CreateDirectory(o.Path + "/Message/");

                        using (var stream = File.CreateText(o.Path + "/Message/" + Path.GetFileName(f)))
                        {
                            stream.Write(bytes);
                        }
                    }
                    File.Delete(f);
                }

                foreach (var o in outputs)
                {
                    if (o.Type == "CSharp")
                    {
                        var csharp = new Schema.CSharp();
                        csharp.Implements = o.Handler;
                        csharp.Initialize(input);
                        csharp.Parse(input, o.Path);

                        if (string.IsNullOrEmpty(o.UnitTest) == false)
                        {
                            csharp.UnitTest(o.UnitTest);
                        }
                    }
                    else if (o.Type == "Unity")
                    {
                        var unity = new Schema.Unity();
                        unity.Implements = o.Handler;
                        unity.Initialize(input);
                        unity.Parse(input, o.Path);
                    }
                }


            }
        }
    }
}
