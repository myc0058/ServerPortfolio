// Magi Caspar Engine.
// Auto Generated By MySqlProcedure Generater
// Do Not Edit
using global::System;
namespace Schema.Query.MySql.StoredProcedure.Game {
public class Game_Shop_ProductBeginSale : global::Engine.Database.Management.Driver.Query {
	public class In {
		public int i_idx;
		public int i_shop_id;
		public int i_product_id;
		public DateTime i_end_time;
	}
	public In IN { get; set; }
	public class Out {
		public int o_error;
	}
	public Out OUT { get; set; }
	public override string Host { get { return "Game"; } }
	public Game_Shop_ProductBeginSale() {
		IN = new In();
		OUT = new Out();
	}
	public override void Execute() {
		var session = global::Engine.Database.Management.Driver.GetSession(Host) as global::Engine.Database.Management.Relational.MySql;
		session.Open();
		var command = (session as global::Engine.Database.Management.Relational.MySql).CreateCommand("Game_Shop_ProductBeginSale", System.Data.CommandType.StoredProcedure);

		command.Parameters.AddWithValue("i_idx", IN.i_idx).Direction = global::System.Data.ParameterDirection.Input;
		command.Parameters.AddWithValue("i_shop_id", IN.i_shop_id).Direction = global::System.Data.ParameterDirection.Input;
		command.Parameters.AddWithValue("i_product_id", IN.i_product_id).Direction = global::System.Data.ParameterDirection.Input;
		command.Parameters.AddWithValue("i_end_time", IN.i_end_time).Direction = global::System.Data.ParameterDirection.Input;
		command.Parameters.Add("o_error", global::MySql.Data.MySqlClient.MySqlDbType.Int32).Direction = global::System.Data.ParameterDirection.Output;
		try
		{
			using(var reader = command.ExecuteReader())
			{
				RecordsAffected = reader.RecordsAffected;
				do
				{
					global::Engine.Database.Api.ResultSet.Cursor cursor = null;
					if (reader.FieldCount > 0)
					{
						if (ResultSet == null)
						{
							ResultSet = new global::Engine.Database.Api.ResultSet();
						}
						cursor = ResultSet.AddCursor();
					}
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							var row = cursor.AddRow();
							for (int i = 0; i < reader.FieldCount; ++i)
							{
								row.AddColumn(reader.GetValue(i));
							}
						}
					}
				}
				while (reader.NextResult());
			}
		}
		catch { throw; }
		finally { if (ResultSet == null) { ResultSet = global::Engine.Framework.Api.Singleton<global::Engine.Database.Api.ResultSet>.Instance; } }

		OUT.o_error = (int)command.Parameters["o_error"].Value;
	}
	public override global::System.Collections.Generic.IEnumerable<string> GetHost() {
		yield return Host;
		yield break;
	}
}
}