using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Purchase")]
    public class Purchase
    {
        public class Item
        {
            public Schema.Protobuf.Enums.EItemDataType Type { get; set; }
            public int Id { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public long Idx { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
