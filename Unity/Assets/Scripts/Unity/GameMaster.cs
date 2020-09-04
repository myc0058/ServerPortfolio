using Schema.Protobuf;
using System.Collections.Generic;
using UnityEngine;

namespace Unity
{
    public class GameMaster : MonoBehaviour
    {
        public GameObject RoomPrefab;

        public Dictionary<long, GameObject> Rooms = new Dictionary<long, GameObject>();

        void Awake()
        {
        }

        void Start()
        {
        }

        void Update()
        {
        }

        void OnDestroy()
        {
        }

        public GameObject MakeRoom(long idx)
        {
            var room = Instantiate(RoomPrefab, this.transform);
            room.transform.localPosition = new Vector3(Rooms.Count * 150, 0, 0);
            room.name = "Room_" + idx;
            var roomComp = room.GetComponent<Room>();
            roomComp.OwnerIdx = idx;
            Rooms.Add(idx, room);
            return room;
        }

        public static GameMaster GetGameMasterComponent()
        {
            return GameObject.Find("GameMaster").GetComponent<GameMaster>();
        }
    }
}

