using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.MockDedicateServer.Utils
{
    public class Config
    {
        public int PlayingSeconds { get; set; } = 5;
        public string GameServerIP { get; set; } = "127.0.0.1";
        public ushort GameServerPort { get; set; } = 7000;

        public static Config Instance { get; set; } = null;

        private Config() { }

        public static Config ReadFromFile()
        {
            try
            {
                string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() + "\\BotConfig.json"));
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(json);
            }
            catch
            {
                return new Config();
            }
        }

        public static bool SaveToFile()
        {
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Instance);
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory() + "\\BotConfig.json"), json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
