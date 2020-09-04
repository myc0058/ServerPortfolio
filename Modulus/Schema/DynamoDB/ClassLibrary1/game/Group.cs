using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Group")]
    public class Group
    {
        public long Idx { get; set; }
        public string Id { get; set; }
        public long GroupId { get; set; }
        public long Lobby { get; set; }
        public string Name { get; set; }
        public DateTime JoinTime { get; set; }
    }
}
