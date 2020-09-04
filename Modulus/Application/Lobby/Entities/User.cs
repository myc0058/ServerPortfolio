using Google.Protobuf.Collections;
using Schema.Protobuf.Message.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using static Engine.Framework.Api;

namespace Application.Lobby.Entities
{
    public partial class User : Engine.Framework.Layer.Task//, Engine.Framework.IDelegatable
    {

        public class Layer : Engine.Framework.Layer {}

        public User() : base(Singleton<Layer>.Instance)
        {
            
        }
        public static User Get(long to)
        {
            return Singleton<Engine.Framework.Container<long, User>>.Instance.Get(to);
        }

        public static void Add(User user)
        {
            Singleton<Engine.Framework.Container<long, User>>.Instance.Add(user.UID, user);
        }

        public static void Remove(User user)
        {
            Singleton<Engine.Framework.Container<long, User>>.Instance.Pop(user.UID);
        }

        internal static void BroadCast<T>(T msg)
        {
            
            lock (Singleton<Engine.Framework.Container<long, User>>.Instance)
            {
                foreach (var e in Singleton<Engine.Framework.Container<long, User>>.Instance.Values)
                {
                    e.Client?.Notify(msg);
                }
            }
        }

        new public void Close()
        {

            if (this == Singleton<User>.Instance)
            {
                return;
            }

            base.Close();
        }
        
        protected override void OnClose()
        {
            Client?.Disconnect();
            Client = null;
            Entities.User.Remove(this);
            Engine.Framework.Api.Logger.Info($"OnClose User {UID}");
            var msg = new global::Schema.Protobuf.Message.Authentication.Logout();
            msg.Idx = Account.Idx;

            MyMatchingInfo.Delegator?.Delegate(UID, MyMatchingInfo.RoomID, msg);
            Delegatables.Match.User.Instance.Delegate(UID, UID, msg);
            Delegatables.Synchronize.User.Instance.Delegate(UID, UID, msg);

            MyMatchingInfo.RoomID = 0;
            MyMatchingInfo.Delegator = null;
        }

        public void OnDisconnect()
        {
            Close();
        }

        public Protocol.Client Client;
        public Basis.Data.Account Account = new Basis.Data.Account();


        public class MatchingInfo
        {
            public long GameServerID { get; set; }
            public long RoomID { get; set; }
            public bool EnterRoom { get; set; } = false;
            public Engine.Network.Protocol.Delegator<Delegatables.Game.Room> Delegator { get; set; }
            public void Clear()
            {
                Delegator = null;
                GameServerID = 0;
                RoomID = 0;
                EnterRoom = false;
            }
        }

        public MatchingInfo MyMatchingInfo { get; set; } = new MatchingInfo();


    }
}
