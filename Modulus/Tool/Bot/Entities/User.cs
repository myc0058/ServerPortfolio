using Google.Protobuf;
using Schema.Protobuf.Message.Authentication;
using System;
using System.Collections.Generic;
using static Engine.Framework.Api;

namespace Tool.Bot.Entities
{
    public partial class User : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }

        public User() : base(Singleton<Layer>.Instance)
        {

        }

        public new long UID
        {
            get
            {
                return base.UID;
            }
            set
            {
                base.UID = value;
            }
        }

        private static Dictionary<long, User> users = new Dictionary<long, User>();

        internal static User Get(long to)
        {
            lock (users)
            {
                users.TryGetValue(to, out User user);
                return user;
            }
        }

        public static void Add(User user)
        {

            lock (users)
            {
                users.Remove(user.UID);
                users.Add(user.UID, user);
            }
        }

        public static void Remove(User user)
        {

            lock (users)
            {
                users.Remove(user.UID);
            }
        }

        internal static void BroadCast<T>(T msg)
        {
            lock (users)
            {
                foreach (var e in users.Values)
                {
                    e.Client.Notify(msg);
                }
            }
        }

        public Protocol.Client Client { get; set; } = null;

        public Dictionary<int, IMessage> LastReceivedPacket { get; set; } = new Dictionary<int, IMessage>();
        public IMessage LastSendedPacket { get; set; } = null;
        public DateTime LastSendedTime { get; set; } = DateTime.UtcNow;

        public Login Account { get; set; } = null;
        
    }
}
