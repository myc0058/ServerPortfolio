using Engine.Framework;
using dbms = Engine.Database.Management;
using System.Threading.Tasks;
using System;
using Schema.Protobuf.Message.Game;
using Schema.Protobuf.Message.Enums;
using Schema.Protobuf.Message.Common;
using System.Linq;

namespace Application.Game.Entities
{
    public partial class Room
    {
        public void OnMessage(Engine.Network.Protocol.Delegator<Delegatables.Lobby.User>.Notifier notifier, global::Schema.Protobuf.Message.Game.EnterRoom msg)
        {
            try
            {
                var result = new MyGameResult()
                {
                    ResponseCode = ResponseCode.Success,
                    Data = new GameResult()
                    {
                        RoomID = UID,
                        Idx = msg.Idx,
                        Rank = -1,
                    }
                };

                var user = new User()
                {
                    Idx = msg.Idx,
                    Delegator = notifier.Delegator,
                    MyGameResult = result,
                    SendGameResult = false,
                    Room = this,
                };

                Users.Remove(user.Idx);
                Users.Add(user.Idx, user);

                var character = new Character(user);
                character.Idx = msg.Idx;
                character.Position = new System.Numerics.Vector3(10 * (Users.Count-1), 1, 0);
                character.Direction = new System.Numerics.Vector3(0, 0, 0);
                character.Speed = 10.0f;

                Characters.Remove(character.Idx);
                Characters.Add(character.Idx, character);
                

                msg.ResponseCode = Schema.Protobuf.Message.Enums.ResponseCode.Success;
                foreach (var item in Characters)
                {
                    var temp = item.Value.ToPacket();

                    msg.Characters.Add(temp);
                }

                var enterUser = new Schema.Protobuf.Message.Game.EnterCharacter();
                enterUser.RoomId = UID;
                enterUser.Character = character.ToPacket();

                foreach (var item in Users)
                {
                    if (item.Key == msg.Idx)
                    {
                        continue;
                    }

                    item.Value.Delegator?.Delegate(UID, item.Value.Idx, enterUser);
                }

                notifier.Response(msg);

                user.SendRTT();
                user.SendRTT();
                user.SendRTT();
            }
            catch (Exception ex)
            {
                Engine.Framework.Api.Logger.Error(ex.StackTrace);
                Users.Remove(msg.Idx);
                Characters.Remove(msg.Idx);
            }
        }
    }
}
