using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schema.DynamoDB.game
{
    [DynamoDBTable("game.PlayResult")]
    public class PlayResult
    {
        public long Idx { get; set; } = -1;
        public int Season { get; set; } = -1;
        public long Room { get; set; } = -1;
        public int CharacterID1 { get; set; } = -1;
        public int CharacterID2 { get; set; } = -1;
        public int Mode { get; set; } = -1;
        public int Rank { get; set; } = -1;
        public int PlayerCount { get; set; } = -1;
        public int Level { get; set; } = -1;
        public int KillCount { get; set; } = 0;
        public int WeaponID { get; set; } = -1;
        public int HelmetID { get; set; } = -1;
        public int ArmorID { get; set; } = -1;
        public int AccessoryID1 { get; set; } = -1;
        public int AccessoryID2 { get; set; } = -1;
        public string WinLose { get; set; } = "";
        public int PlayTime { get; set; } = -1;
        public long EndTime { get; set; } = -1;

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }

        public static async Task<List<PlayResult>> GetPlayResults(AmazonDynamoDBClient client, DynamoDBContext context, long idx, int limit, DateTime? startDate, DateTime? endDate)
        {
            var playResults = new List<Schema.DynamoDB.game.PlayResult>();
            {
                var request = new QueryRequest
                {
                    TableName = "game.PlayResult",
                    IndexName = "Idx-EndTime-index",
                    KeyConditionExpression = "Idx = :V1",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            { ":V1", new AttributeValue { N = $"{idx}" }},
                        },
                    ConsistentRead = false,
                    ScanIndexForward = false,
                    Limit = limit
                };

                if (startDate != null && endDate != null)
                {
                    request.KeyConditionExpression += $" AND EndTime BETWEEN :V2 AND :V3";
                    request.ExpressionAttributeValues.Add(":V2", new AttributeValue { N = $"{startDate.Value.Ticks}" });
                    request.ExpressionAttributeValues.Add(":V3", new AttributeValue { N = $"{endDate.Value.Ticks}" });
                }

                var response = await client.QueryAsync(request);

                Document doc = null;
                foreach (Dictionary<string, AttributeValue> item in response.Items)
                {
                    // Process the result.
                    doc = Document.FromAttributeMap(item);
                    playResults.Add(context.FromDocument<Schema.DynamoDB.game.PlayResult>(doc));
                }
            }

            return playResults;
        }
    }
}
