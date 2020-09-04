using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Generater
{
    class Program
    {

        static void Main(string[] args)
        {

            //myc0058 Config.xml change relative path
            string input = Directory.GetCurrentDirectory();

            XmlDocument doc = new XmlDocument();

            string config = "Generater.Config.xml";
            using (var stream = typeof(Generater.Program).Assembly.GetManifestResourceStream(config))
            {
                doc.Load(stream);
            }

            if (string.IsNullOrWhiteSpace(doc["root"]["Protobuf"].Attributes["Root"].Value) == false)
            {
                Protobuf.Root = doc["root"]["Protobuf"].Attributes["Root"].Value;
            }
            
            using (StreamWriter sw = File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "Workspace.txt")))
            {
                Console.WriteLine(Protobuf.Root);
            }

            
            string protoc = doc["root"]["Protobuf"].Attributes["Protoc"].Value;


            Console.WriteLine($"Protoc Path : {protoc}");

            Generater.Protobuf.Compiler = protoc;

            List<Generater.Protobuf.Output> outputs = new List<Generater.Protobuf.Output>();

            try
            {
                input = doc["root"]["Protobuf"]["Input"].Attributes["Path"].Value;
                Protobuf.Source = input;
            }
            catch
            {

            }

            try
            {
                var node = doc["root"]["Protobuf"]["Compile"];
                foreach (XmlElement e in node.ChildNodes)
                {
                    string type = e.Attributes["Type"].Value;
                    string path = e.Attributes["Path"].Value;

                    

                    var o = new Generater.Protobuf.Output();
                    o.UnitTest = e.Attributes["UnitTest"]?.Value;
                    {
                        foreach (XmlElement element in e.ChildNodes)
                        {
                            var project = element.Attributes["Project"].Value;

                            var imp = new Protobuf.Implement();
                            o.Handler.Add(imp);
                            imp.Project = project;
                            foreach (XmlElement h in element.ChildNodes)
                            {
                                var ns = h.Attributes["Path"].Value;
                                var classname = h.Attributes["ClassName"].Value;
                                var @namespace = h.Attributes["Namespace"].Value;
                                imp.Infos.Add(new Tuple<string, string, string>(ns, classname, @namespace));

                            }
                        }
                    }

                    o.Type = type;
                    o.Path = Path.Combine(Protobuf.Root, path);

                    outputs.Add(o);
                }
            }
            catch
            {

            }


            Generater.Protobuf.Compile(Path.Combine(Protobuf.Root, input), outputs);

            Console.WriteLine("Done.");

            Console.ReadLine();
        }
    }
}
