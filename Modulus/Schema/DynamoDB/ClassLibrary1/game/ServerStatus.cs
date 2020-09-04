using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.ServerStatus")]
    public class ServerStatus
    {
        public string Region { get; set; }
        public bool Avaliable { get; set; }
    }
}