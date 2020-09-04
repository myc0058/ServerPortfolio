using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.Account")]
    public class Account
    {
        public string Id { get; set; }
        public long Idx { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string LastAccessCountryCode { get; set; }
        public DateTime LastPlayTime { get; set; }
        
        public string CountryCode { get; set; } = "-";
        public long Lobby { get; set; }
        public int Exp { get; set; }
        public int Level { get; set; }
        public int IsGM { get; set; } = 0;
        public string LastAccessIP { get; set; }

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
        
    }
}