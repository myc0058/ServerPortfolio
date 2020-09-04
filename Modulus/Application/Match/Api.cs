using System;
using System.IO;
using System.Net;
using System.Xml;
using Engine.Framework;
using static Engine.Framework.Api;
using dbms = Engine.Database.Management;
using Engine.Database;
using Basis.Data;
using System.Collections.Generic;
using Application.Match.Scheduler;
using Application.Common;
using Application.Common.Scheduler;

namespace Application.Match
{
    public static partial class Api
    {
        public static long Idx => Engine.Network.Api.Idx;
        
        public static bool StandAlone { get; set; } = false;

        public static void StartUp()
        {
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

                    {
                        var Game_Match = root["Delegate"]["Game_Match"];
                        {
                            var port = Game_Match.Attributes["Port"].Value.ToUInt16();
                            Engine.Network.Api.Listen(port, 128, () =>
                            {
                                var delegator = new Engine.Network.Protocol.Delegator<Delegatables.Game>();
                                delegator.Accept(port);
                            });

                        }

                        var Lobby_Match = root["Delegate"]["Lobby_Match"];
                        {
                            var port = Lobby_Match.Attributes["Port"].Value.ToUInt16();

                            Engine.Network.Api.Listen((ushort)(port), 128, () =>
                            {
                                var delegator = new Engine.Network.Protocol.Delegator<Delegatables.Lobby>();
                                delegator.Accept((ushort)(port));
                            });

                            port++;

                            Engine.Network.Api.Listen((ushort)(port), 128, () =>
                            {
                                var delegator = new Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>();
                                delegator.Accept((ushort)(port));
                            });                            
                        }


                        var Match_Sync = root["Delegate"]["Match_Synchronize"];
                        var matchSyncPort = Match_Sync.Attributes["Port"].Value.ToUInt16();
                        var matchSyncIp = Match_Sync.Attributes["Ip"].Value;

                        Delegatables.Synchronize.Instance.UID = Api.Idx;
                        Delegatables.Synchronize.Instance.Connect(matchSyncIp, matchSyncPort);
                        
                        Delegatables.Agent.Instance.UID = Engine.Framework.Api.UniqueKey;
                        var info = new Schema.Protobuf.Message.Administrator.ConnectedAgentInfo();
                        info.ServerTypes.Add("Match");
                        info.Address = Delegatables.Agent.Instance.UID;
                        Delegatables.Agent.Instance.ConnectStream = info.ToMemoryStream();
                        
                        Delegatables.Agent.Instance.Connect("127.0.0.1", 9880);
                    }


                }


                Singleton<Scheduler.MatchMaker>.Instance.Run(1000);
                Singleton<SyncTime>.Instance.Delegator = Delegatables.Synchronize.Instance;
                Singleton<SyncTime>.Instance.Run(10);

                Singleton<SyncUtil>.Instance.Delegator = Delegatables.Synchronize.Instance;
                Singleton<SyncUtil>.Instance.Run(3000);

                Engine.Framework.Api.Logger.Info("---------------- Match Server StartUp ----------------");


            }
            catch (Exception e)
            {
                Engine.Framework.Api.Logger.Info(e);
            }
        }

        public static void CleanUp()
        {

        }
    }
}
