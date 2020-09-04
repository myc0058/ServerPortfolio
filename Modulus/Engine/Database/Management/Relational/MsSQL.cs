﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;


namespace Engine.Database.Management.Relational
{
    public class MsSql : Driver.ISession
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Pw { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Db { get; set; }

        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }
        public SqlCommand Command { get; set; }
        public SqlBulkCopy BulkCopy { get; set; }

        private string connectionString { get; set; }
        public Driver.ISession Create()
        {
            var session = new MsSql();
            session.connectionString = connectionString;
            return session;
        }
        public void Initialize()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.UserID = Id;
            connectionStringBuilder.Password = Pw;
            connectionStringBuilder.DataSource = $"{Ip},{Port}";
            connectionStringBuilder.InitialCatalog = Db;
            connectionStringBuilder.Pooling = true;
            //connectionString.AllowZeroDateTime = true;
            //connectionString.CurrentLanguage = "utf8";
            //connectionStringBuilder.CurrentLanguage = "utf8";
            connectionStringBuilder.MultipleActiveResultSets = false;

            //connectionStringBuilder.CheckParameters = false;
            //connectionStringBuilder.UseCompression = false;
            connectionStringBuilder.ConnectTimeout = 5;
            connectionStringBuilder.MinPoolSize = (int)Engine.Framework.Api.ThreadCount;
            connectionStringBuilder.MaxPoolSize = (int)(Engine.Framework.Api.ThreadCount * Database.Management.Driver.SessionCount * 2);

            //connectionString.SslMode = MySqlSslMode.None;
            connectionStringBuilder.Encrypt = true;
            connectionStringBuilder.TrustServerCertificate = false;

            connectionString = connectionStringBuilder.ToString();
        }
        public void CopyFrom(Driver.ISession value) { }
        public Driver.ISession Open(bool transaction = true) {

            int max = 10;
            while (true)
            {
                try
                {
                    if (Connection == null)
                    {
                        Connection = new SqlConnection(connectionString);
                        Connection.Open();
                    }

                    if (transaction == true)
                    {
                        BeginTransaction();
                    }
                }
                catch
                {
                    Task.Delay(100).Wait();
                    max -= 1;
                    Close();
                    Dispose();
                    if (max < 0) { throw; }
                    continue;
                }
                break;
            }
            //if (Driver.Sessions.Value.Contains(this) == false)
            //{
            //    Driver.Sessions.Value.Add(this);
            //}
            return this;

        }
        public void BeginTransaction() {

            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();
            }
            if (Command != null)
            {
                Command.Transaction = Transaction;
            }
        }

        public void Commit() {
            Transaction?.Commit();
            Transaction = null;
            if (Command != null)
            {
                Command.Transaction = null;
            }
        }
        public void Rollback() {
            Transaction?.Rollback();
            Transaction = null;
            if (Command != null)
            {
                Command.Transaction = null;
            }
        }
        public void Close() {
            try
            {
                Rollback();
                Connection?.Close();
            }
            catch (Exception ex)
            {
                Engine.Framework.Api.Logger.Info("Driver Level Rollback Exception " + ex);
            }
        }
        public void Dispose()
        {
            Command?.Dispose();
            Command = null;

            Transaction?.Dispose();
            Transaction = null;

            Connection?.Dispose();
            Connection = null;

            BulkCopy = null;
        }


        public SqlCommand CreateCommand(string text, System.Data.CommandType type)
        {
            if (Connection == null)
            {
                Open();
            }

            if (Command == null)
            {
                Command = Connection.CreateCommand();
                Command.Transaction = Transaction;
            }

            Command.CommandText = text;
            Command.CommandType = type;
            Command.Parameters.Clear();

            return Command;
        }

        public SqlCommand CreateCommand()
        {
            if (Command == null)
            {
                Command = Connection.CreateCommand();
                Command.Transaction = Transaction;
            }

            Command.CommandType = System.Data.CommandType.Text;
            Command.CommandText = string.Empty;
            Command.Parameters.Clear();

            return Command;
        }

        public SqlBulkCopy CreateBulkCopy(string tablename)
        {
            if (BulkCopy == null)
            {
                BulkCopy = new SqlBulkCopy(
                    Connection,
                    SqlBulkCopyOptions.Default,
                    Transaction
                    );
            }

            BulkCopy.DestinationTableName = tablename;
            return BulkCopy;
        }
    }
}
