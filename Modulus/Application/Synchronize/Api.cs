using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using Engine.Database;
using Engine.Framework;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;

namespace Application.Synchronize
{
    public static partial class Api
    {
        public static int UID { get; private set; }
        public static bool StandAlone { get; set; } = false;
        public static long Idx { get; internal set; } = 0;
        private static int seed = 0;
        public static int GetUniqueKeySeed()
        {
            return Interlocked.Increment(ref seed);
        }

        public static void StartUp()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            seed = Convert.ToInt32((DateTime.UtcNow - epoch).TotalSeconds);
            Engine.Framework.Api.Offset = seed;
                
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments  = true;

            try
            {

                

                Engine.Framework.Api.Config.Seek(0, SeekOrigin.Begin);
                using (XmlReader reader = XmlReader.Create(Engine.Framework.Api.Config, readerSettings))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    var root = doc["root"];
                    

                    {
                        var Lobby_Synchronize = root["Delegate"]["Lobby_Synchronize"];
                        var lobbySyncPort = Lobby_Synchronize.Attributes["Port"].Value.ToUInt16();
                        {
                            
                            Engine.Network.Api.Listen(lobbySyncPort, () =>
                            {
                                new Engine.Network.Protocol.Delegator<Delegatables.Lobby>().Accept(lobbySyncPort);
                            });
                        }

                        lobbySyncPort++;

                        {
                            var port = Lobby_Synchronize.Attributes["Port"].Value.ToUInt16();
                            Engine.Network.Api.Listen(lobbySyncPort, () =>
                            {
                                new Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>().Accept(lobbySyncPort);
                            });
                        }

                        

                        {
                            var Match_Sync = root["Delegate"]["Match_Synchronize"];
                            var matchSyncPort = Match_Sync.Attributes["Port"].Value.ToUInt16();


                            Engine.Network.Api.Listen(matchSyncPort, () =>
                            {
                                new Engine.Network.Protocol.Delegator<Delegatables.Match>().Accept(matchSyncPort);
                            });

                        }

                        {
                            var Game_Sync = root["Delegate"]["Game_Synchronize"];
                            var gameSyncPort = Game_Sync.Attributes["Port"].Value.ToUInt16();


                            Engine.Network.Api.Listen(gameSyncPort, () =>
                            {
                                new Engine.Network.Protocol.Delegator<Delegatables.Game>().Accept(gameSyncPort);
                            });

                        }
                    }

                }

                Console.WriteLine("---------------- Synchronize Server StartUp ----------------");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CleanUp()
        {

        }
        
    }
}
