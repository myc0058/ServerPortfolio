using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.RepresentCharacter")]
    public class RepresentCharacter
    {
        public long Idx { get; set; }
        public List<Schema.Protobuf.Data.Character> RepresentTrees { get; set; } = new List<Protobuf.Data.Character>();
    }
}
