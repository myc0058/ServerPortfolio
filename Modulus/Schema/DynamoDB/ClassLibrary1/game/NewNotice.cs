using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.NewNotice")]
    public class NewNotice
    {
        public string Region { get; set; }
        public string Message { get; set; }
    }
}