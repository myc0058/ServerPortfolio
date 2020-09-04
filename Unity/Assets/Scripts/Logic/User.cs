using System;
using System.Collections.Generic;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User
    {
        public long Idx { get; set; } = 0;
        public string ID { get; set; } = string.Empty;

        public Room Room { get; set; } = new Room();

        public User()
        {
        }
    }
}
