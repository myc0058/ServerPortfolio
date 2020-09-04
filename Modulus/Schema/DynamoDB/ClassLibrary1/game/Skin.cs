using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Skin")]
    public class Skin
    {
        public long Idx { get; set; }
        public List<int> Elements { get; set; } = new List<int>();
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
