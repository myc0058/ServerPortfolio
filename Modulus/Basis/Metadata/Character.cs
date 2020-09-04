using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Engine.Framework;
using System.Collections;

namespace Basis.Metadata
{
    [Engine.Framework.Attributes.Metadata(type: Engine.Framework.Attributes.Metadata.Type.Json, callback: "Build", filename: "Character")]
    public class Character : Engine.Framework.Repository.Metadata
    {
        [Engine.Framework.Repository.Metadata.PrimaryKey]
        public int Id;
        public string Name;

        public override object CustomLoad(StreamReader reader)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Character>>(reader.ReadToEnd());
            return obj;
        }

        public static void Build()
        {
        }

        public static string Generate()
        {
            List<Character> list = new List<Character>();
            list.Add(new Character()
            { 
                Id = 1,
                Name = "하리"
            });

            list.Add(new Character()
            {
                Id = 2,
                Name = "신비"
            });

            list.Add(new Character()
            {
                Id = 3,
                Name = "은비"
            });

            string result = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return result;
        }
    }
}
