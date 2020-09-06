using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Engine.Framework.Api;

namespace Engine.Database
{
    
    static public class Api
    {

        public static DateTime ToDateTime(this object value)
        {
            try
            {
                if (value is DateTime)
                {
                    return (DateTime)value;
                }

                if (value is global::MySql.Data.Types.MySqlDateTime)
                {
                    return ((global::MySql.Data.Types.MySqlDateTime)value).GetDateTime();
                }

                if (value is string)
                {
                    return DateTime.Parse((string)value);
                }

                if (value is long)
                {
                    return new DateTime((long)value);
                }
            }
            catch
            {
                return new DateTime(0);
            }
            return new DateTime(0);
        }

        public static ResultSet ToResultSet(this MySql.Data.MySqlClient.MySqlDataReader reader, bool leaveOpen = false)
        {
            ResultSet resultRet = new ResultSet();
            do
            {
                global::Engine.Database.Api.ResultSet.Cursor cursor = null;
                if (reader.FieldCount > 0)
                {
                    cursor = resultRet.AddCursor();
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

            if (leaveOpen == false)
            {
                reader.Close();
                reader.Dispose();
            }

            return resultRet;

        }
        public static ResultSet ToResultSet(this System.Data.SqlClient.SqlDataReader reader, bool leaveOpen = false)
        {
            ResultSet resultRet = new ResultSet();
            do
            {
                global::Engine.Database.Api.ResultSet.Cursor cursor = null;
                if (reader.FieldCount > 0)
                {
                    cursor = resultRet.AddCursor();
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

            if (leaveOpen == false)
            {
                reader.Close();
                reader.Dispose();
            }

            return resultRet;

        }

        public static ResultSet ToResultSet(this System.Data.Common.DbDataReader reader, bool leaveOpen = false)
        {
            ResultSet resultRet = new ResultSet();
            do
            {
                global::Engine.Database.Api.ResultSet.Cursor cursor = null;
                if (reader.FieldCount > 0)
                {
                    cursor = resultRet.AddCursor();
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

            if (leaveOpen == false)
            {
                reader.Close();
                reader.Dispose();
            }

            return resultRet;

        }

        public static string DesDecrypt(string value, string key)
        {
            //키 유효성 검사
            byte[] btKey = ASCIIEncoding.ASCII.GetBytes(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            des.Key = btKey;
            des.IV = btKey;

            ICryptoTransform desdecrypt = des.CreateDecryptor();

            byte[] buffer = Convert.FromBase64String(value);
            using (MemoryStream msDecrypt = new MemoryStream(buffer))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, desdecrypt, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        string plaintext = srDecrypt.ReadToEnd();
                        return plaintext;
                    }
                }
            }

        }//end of func DesDecrypt

        public static void StartUp(XmlElement root)
        {
            if (IsOpen == true) { return; }
            Engine.Database.Management.Driver.Instance = new Engine.Database.Management.Driver();
            if (root["MySql"] != null)
            {
                var MySql = root["MySql"].ChildNodes;


                foreach (XmlNode element in MySql)
                {
                    var driver = new Engine.Database.Management.Relational.MySql();
                    driver.Ip = element.Attributes["Ip"].Value;
                    driver.Port = element.Attributes["Port"].Value;
                    driver.Id = element.Attributes["Id"].Value;
                    driver.Pw = element.Attributes["Pw"].Value;
                    driver.Db = element.Attributes["Db"].Value;
                    driver.Name = element.Attributes["Name"].Value;

                    if (element.Attributes["Cript"] != null)
                    {
                        if (Convert.ToBoolean(element.Attributes["Cript"].Value) == true)
                        {
                            driver.Id = DesDecrypt(driver.Id, "showyour");
                            driver.Pw = DesDecrypt(driver.Pw, "showyour");
                        }
                    }

                    Engine.Database.Management.Driver.AddSession(driver.Name, driver);
                }

            }

            if (root["MsSql"] != null)
            {
                var MySql = root["MsSql"].ChildNodes;


                foreach (XmlNode element in MySql)
                {
                    var driver = new Engine.Database.Management.Relational.MsSql();
                    driver.Ip = element.Attributes["Ip"].Value;
                    driver.Port = element.Attributes["Port"].Value;
                    driver.Id = element.Attributes["Id"].Value;
                    driver.Pw = element.Attributes["Pw"].Value;
                    driver.Db = element.Attributes["Db"].Value;
                    driver.Name = element.Attributes["Name"].Value;

                    if (element.Attributes["Cript"] != null)
                    {
                        if (Convert.ToBoolean(element.Attributes["Cript"].Value) == true)
                        {
                            driver.Id = DesDecrypt(driver.Id, "showyour");
                            driver.Pw = DesDecrypt(driver.Pw, "showyour");
                        }
                    }

                    Engine.Database.Management.Driver.AddSession(driver.Name, driver);
                }

            }


            if (root["Redis"] != null)
            {
                var Redis = root["Redis"].ChildNodes;

                foreach (XmlNode element in Redis)
                {
                    var driver = new Engine.Database.Management.NoSql.Redis();

                    driver.SetMaster(element["Master"].Attributes["Ip"].Value, element["Master"].Attributes["Port"].Value);

                    if (element["Slaves"] != null)
                    {
                        foreach (XmlNode s in element["Slaves"].ChildNodes)
                        {
                            driver.SetMaster(s.Attributes["Ip"].Value, s.Attributes["Port"].Value);
                        }
                    }


                    driver.Id = element.Attributes["Id"].Value;
                    driver.Pw = element.Attributes["Pw"].Value;
                    driver.Db = element.Attributes["Db"].Value;
                    driver.Name = element.Attributes["Name"].Value;

                    if (element.Attributes["Cript"] != null)
                    {
                        if (Convert.ToBoolean(element.Attributes["Cript"].Value) == true)
                        {

                            driver.Id = DesDecrypt(driver.Id, "showyour");
                            driver.Pw = DesDecrypt(driver.Pw, "showyour");
                        }
                    }

                    Engine.Database.Management.Driver.AddSession(driver.Name, driver);
                }
            }

            Engine.Database.Management.Driver.Instance.Run();

            Framework.Api.Add(Singleton<Layer>.Instance);
            Framework.Api.Add(Singleton<RedisLayer>.Instance);
            isOpen = true;

        }

        static private bool isOpen = false;
        static public bool IsOpen
        {
            get { return isOpen; }
        }


        internal class Layer : Framework.Layer
        {
          
        }

        internal class RedisLayer : Framework.Layer
        {
            
        }

        static public void StartUp()
        {
            if (IsOpen == true) { return; }
            Engine.Framework.Api.Logger.Info("Engine.Database.Api.StartUp");

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

			try
			{
                Engine.Framework.Api.Config.Seek(0, SeekOrigin.Begin);
                using (XmlReader reader = XmlReader.Create(Engine.Framework.Api.Config, readerSettings))
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(reader);
					var root = doc["root"];
                    StartUp(root);
				}
			}
            catch (Exception e)
			{
                Engine.Framework.Api.Logger.Info(e);
			}

            isOpen = true;
        }
        static public void CleanUp()
        {
            isOpen = false;
            if (Management.Driver.Instance != null)
            {
                Management.Driver.Instance.Close();
            }
        }
    

        public class ResultSet : IEnumerable<ResultSet.Cursor>
        {


            public bool HasCursors
            {
                get
                {
                    if (cursors.Count == 0) { return false; }
                    return true;
                }
            }

            public Cursor this[int index]
            {
                get
                {
                    if (cursors.Count == 0) { return Singleton<Cursor>.Instance; }
                    if (index >= cursors.Count) { return Singleton<Cursor>.Instance; }
                    return cursors[index] as Cursor;
                }
            }

            public void Merge(ResultSet rhs)
            {
                foreach (var c in rhs)
                {
                    cursors.Add(c);
                }
            }

            public Cursor AddCursor()
            {
                Cursor cursor = new Cursor();
                cursors.Add(cursor);
                return cursor;
            }

            public IEnumerator<ResultSet.Cursor> GetEnumerator()
            {
                return (IEnumerator<ResultSet.Cursor>)cursors.GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return cursors.GetEnumerator();
            }

            private List<Cursor> cursors = new List<Cursor>();

            public class Cursor : IEnumerable<Row>
            {
                private List<Row> Rows = new List<Row>();
                public int Count { get { return Rows.Count; } }
                public Row this[int index]
                {
                    get
                    {
                        if (Rows.Count <= index) { return Singleton<Row>.Instance; }
                        return Rows[index] as Row;
                    }
                }

                public Row AddRow()
                {
                    Row row = new Row();
                    Rows.Add(row);
                    return row;
                }

                public IEnumerator<Row> GetEnumerator()
                {
                    return (IEnumerator<Row>)Rows.GetEnumerator();

                }
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return Rows.GetEnumerator();
                }
            }
            public class Row : IEnumerable
            {
                private ArrayList Columns = new ArrayList();
                public int Count { get { return Columns.Count; } }
                public object this[int index]
                {
                    get
                    {
                        if (Columns.Count <= index) { return null; }
                        return Columns[index];
                    }
                }

                public void AddColumn(object value)
                {
                    Columns.Add(value);
                }
                public IEnumerator GetEnumerator()
                {
                    return Columns.GetEnumerator();
                }

            }
        }


    }
}
