using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Data;
using System.Collections;

namespace Engine.Database.Management.Relational
{


	public sealed class MySql : Driver.ISession
	{
		//  public class Session : Driver.Session {
		public string Name { get; set; }
		public string Id { get; set; }
		public string Pw { get; set; }
		public string Ip { get; set; }
		public string Port { get; set; }
		public string Db { get; set; }
		public MySqlConnection Connection { get; set; }
        public MySqlTransaction Transaction { get; set; }
        public MySqlCommand Command { get; set; }
		private string connectionStringValue;

        public Driver.ISession Create()
        {
            var session = new MySql();
            session.connectionStringValue = connectionStringValue;
            return session;
        }

        public MySqlCommand CreateCommand(string text, CommandType type)
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

        public MySqlCommand CreateCommand()
        {
            if (Command == null)
            {
                Command = Connection.CreateCommand();
                Command.Transaction = Transaction;
            }

            Command.CommandType = CommandType.Text;
            Command.CommandText = "";
            Command.Parameters.Clear();
          
            return Command;
        }

		public void BeginTransaction()
		{
            if (Transaction == null)
            {
                Transaction = Connection.BeginTransaction();

            }

            if (Command != null)
            {
                Command.Transaction = Transaction;
            }
		}

		public void Commit()
		{
            Transaction?.Commit();
            Transaction = null;
            if (Command != null)
            {
                Command.Transaction = null;
            }
        }

		public void Rollback()
		{
            Transaction?.Rollback();
            Transaction = null;
            if (Command != null)
            {
                Command.Transaction = null;
            }
        }

		public void Initialize()
		{
			MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder();

			connectionString.UserID = Id;
			connectionString.Password = Pw;
			connectionString.Server = Ip;
			connectionString.Port = Convert.ToUInt32(Port);
			connectionString.Database = Db;
			connectionString.Pooling = true;
			connectionString.AllowZeroDateTime = true;
			connectionString.CharacterSet = "utf8";
			connectionString.CheckParameters = false;
			connectionString.UseCompression = false;
			connectionString.ConnectionTimeout = 5;
            connectionString.MinimumPoolSize = (uint)Engine.Framework.Api.ThreadCount;
            connectionString.MaximumPoolSize = (uint)(Engine.Framework.Api.ThreadCount * Database.Management.Driver.SessionCount * 2);
            connectionString.SslMode = MySqlSslMode.None;

            connectionStringValue = connectionString.GetConnectionString(true);


		}

        public void Dispose()
        {

            Command?.Dispose();
            Command= null;

            Transaction?.Dispose();
            Transaction = null;

            Connection?.Dispose();
            Connection = null;


        }
		public Driver.ISession Open(bool transaction = true)
		{
            int max = 10;
            while (true)
            {
                try
                {
                    if (Connection == null)
                    {
                        Connection = new MySqlConnection(connectionStringValue);
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
            /*if (Driver.Sessions.Value.Contains(this) == false)
            {
                Driver.Sessions.Value.Add(this);
            }*/
            return this;
        }
        public void Close()
        {
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

        public void CopyFrom(Driver.ISession value)
		{

			var rhs = value as MySql;
			if (rhs == null) { return; }

			Name = rhs.Name;
			Id = rhs.Id;
			Pw = rhs.Pw;
			Ip = rhs.Ip;
			Port = rhs.Port;
			Db = rhs.Db;

		}

	}
}
