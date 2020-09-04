using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.Bot.Utils
{
    public class Config
    {
        public int StartIndex { get; set; } = 0;
        public int ClientCount { get; set; } = 1;
        public string LobbyServerIP { get; set; } = "127.0.0.1";
        public ushort LobbyServerPort { get; set; } = 4081;
        public string ClientIDPrefix { get; set; } = "Bot";
        public bool RandomCancelMatching { get; set; } = true;

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
