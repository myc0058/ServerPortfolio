using Amazon;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Engine.Database.Management.Amazon
{
    public class DynamoDB : Driver.ISession
    {
        public ThreadLocal<AmazonDynamoDBClient> Connection = new ThreadLocal<AmazonDynamoDBClient>();

        public void Initialize()
        {

            if (Connection.Value == null)
            {
                Connection.Value = new AmazonDynamoDBClient("AKIAU5ZOL5U7TIPKDK62", "AmSiJigLyn+yImGfBzu3vpmOijc3SuhP60L5Iblh");
            }

        }

        public Driver.ISession Create()
        {
            return this;
        }

        public AmazonDynamoDBClient GetClient()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.APNortheast2;

            if (Connection.Value == null)
            {
                Connection.Value = new AmazonDynamoDBClient("AKIAU5ZOL5U7TIPKDK62", "AmSiJigLyn+yImGfBzu3vpmOijc3SuhP60L5Iblh", clientConfig);
            }

            return Connection.Value;
        }

        public void BeginTransaction() { }
        public void Commit() { }
        public void Rollback() { }
        public Driver.ISession Open(bool transaction = true)
        {
            return null;
        }
        public void Close() { }
        public void CopyFrom(Driver.ISession value) { }

        public void Dispose()
        {

        }
    }
}
