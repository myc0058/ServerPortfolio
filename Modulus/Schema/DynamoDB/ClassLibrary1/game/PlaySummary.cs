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
    [DynamoDBTable("game.PlaySummary")]
    public class PlaySummary
    {
        public long Idx { get; set; }
        public string SecondKey { get; set; }
        public int Season { get; set; }
        public int Mode { get; set; }
        public int ChracterID { get; set; }
        public int PlayTime { get; set; }
        public int PlayCount { get; set; }
        public int WinCount { get; set; }
        public int LevelCount { get; set; }
        public int KillCount { get; set; }
        public int BestKillCount { get; set; }
        public int DamageToUser { get; set; }
        public int AssistCount { get; set; }
        public int RSKillKillCount { get; set; }
        public int ResurrectOtherCount { get; set; }
        public int RallyOtherCount { get; set; }
        public int KillBossCount { get; set; }
        public int KillChiefCount { get; set; }
        public int KillMonsterCount { get; set; }
        public int KillGuardCount { get; set; }
        public int GetLegendaryEquipmentCount { get; set; }
        [DynamoDBVersion]
        public int? VersionNumber { get; set; }

        public static async Task<List<PlaySummary>> GetPlaySummaries(AmazonDynamoDBClient client, DynamoDBContext context, long idx)
        {
            var playSummarys = new List<PlaySummary>();
            
            {
                var request = new QueryRequest
                {
                    TableName = "game.PlaySummary",
                    KeyConditionExpression = "Idx = :V1",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                        {
                            { ":V1", new AttributeValue { N = $"{idx}" }}
                        },
                    ConsistentRead = false
                };

                var response = await client.QueryAsync(request);

                Document doc = null;
                foreach (Dictionary<string, AttributeValue> item in response.Items)
                {
                    //레벨카운트가 int를 넘어서는 데이타가 있을때 방어코드
                    if (item.ContainsKey("LevelCount") == true)
                    {
                        if (long.TryParse(item["LevelCount"].N, out var oldLevel) == true)
                        {
                            if (oldLevel > int.MaxValue)
                            {
                                item["LevelCount"] = new AttributeValue { N = $"{int.MaxValue}" };
                            }
                        }

                    }

                    // Process the result.
                    doc = Document.FromAttributeMap(item);
                    playSummarys.Add(context.FromDocument<Schema.DynamoDB.game.PlaySummary>(doc));
                }
            }

            return playSummarys;
        }
    }
}
