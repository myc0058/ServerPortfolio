using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Game.Entities;
using Engine.Framework;
using Google.Protobuf.Collections;
using Schema.Protobuf;
using System.Xml;
using static Engine.Framework.Api;
using System.Net;
using System.Diagnostics;
using System.Threading;
using dbms = Engine.Database.Management;
using Engine.Database;
using Application.Common;
using Application.Common.Scheduler;

namespace Application.Game
{

    public static partial class Api
    {
        public static bool StandAlone { get; set; } = false;
        public static long Idx => Engine.Network.Api.Idx;
        public static string LobbyGameIp = string.Empty;
        public static ushort LobbyGamePort = 0;

        public static void StartUp()
        {
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);


            Engine.Framework.Api.Add(Singleton<Entities.Room.Layer>.Instance);


            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;


            // connect delegate
            try
            {
                Engine.Framework.Api.Config.Seek(0, SeekOrigin.Begin);
                using (XmlReader reader = XmlReader.Create(Engine.Framework.Api.Config, readerSettings))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    var root = doc["root"];
                    
                    Console.WriteLine($"Game Idx = {Api.Idx}, Ip = {UInt32ToIPAddress((uint)Api.Idx)}");


                    var matchPort = root["Delegate"]["Game_Match"].Attributes["Port"].Value.ToUInt16();
                    var matchIp = root["Delegate"]["Game_Match"].Attributes["Ip"].Value;
                    Delegatables.Match.Instance.UID = Api.Idx;
                    Delegatables.Match.Instance.Connect(matchIp, matchPort);

                    var syncPort = root["Delegate"]["Game_Synchronize"].Attributes["Port"].Value.ToUInt16();
                    var syncIp = root["Delegate"]["Game_Synchronize"].Attributes["Ip"].Value;
                    Delegatables.Synchronize.Instance.UID = Api.Idx;
                    Delegatables.Synchronize.Instance.Connect(syncIp, syncPort);

                    {
                        var Lobby_GameRoom = root["Delegate"]["Lobby_GameRoom"];
                        LobbyGameIp = Lobby_GameRoom.Attributes["Ip"].Value;
                        LobbyGamePort = Lobby_GameRoom.Attributes["Port"].Value.ToUInt16();
                        Engine.Network.Api.Listen(LobbyGamePort, () =>
                        {
                            new Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>().Accept(LobbyGamePort);
                        });

                    }

                    var Lobby_Synchronize = root["Delegate"]["Lobby_Synchronize"];

                    Singleton<Scheduler.ServerStateSender>.Instance.SendServerState();
                    Singleton<Scheduler.ServerStateSender>.Instance.Run(1000 * 60);

                    Singleton<SyncTime>.Instance.Delegator = Delegatables.Synchronize.Instance;
                    Singleton<SyncTime>.Instance.Run(10);

                    Singleton<SyncUtil>.Instance.Delegator = Delegatables.Synchronize.Instance;
                    Singleton<SyncUtil>.Instance.Run(3000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("---------------- Game Server StartUp ----------------");
        }
    }
}
