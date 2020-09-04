using Schema.Protobuf.Message.Authentication;
using UnityEngine;

namespace Schema.Protobuf
{
    public partial class User : Handler
    {
        public Client Client { get; set; } = null;
    }
}
