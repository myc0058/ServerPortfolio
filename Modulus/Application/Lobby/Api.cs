using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Engine.Framework;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;
using System.Linq;
using Application.Lobby.Entities;
using Application.Lobby.Protocol;
using Amazon.DynamoDBv2.Model;
using Application.Common;
using Application.Common.Scheduler;

namespace Application.Lobby
{
    public static class Api
    {
        public static class Extension
        {
        };

        public static long Idx => Engine.Network.Api.Idx;

        public static bool StandAlone { get; set; } = false;
        public static int HeartBeatInterval { get; set; } = 15;

        public static void StartUp() {


            var metadataWatcher = new Engine.Framework.Api.MetadataWatcher();
            metadataWatcher.Path = Path.Combine(Directory.GetCurrentDirectory(), "..", "Metadata");
            metadataWatcher.Filter = "*.json";

            Engine.Framework.Api.AddWatcher(metadataWatcher);


            

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            try
            {
                Engine.Framework.Api.Config.Seek(0, SeekOrigin.Begin);
                using (XmlReader reader = XmlReader.Create(Engine.Framework.Api.Config, readerSettings))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    var root = doc["root"];

                    Console.WriteLine($"Lobby Idx = {Api.Idx}, Ip = {UInt32ToIPAddress((uint)Api.Idx)}");
                    Engine.Framework.Api.Add(Singleton<Entities.User.Layer>.Instance);
                    
                    var Lobby_Synchronize = root["Delegate"]["Lobby_Synchronize"];
                    var Lobby_GameRoom = root["Delegate"]["Lobby_GameRoom"];
                    var Lobby_Match = root["Delegate"]["Lobby_Match"];

                    var lobbySyncIp = Lobby_Synchronize.Attributes["Ip"].Value;
                    var lobbySyncPort = Lobby_Synchronize.Attributes["Port"].Value.ToUInt16();

                    var lobbyGameIps = Lobby_GameRoom.Attributes["Ip"].Value.Split(',');
                    var lobbyGamePort = Lobby_GameRoom.Attributes["Port"].Value.ToUInt16();

                    var lobbyMatchIp = Lobby_Match.Attributes["Ip"].Value;
                    var lobbyMatchPort = Lobby_Match.Attributes["Port"].Value.ToUInt16();

                    Delegatables.Synchronize.Instance.UID = Api.Idx;
                    Delegatables.Synchronize.Instance.Connect(lobbySyncIp, lobbySyncPort);

                    //myc0058 IP XML파일에서 똑바로 가져오도록 수정한다.
                    Delegatables.Synchronize.User.Instance.UID = Api.Idx;
                    Delegatables.Synchronize.User.Instance.Connect(lobbySyncIp, (ushort)(lobbySyncPort + 1));
                    
                    //Delegatables.Match.Instance.UID = Api.Idx;
                    //Delegatables.Match.Instance.Connect(lobbyMatchIp, lobbyMatchPort);

                    Delegatables.Match.User.Instance.UID = Api.Idx;
                    Delegatables.Match.User.Instance.Connect(lobbyMatchIp, (ushort)(lobbyMatchPort + 1));

                    foreach (var item in lobbyGameIps)
                    {
                        //myc0058 이렇게 말고 delegator의 UID를 상대방이 알순 없는지 다시 확인한다.
                        //참조 Application.Match.Delegatables.Lobby.User

                        long id = Engine.Framework.Api.AddressToInt64(item, lobbyGamePort);
                        var delegator = Engine.Network.Protocol.Delegator<Delegatables.Game.Room>.Create(id);
                        delegator.UID = Api.Idx;
                        delegator.Connect(item, lobbyGamePort);
                    }

                    var Client_Lobby = root["Delegate"]["Client_Lobby"];
                    var port = Client_Lobby.Attributes["Port"].Value.ToUInt16();
                    Engine.Network.Api.Listen(port, () => {
                        var client = new Application.Lobby.Protocol.Client();
                        client.Accept(port);
                    });

                    Delegatables.Agent.Instance.UID = Engine.Framework.Api.UniqueKey;
                    var info = new Schema.Protobuf.Message.Administrator.ConnectedAgentInfo();
                    info.ServerTypes.Add(Schema.Protobuf.CSharp.Enums.EServer.Lobby.ToString());
                    info.Address = Delegatables.Agent.Instance.UID;
                    Delegatables.Agent.Instance.ConnectStream = info.ToMemoryStream();
                    Delegatables.Agent.Instance.Connect("127.0.0.1", 9880);
                    
                }

                if (StandAlone == false)
                {
                    Singleton<Scheduler.HeartBeat>.Instance.Run(10000);
                }

                Singleton<SyncTime>.Instance.Delegator = Delegatables.Synchronize.Instance;
                Singleton<SyncTime>.Instance.Run(10);

                Singleton<SyncUtil>.Instance.Delegator = Delegatables.Synchronize.Instance;
                Singleton<SyncUtil>.Instance.Run(3000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }



            Console.WriteLine("---------------- Lobby Server StartUp ----------------");
        }

        public static void BroadCast<T>(T msg) where T : Google.Protobuf.IMessage
        {
            Protocol.Client.BroadCast(msg);
        }
    }
}
