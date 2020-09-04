using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Mastery")]
    public class Mastery
    {
        public class Tree
        {
            public string Name;
            public int Point;
            public int MaxPoint;
            public int Preset;
            public List<Schema.Protobuf.Data.Mastery> Masteries { get; set; } = new List<Protobuf.Data.Mastery>();
        }

        public long Idx { get; set; }
        public List<Tree> MasteryTrees { get; set; }
        public int Selected { get; set; }
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
