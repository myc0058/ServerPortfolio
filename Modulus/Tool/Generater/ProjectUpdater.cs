using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Generater
{
    public class ProjectUpdater
    {
        public string ProjectPath = "";
        public List<string> CsFiles = null;
        public List<string> Remove = new List<string>();
        public void Update()
        {
            if (ProjectPath == "")
            {
                return;
            }


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ProjectPath);

                XmlNode node = doc["Project"]["ItemGroup"];

                while (node != null)
                {

                    var compile = node["Compile"];
                    if (compile == null)
                    {
                        node = node.NextSibling;
                        continue;
                    }

                    break;

                }

                string ns = doc["Project"].NamespaceURI;

                var dic = new HashSet<string>();

                foreach (var e in CsFiles)
                {
                    dic.Add(e);
                }

                var remove = new List<XmlNode>();
                foreach (XmlNode n in node.ChildNodes)
                {

                    foreach (var e in Remove)
                    {
                        if (n.Attributes["Include"].Value.StartsWith(e) == true)
                        {
                            remove.Add(n);
                        }
                    }

                    //string filename = Path.GetFileNameWithoutExtension(n.Attributes["Include"].Value);
                    //dic.Remove(filename);

                }

                foreach (var e in remove)
                {
                    node.RemoveChild(e);
                }


                foreach (var e in CsFiles)
                {
                    var compile = doc.CreateElement("Compile", ns);
                    var attribute = doc.CreateAttribute("Include");
                    attribute.Value = e;
                    compile.Attributes.Append(attribute);
                    node.AppendChild(compile);
                }

                if (CsFiles.Count > 0)
                {
                    doc.Save(ProjectPath);
                }


            }
            catch
            {
            }

        }

    }
}
