using Schema.Protobuf;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Schema.Protobuf.Room;

namespace Unity
{
    public class Room : MonoBehaviour
    {

        public class Interporation
        {
            public UnityEngine.Vector3 orgPosition { get; set; } = Vector3.zero;
            public UnityEngine.Vector3 targetPosition { get; set; } = Vector3.zero;
            public long deltaTicks = 0;
            public long nowSeq = 0;
        }

        public GameObject MapPrefab;
        public GameObject CharacterPrefab;

        public Dictionary<long, GameObject> characters = new Dictionary<long, GameObject>();
        
        public long OwnerIdx { get; set; }

        void Awake()
        {
            var map = Instantiate(MapPrefab, this.transform);
            map.transform.localPosition = Vector3.zero;
        }

        public void AddCharacter(User user, Character character)
        {
            var characterObj = Instantiate(CharacterPrefab, this.transform);
            
            characterObj.name = "Char_" + character.Idx;

            var capsule = characterObj.transform.Find("Capsule");
            capsule.localPosition = character.Position;
            if (OwnerIdx == character.Idx)
            {
                capsule.gameObject.AddComponent<MoveCharacter>();
                var moveCharacter = capsule.GetComponent<MoveCharacter>();
                moveCharacter.Speed = character.Speed;
                moveCharacter.Idx = character.Idx;
                moveCharacter.User = user;
                moveCharacter.Room = this;
                capsule.GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                capsule.gameObject.AddComponent<ControlCharacter>();
                var controlCharacter = capsule.GetComponent<ControlCharacter>();
                controlCharacter.Room = this;
                capsule.GetComponent<Renderer>().material.color = Color.red;
            }

            characters.Add(character.Idx, characterObj);
        }

        private List<Schema.Protobuf.Message.Game.World> worldList = new List<Schema.Protobuf.Message.Game.World>();

        public void RecvWorld(Schema.Protobuf.Message.Game.World msg)
        {
            worldList.Add(msg);

            if (worldList.Count < 3)
            {
                return;
            }

            var nowWorld = worldList[0];
            var nextWorld = worldList[1];

            foreach(var item in characters)
            {
                var capsule = item.Value.transform.Find("Capsule");

                var nowCharacter = nowWorld.Characters.Where(x => x.Idx == item.Key).FirstOrDefault();
                var nextCharacter = nextWorld.Characters.Where(x => x.Idx == item.Key).FirstOrDefault();
                Interporation interporation = new Interporation()
                {
                    orgPosition = nowCharacter.Position.ToUnityVector3(),
                    targetPosition = nextCharacter.Position.ToUnityVector3(),
                    deltaTicks = nextWorld.DeltaTicks,
                    nowSeq = nowWorld.Seq,
                };

                if (item.Key == OwnerIdx)
                {
                    var moveCharacter = capsule.GetComponent<MoveCharacter>();
                    moveCharacter.Move(interporation);
                }
                else
                {
                    var controlCharacter = capsule.GetComponent<ControlCharacter>();
                    controlCharacter.Move(interporation);
                }
            }

            worldList.RemoveAt(0);
        }

        public static Room GetRoomComponent(string name)
        {
            return GameObject.Find("Room_" + name).GetComponent<Room>();
        }
    }
}