using Google.Protobuf;
using Schema.Protobuf.Message.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Engine.Framework.Api;
using static Tool.MockDedicateServer.Protocol.Client;

namespace Tool.MockDedicateServer.Entities
{
    public partial class Game : Engine.Framework.Layer.Task
    {
        public class Layer : Engine.Framework.Layer { }

        public Game() : base(Singleton<Layer>.Instance)
        {

            var values = Enum.GetValues(typeof(DedicateState));

            foreach(DedicateState item in values)
            {
                WaitingResponses[item] = false;
            }
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

        public Protocol.Client Client { get; set; } = null;

        public Dictionary<int, IMessage> LastReceivedPacket = new Dictionary<int, IMessage>();
        public Dictionary<long, EnterBattle> ReceivedEnterBattle = new Dictionary<long, EnterBattle>();
        public IMessage LastSendedPacket = null;
        public DateTime LastSendedTime = DateTime.UtcNow;
        public Dictionary<DedicateState, bool> WaitingResponses = new Dictionary<DedicateState, bool>();

        public int EnteredPlayerCount { get; set; } = 0;
        public DateTime WaitingAllPlayerTimeout;
        public DateTime PlayingTimeout;
    }
}
