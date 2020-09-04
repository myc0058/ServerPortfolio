using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Engine.Framework;
using Google.Protobuf;
using Schema.Protobuf.Message.Action;
using Schema.Protobuf.Message.Authentication;
using Schema.Protobuf.Message.Statistics;
using Schema.Protobuf.Message.Synchronize;
using Tool.MockDedicateServer.Entities;
using Tool.MockDedicateServer.Utils;

namespace Tool.MockDedicateServer.Protocol
{
    public class Client : Engine.Network.Protocol.Tcp, Engine.Framework.INotifier
    {
        public class Notifier : Engine.Framework.INotifier
        {
            public virtual void Response<T>(T msg)
            {
                var serializer = new Serializer<T>();
                serializer.Sequence = Sequence;
                serializer.Message = msg;
                Protocol.Write(serializer);
            }
            public virtual void Notify<T>(T msg)
            {
                var serializer = new Serializer<T>();
                serializer.Message = msg;
                Protocol.Write(serializer);
            }

            public long Sequence { get; set; }
            public Engine.Network.Protocol.Tcp Protocol;
        }

        public virtual void Response<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }
        public virtual void Notify<T>(T msg)
        {
            var serializer = new Serializer<T>();
            serializer.Message = msg;
            base.Write(serializer);
        }

        public static void SerializeProtobufTo<T>(Stream output, long sequence, T msg)
        {
            var proto = msg as Google.Protobuf.IMessage;

            output.Write(BitConverter.GetBytes(proto.CalculateSize() + sizeof(int) + sizeof(int) + sizeof(long)), 0, 4);
            output.Write(BitConverter.GetBytes(Engine.Framework.Id<T>.Value), 0, 4);
            output.Write(BitConverter.GetBytes(sequence), 0, 8);
            proto.Serialize(output, true);

        }

        public class Serializer<T> : Engine.Framework.ISerializable
        {
            public void Serialize(Stream output)
            {
                SerializeProtobufTo<T>(output, Sequence, Message);
            }
            public int Length
            {
                get
                {
                    if (length == 0)
                    {
                        var proto = Message as Google.Protobuf.IMessage;
                        length = proto.CalculateSize() + sizeof(int) + sizeof(int) + sizeof(long);
                    }
                    return length;
                }
            }
            protected int length { get; set; }
            public T Message;
            public long Sequence { get; set; }
        }

        public Client()
        {
            OnRead = onRead;
            OnConnect = onConnect;
            OnDisconnect = onDisconnect;

            UseCompress = true;
        }

        private void onConnect(bool ret)
        {
            if (ret == true)
            {
                State = DedicateState.Connected;
                Console.WriteLine("Connected with Server!!");
            }
            else
            {
                Console.WriteLine("Connect Fail to Server !!");
            }
        }

        protected override void Defragment(MemoryStream transferred)
        {
            var buffer = transferred.GetBuffer();

            int blockSize = 0;
            int readBytes = 0;

            while (transferred.Length - readBytes > sizeof(int))
            {
                blockSize = BitConverter.ToInt32(buffer, readBytes);
                if (blockSize < 1 || blockSize > RecvBufferSize)
                {
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Disconnect();
                    return;
                }

                if (blockSize + readBytes > transferred.Length) { break; }

                Stream stream = new MemoryStream(buffer, readBytes + 8, blockSize - 8, true, true);
                readBytes += blockSize;


                CryptoStream csEncrypt = null;
                if (aesAlg != null)
                {
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    csEncrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
                    stream = csEncrypt;
                }

                GZipStream compressionStream = null;
                if (UseCompress)
                {
                    compressionStream = new GZipStream(stream, CompressionMode.Decompress, true);
                    stream = compressionStream;
                }


                MemoryStream result = new MemoryStream();

                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);

                try
                {
                    OnRead?.Invoke(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("OnRead Exception " + e);
                }


            }

            transferred.Seek(readBytes, SeekOrigin.Begin);

        }

        private void onDisconnect()
        {
            Console.WriteLine("Disconnect!!");

            if (Game != null)
            {
                var postDisconnect = Game;
                Game.PostMessage(() =>
                {
                    postDisconnect.OnDisconnect();
                });
                Game = null;
            }

        }

        private int onRead(MemoryStream transferred)
        {
            int offset = 0;
            byte[] buffer = transferred.GetBuffer();
            while ((transferred.Length - offset) > sizeof(int))
            {

                int size = BitConverter.ToInt32(buffer, offset);

                if (size < 1 || size > RecvBufferSize)
                {
                    transferred.Seek(transferred.Length, SeekOrigin.Begin);
                    Disconnect();
                    return 0;
                }

                if (size > transferred.Length - offset)
                {
                    break;
                }


                int code = BitConverter.ToInt32(buffer, offset + 4);
                long seq = BitConverter.ToInt64(buffer, offset + 8);

                if (Game == null)
                {
                    Game = Engine.Framework.New<Entities.Game>.Instantiate;
                    Game.Client = this;
                }

                using (MemoryStream msg = new MemoryStream(buffer, offset + 16, size - 16, true, true))
                {

                    var notifier = new Notifier();
                    notifier.Sequence = seq;
                    notifier.Protocol = this;
                    var callback = Schema.Protobuf.Api.Bind(Game,
                                                        notifier,
                                                        code,
                                                        msg);

                    Game.PostMessage(callback);
                }

                offset += size;
            }

            transferred.Seek(offset, SeekOrigin.Begin);
            return 0;

        }

        public static Client Instance { get; } = new Client();

        public void Update()
        {
            if (Game == null) return;

            if ((int)State < (int)DedicateState.Connected) return;

            if (Game.WaitingResponses[State] == true)
            {
                switch (State)
                {
                    case DedicateState.WaitingEnterAllPlayer:
                        if (DateTime.UtcNow >= Game.WaitingAllPlayerTimeout)
                        {
                            /*
                            State = DedicateState.Playing;
                            Game.WaitingResponses[DedicateState.Playing] = true;
                            Game.PlayingTimeout = DateTime.UtcNow.AddSeconds(5);
                            */
                        }
                        break;
                    case DedicateState.Playing:
                        if (DateTime.UtcNow >= Game.PlayingTimeout)
                        {
                            State = DedicateState.Finish;
                            Game.WaitingResponses[DedicateState.Finish] = false;
                        }
                        break;
                }
                return;
            }

            try
            {
                switch (State)
                {
                    case DedicateState.Connected:
                        {
                            DedicateReady dedicateReady = new DedicateReady();
                            Process currentProcess = Process.GetCurrentProcess();
                            dedicateReady.ProcessId = currentProcess.Id;
                            dedicateReady.Port = Api.ListenPortForClient;
                            dedicateReady.BattleMode = Api.BattleMode;

                            State = DedicateState.ReceiveEndMatching;
                            Game.LastSendedPacket = dedicateReady;
                            Game.WaitingResponses[DedicateState.ReceiveEndMatching] = true;
                            Game.LastSendedTime = DateTime.UtcNow;

                            MemoryStream stream = new MemoryStream();
                            SerializeProtobufTo(stream, 0, dedicateReady);
                            base.Write(stream);
                        }
                        break;
                    case DedicateState.ReceiveEndMatching:
                        {
                            State = DedicateState.WaitingEnterAllPlayer;
                            Game.WaitingResponses[DedicateState.WaitingEnterAllPlayer] = true;
                            Game.WaitingAllPlayerTimeout = DateTime.UtcNow.AddMinutes(1);
                        }
                        break;
                    case DedicateState.WaitingEnterAllPlayer:
                        {
                            State = DedicateState.Playing;
                            Game.WaitingResponses[DedicateState.Playing] = true;
                            Game.PlayingTimeout = DateTime.UtcNow.AddMinutes(5);
                        }
                        break;
                    case DedicateState.Finish:
                        {
                            switch (Api.BattleMode)
                            {
                                case Schema.Protobuf.Enums.EBattleMode.Tag:
                                    {
                                        int rank = Game.ReceivedEnterBattle.Count;

                                        foreach (var item in Game.ReceivedEnterBattle)
                                        {
                                            Schema.Protobuf.Message.Action.EndGame endGame = new Schema.Protobuf.Message.Action.EndGame();

                                            endGame.Idx = item.Value.Passport.Idx;
                                            endGame.Mode = Api.BattleMode;
                                            endGame.Rank = rank;
                                            MemoryStream stream = new MemoryStream();
                                            SerializeProtobufTo(stream, 0, endGame);
                                            base.Write(stream);
                                            rank--;

                                            Game.LastSendedPacket = endGame;
                                            Game.LastSendedTime = DateTime.UtcNow;
                                        }
                                    }
                                    break;
                                case Schema.Protobuf.Enums.EBattleMode.FreeForAll:
                                    {
                                        int rank = Game.ReceivedEnterBattle.Count;

                                        foreach (var item in Game.ReceivedEnterBattle)
                                        {
                                            if (rank < 2) continue;

                                            Schema.Protobuf.Message.Action.EndGame endGame = new Schema.Protobuf.Message.Action.EndGame();

                                            endGame.Idx = item.Value.Passport.Idx;
                                            endGame.Mode = Api.BattleMode;
                                            endGame.Rank = rank;
                                            MemoryStream stream = new MemoryStream();
                                            SerializeProtobufTo(stream, 0, endGame);
                                            base.Write(stream);
                                            rank--;

                                            Game.LastSendedPacket = endGame;
                                            Game.LastSendedTime = DateTime.UtcNow;
                                        }

                                    }
                                    break;
                                case Schema.Protobuf.Enums.EBattleMode.RandomTrio:
                                    {
                                        EndMatching endMatching = (EndMatching)Game.LastReceivedPacket[Engine.Framework.Id<Schema.Protobuf.Message.Action.EndMatching>.Value];

                                        var partyIds = endMatching.Passports.GroupBy(r => r.Party.Id).Select(r => new { PartyID = r.Key }).ToList();
                                        int rank = partyIds.Count;

                                        foreach (var item in partyIds)
                                        {
                                            if (rank < 2) continue;

                                            Schema.Protobuf.Message.Action.EndGame endGame = new Schema.Protobuf.Message.Action.EndGame();
                                            endGame.Mode = Schema.Protobuf.Enums.EBattleMode.RandomTrio;
                                            endGame.Idx = item.PartyID;
                                            endGame.Rank = 1;

                                            MemoryStream stream = new MemoryStream();
                                            SerializeProtobufTo(stream, 0, endGame);
                                            base.Write(stream);
                                            rank--;

                                            Game.LastSendedPacket = endGame;
                                            Game.LastSendedTime = DateTime.UtcNow;
                                        }
                                    }
                                    break;
                            }

                            State = DedicateState.Result;
                            Game.WaitingResponses[DedicateState.Result] = true;
                        }
                        break;
                    case DedicateState.Result:
                        {
                            Result result = new Result();

                            result.CreateGame = new CreateGame();
                            result.CreateGame.Timestamp = DateTime.UtcNow.Ticks;

                            result.BeginGame = new BeginGame();
                            result.BeginGame.Timestamp = 0;

                            State = DedicateState.WaitDestroy;
                            Game.LastSendedPacket = result;
                            Game.WaitingResponses[DedicateState.WaitDestroy] = true;
                            Game.LastSendedTime = DateTime.UtcNow;

                            MemoryStream stream2 = new MemoryStream();
                            SerializeProtobufTo(stream2, 0, result);
                            base.Write(stream2);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void ResendLastPacket()
        {
            MemoryStream stream = new MemoryStream();
            SerializeProtobufTo(stream, 0, Game.LastSendedPacket);
            base.Write(stream);
        }

        public Game Game = null;

        public enum DedicateState
        {
            None = 0,
            Connected,
            ReceiveEndMatching,
            WaitingEnterAllPlayer,
            Playing,
            Finish,
            Result,
            WaitDestroy,
        }

        private DedicateState state = DedicateState.None;
        public DedicateState State
        {
            get
            {
                return state;
            }

            set
            {
                Api.WriteDebugLog($"Change State : {state.ToString()} ===> {value.ToString()}");
                state = value;
            }
        }
    }
}
