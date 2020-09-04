using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.DLC")]
    public class DLC
    {
        public long Idx { get; set; }
        public List<int> Apps { get; set; }
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
