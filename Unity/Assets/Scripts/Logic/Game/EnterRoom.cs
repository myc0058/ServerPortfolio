using Schema.Protobuf.Message.Game;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public override void OnMessage(INotifier notifier, EnterRoom msg)
        {
            base.OnMessage(notifier, msg);

            Unity.GameMaster.GetGameMasterComponent().MakeRoom(Idx);
            var roomComp = Unity.Room.GetRoomComponent(Idx.ToString());
            var dropdownEvent = DropdownEvent.GetDropdownComponent();

            Room.GameServerID = msg.GameServerId;
            Room.RoomId = msg.RoomId;
            Room.UIRoomComp = roomComp;

            foreach (var item in msg.Characters)
            {
                var character = new Room.Character(item);
                Room.Characters.Add(character.Idx, character);
                roomComp.AddCharacter(this, character);
                dropdownEvent.AddCharacter(character.Idx);
            }

            Debug.Log($"Receive {msg.GetType()} msg => " + msg.ToString());
        }
    }
}

