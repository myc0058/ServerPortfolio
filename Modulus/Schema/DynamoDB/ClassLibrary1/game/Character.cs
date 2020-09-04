using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Character")]
    public class Character
    {
        public long Idx { get; set; }
        public int Id { get; set; }
        public int Skin { get; set; }
        public List<Schema.Protobuf.Data.Emote> Emotes { get; set; } = new List<Protobuf.Data.Emote>();
        public int Enabled { get; set; }
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
