using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Wallet")]
    public class Wallet
    {
        public long Idx { get; set; }
        public List<Schema.Protobuf.Data.Currency> Currencies { get; set; } = new List<Protobuf.Data.Currency>();
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }
}
