using Schema.Protobuf.Message.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tool.Bot.Utils;

namespace Tool.Bot.Protocol
{
    public class BotClient : Client, INotifyPropertyChanged
    {
        override protected void onDisconnect()
        {
            base.onDisconnect();
            Remark += "Disconnect!!!";
        }

        public static void UpdateAll()
        {
            lock (clientList)
            {
                Parallel.ForEach(clientList, options, (client) =>
                {
                    var botClient = (BotClient)client;
                    botClient.Update();
                });
            }
        }

        public void Update()
        {
            if (user == null) { return; }

            if ((int)State < (int)UserState.ReceivedEncript) { return; }
            
            try
            {
                if (WaitingResponse == true)
                {
                    switch (State)
                    {
                        default:
                            return;
                    }
                }
                else
                {
                    switch (State)
                    {
                        case UserState.ReceivedEncript :
                            {
                                Clear();
                                var login = new Login();
                                login.Id = UserID;
                                login.ClientPlatform = Schema.Protobuf.Message.Enums.ClientPlatform.Editor;
                                State = UserState.Login;
                                user.LastSendedPacket = login;
                                SetWaitingResponse(State, true);
                                user.LastSendedTime = DateTime.UtcNow;

                                Notify(login);
                                /*
                                MemoryStream stream = new MemoryStream();
                                SerializeProtobufTo(stream, 0, login);
                                base.Write(stream);
                                */
                            }
                            break;
                        
                        case UserState.Login:
                            {
                                var msg = new Schema.Protobuf.Message.Game.BeginMatching();
                                State = UserState.BeginMatching;
                                user.LastSendedPacket = msg;
                                SetWaitingResponse(State, true);
                                user.LastSendedTime = DateTime.UtcNow;
                                MemoryStream stream = new MemoryStream();
                                SerializeProtobufTo(stream, 0, msg);
                                base.Write(stream);
                            }
                            break;
                        case UserState.BeginMatching:
                            {
                                State = UserState.ReceivedEndMatching;
                                SetWaitingResponse(State, true);
                            }
                            break;
                        case UserState.ReceivedEndMatching:
                            {
                                var msg = new Schema.Protobuf.Message.Game.EnterRoom();
                                State = UserState.EnterRoom;
                                user.LastSendedPacket = msg;
                                SetWaitingResponse(State, true);
                                user.LastSendedTime = DateTime.UtcNow;
                                MemoryStream stream = new MemoryStream();
                                SerializeProtobufTo(stream, 0, msg);
                                base.Write(stream);
                            }
                            break;
                        case UserState.EnterRoom:
                            {
                                var msg = new Schema.Protobuf.Message.Game.Chat();
                                msg.Idx = user.Account.Idx;
                                msg.Id = user.Account.Id;
                                msg.Msg = "first chat to room";

                                MemoryStream stream = new MemoryStream();
                                SerializeProtobufTo(stream, 0, msg);
                                base.Write(stream);

                                State = UserState.ReceivedMyGameResult;
                                SetWaitingResponse(State, true);
                            }
                            break;
                        case UserState.ReceivedMyGameResult:
                            {
                                State = UserState.ReceivedFinishGame;
                                SetWaitingResponse(State, true);
                            }
                            break;

                        case UserState.ReceivedFinishGame:
                            {

                                var logout = new Schema.Protobuf.Message.Lobby.Logout();

                                State = UserState.Logout;
                                user.LastSendedPacket = logout;
                                SetWaitingResponse(State, true);
                                user.LastSendedTime = DateTime.UtcNow;

                                MemoryStream stream = new MemoryStream();
                                SerializeProtobufTo(stream, 0, logout);
                                base.Write(stream);
                            }
                            break;

                        case UserState.Logout:
                            {
                                State = UserState.ReceivedEncript;
                                SetWaitingResponse(State, false);
                            }
                            break;
                    }
                
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void Clear()
        {
            user.LastReceivedPacket.Clear();
            user.LastSendedPacket = null;
            waitingResponses.Clear();
        }

        public enum UserState
        {
            None = 0,
            ReceivedEncript,
            Login,
            BeginMatching,
            ReceivedEndMatching,
            EnterRoom,
            ReceivedMyGameResult,
            ReceivedFinishGame,
            Logout,
            Idle,
            Disconnected
        }

        private long index;
        public long Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
                UserID = Config.Instance.ClientIDPrefix + "_" + index.ToString();
            }
        }

        public string No
        {
            get { return index.ToString(); }
        }


        private long idx = 0;
        public long Idx
        {
            get { return idx; }
            set { idx = value; OnPropertyChanged("Idx"); }
        }

        public string UserID { get; set; }

        public DateTime StartIdleTime { get; set; } = DateTime.UtcNow;

        public bool SendedCancelMatching { get; set; } = false;

        private UserState userState = UserState.None;
        public UserState State
        {
            get { return userState; }
            set
            {
                if (userState != value && value == UserState.Idle)
                {
                    StartIdleTime = DateTime.UtcNow;
                }
                userState = value;
                OnPropertyChanged("State");
                OnPropertyChanged("WaitingResponse");
            }
        }

        private Dictionary<UserState, bool> waitingResponses = new Dictionary<UserState, bool>();

        public bool GetWaitingResponse(UserState userState)
        {
            return waitingResponses[userState];
        }

        public void SetWaitingResponse(UserState userState, bool needWaiting)
        {
            bool result = false;
            if (waitingResponses.TryGetValue(userState, out result) == true)
            {
                if (result == false)
                    return;
            }

            waitingResponses[userState] = needWaiting;
            OnPropertyChanged("WaitingResponse");
        }

        public bool WaitingResponse
        {
            get
            {
                bool result = false;
                if (waitingResponses.TryGetValue(State, out result) == true)
                    return waitingResponses[State];
                else
                    return false;
            }
        }


        private int playCount = 0;
        public int PlayCount
        {
            get
            {
                return playCount;
            }
            set
            {
                playCount = value;
                OnPropertyChanged("PlayCount");
            }
        }

        public enum ClientType
        {
            FFANormal = 0,
            TrioNormal = 1,
            TagNormal = 2,
            Max = 3,
        }

        private ClientType type = ClientType.FFANormal;
        public ClientType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        private string remark = "";
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
