using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Framework
{
	public class Repository
	{
        [Serializable]
		public class Metadata
		{
            
			public class PrimaryKey : Attribute { }
            public class SecondaryKey : Attribute { }
			virtual public void CustomLoad(XmlAttributeCollection attributes) { }
            virtual public object CustomLoad(StreamReader reader){ return null; }

			public class Loader : Attribute
			{
				public virtual void Load(object obj, FieldInfo field, XmlElement element) { }
			}
			public class List : Loader
			{
				public override void Load(object obj, FieldInfo field, XmlElement element)
				{
					
				}
			}
		}

		static private Dictionary<global::System.Type, Repository> tables = new Dictionary<global::System.Type, Repository>();
		private Dictionary<object, Repository.Metadata> metadatas = new Dictionary<object, Repository.Metadata>();
		private ArrayList metadatasToArray = new ArrayList();
		static public T GetMetadata<T>(object key) where T : Repository.Metadata {

			Repository table = null;
			if (tables.TryGetValue(typeof(T), out table) == false) {
				return null;
			}

			Repository.Metadata metadata;
			if (table.metadatas.TryGetValue(key, out metadata) == false) {
				return null;
			}
			return metadata as T;

		}

        

        static public Repository GetTable<T>()
        {

            Repository table = null;
            if (tables.TryGetValue(typeof(T), out table) == false)
            {
                table = new Repository();
                tables.Add(typeof(T), table);
                return table;
            }

            return table;

        }

        static public ArrayList GetMetadatas<T>() where T : Repository.Metadata {

			Repository table = null;
			if (tables.TryGetValue(typeof(T), out table) == false) {
				return null;
			}
			return table.metadatasToArray;
		}

		static public T RandomMetadata<T>() where T : Repository.Metadata
		{
			var metadatas = Repository.GetMetadatas<T>();

			if (metadatas == null || metadatas.Count == 0) { return null; }
			int max = metadatas.Count;
			var metadata = (metadatas[Engine.Framework.Dice.Throw(0, max)] as T);
			return metadata;

		}

		//static public void Load<T>(byte[] buffer) where T : Repository.Metadata, new() {
		//	XmlDocument doc = new XmlDocument();
		//	doc.Load(new MemoryStream(buffer));
		//	Load<T>(doc);
			
		//}

		private static void LoadXml<T>(XmlDocument doc) where T : Repository.Metadata, new() {

			Repository table;
			if (tables.TryGetValue(typeof(T), out table) == false) {
				table = new Repository();
				tables.Add(typeof(T), table);
			}

			table.Clear();

			XmlNode root = doc.DocumentElement;
			if (root == null) { return; }
			foreach (var e in root.ChildNodes) {

                if (e is System.Xml.XmlComment) { continue; }

                XmlElement node = e as XmlElement;
                var metadata = new T();

				var fields = metadata.GetType().GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);

                object primaryKey = null;
                object secondaryKey = null;
				foreach (var field in fields) {

					var attribute = node.Attributes[field.Name];
					if (attribute == null) { continue; }

					try {
						if (field.IsInitOnly == false) { continue; }
                        if (field.FieldType.IsEnum == true) {

                            try
                            {
                                var et = Enum.Parse(field.FieldType, attribute.Value);
                                field.SetValue(metadata, et);
                            }
                            catch (Exception)
                            {
                                field.SetValue(metadata, 0);
                            }
                        } else if (field.FieldType.IsPrimitive == true) {
                            var convertedType = Convert.ChangeType(attribute.Value, field.FieldType);
                            field.SetValue(metadata, convertedType);
                        } else if(field.FieldType == typeof(DateTime))
                        {
                            field.SetValue(metadata, DateTime.Parse(attribute.Value));

						} else {

							var list = field.GetCustomAttribute(typeof(Repository.Metadata.List));
							if (list != null)
							{
								(list as Repository.Metadata.List).Load(metadata, field, node);
							}
							else
							{
								try
								{
									field.SetValue(metadata, attribute.Value);
								}
                                catch (Exception)
                                {
                                    field.SetValue(metadata, null);
                                }
                            }

							
						}

                        var primary = field.GetCustomAttributes(typeof(Repository.Metadata.PrimaryKey), false);
                        var secondary = field.GetCustomAttributes(typeof(Repository.Metadata.SecondaryKey), false);

						if (primary != null && primary.Length > 0) {
                            primaryKey = field.GetValue(metadata);
						}
                        if (secondary != null && secondary.Length > 0)
                        {
                            secondaryKey = field.GetValue(metadata);
                        }
					} catch (Exception) {
						//Debug.LogError(e + " " + field.Name);
					}

				}

				try {
					metadata.CustomLoad(node.Attributes);
				} catch { }

				//Debug.Log("Add Table " + table.GetType() + " Key : " + metadata.GetKey());
				table.metadatasToArray.Add(metadata);

                object key = null;
                if (primaryKey != null && secondaryKey != null)
                {
                    key = (primaryKey, secondaryKey);
                }
                else if (primaryKey != null)
                {
                    key = primaryKey;
                }
                else if (secondaryKey != null)
                {
                    key = secondaryKey;
                }
                else
                {
                    Engine.Framework.Api.Logger.Info($"Not found {nameof(T)}'s Key");
                    break;
                }
				if (table.metadatas.ContainsKey(key) == false) {
					table.metadatas.Add(key, metadata);
				}

			}
		}

        public void Add(object key, Repository.Metadata metadata)
        {
            metadatasToArray.Add(metadata);

            if (metadatas.ContainsKey(key) == false)
            {
                metadatas.Add(key, metadata);
            }
        }

        public void Clear() {
			metadatas.Clear();
			metadatasToArray.Clear();
		}

		static public void LoadXml<T>(string path) where T : Repository.Metadata, new() {
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			LoadXml<T>(doc);
		}
        static public void LoadJson<T>(string path) where T : Repository.Metadata, new()
        {
            try
            {
                using (var stream = File.OpenText(path))
                {
                    LoadJson<T>(stream);
                    return;
                }
            }
            catch (Exception ex)
            {
                Engine.Framework.Api.Logger.Info(ex.Message);
            }
        }

        static public void LoadJson<T>(StreamReader reader) where T : Repository.Metadata, new()
        {

            Repository table;
            if (tables.TryGetValue(typeof(T), out table) == false)
            {
                table = new Repository();
                tables.Add(typeof(T), table);
            }

            table.Clear();

            T loader = new T();

            T[] array = null;
            array = loader.CustomLoad(reader) as T[];

            if (array == null) { return; }


            foreach (T metadata in array)
            {
                var fields = metadata.GetType().GetFields();

                object primaryKey = null;
                object secondaryKey = null;
                foreach (var field in fields)
                {
                    var primary = field.GetCustomAttributes(typeof(Repository.Metadata.PrimaryKey), false);
                    var secondary = field.GetCustomAttributes(typeof(Repository.Metadata.SecondaryKey), false);

                    if (primary != null && primary.Length > 0)
                    {
                        primaryKey = field.GetValue(metadata);
                    }
                    if (secondary != null && secondary.Length > 0)
                    {
                        secondaryKey = field.GetValue(metadata);
                    }
                }

                table.metadatasToArray.Add(metadata);
                object key = null;
                if (primaryKey != null && secondaryKey != null)
                {
                    key = (primaryKey, secondaryKey);
                }
                else if (primaryKey != null)
                {
                    key = primaryKey;
                }
                else if (secondaryKey != null)
                {
                    key = secondaryKey;
                }

                if (key == null)
                {
                    Engine.Framework.Api.Logger.Info($"Not found {nameof(T)}'s Key");
                    continue;
                }

                if (table.metadatas.ContainsKey(key) == false)
                {
                    table.metadatas.Add(key, metadata);
                }
                else
                {
                    Engine.Framework.Api.Logger.Info($"Duplicate Metadata Key. Type - {typeof(T)} , Key = {key}");
                }

            }



        }

        static public void LoadCsv<T>(string path) where T : Repository.Metadata, new()
        {
            try
            {
                using (var stream = File.OpenText(path))
                {
                    LoadCsv<T>(stream);
                    return;
                }
            }
            catch (Exception)
            {
                try
                {
                    LoadCsv<T>((StreamReader)null);
                }
                catch
                {

                }
            }
        }

        static public void LoadCsv<T>(StreamReader reader) where T : Repository.Metadata, new()
        {

            Repository table;
            if (tables.TryGetValue(typeof(T), out table) == false)
            {
                table = new Repository();
                tables.Add(typeof(T), table);
            }

            table.Clear();

            T loader = new T();

            T[] array = null;
            array = loader.CustomLoad(reader) as T[];

            if (array == null) { return; }


            foreach (T metadata in array)
            {
                var fields = metadata.GetType().GetFields();

                object primaryKey = null;
                object secondaryKey = null;
                foreach (var field in fields)
                {
                    var primary = field.GetCustomAttributes(typeof(Repository.Metadata.PrimaryKey), false);
                    var secondary = field.GetCustomAttributes(typeof(Repository.Metadata.SecondaryKey), false);

                    if (primary != null && primary.Length > 0)
                    {
                        primaryKey = field.GetValue(metadata);
                    }
                    if (secondary != null && secondary.Length > 0)
                    {
                        secondaryKey = field.GetValue(metadata);
                    }
                }

                table.metadatasToArray.Add(metadata);
                object key = null;
                if (primaryKey != null && secondaryKey != null)
                {
                    key = (primaryKey, secondaryKey);
                }
                else if (primaryKey != null)
                {
                    key = primaryKey;
                }
                else if (secondaryKey != null)
                {
                    key = secondaryKey;
                }

                if (key == null)
                {
                    Engine.Framework.Api.Logger.Info($"Not found {nameof(T)}'s Key");
                    continue;
                }

                if (table.metadatas.ContainsKey(key) == false)
                {
                    table.metadatas.Add(key, metadata);
                }
                else
                {
                    Engine.Framework.Api.Logger.Info($"Duplicate Metadata Key. Type - {typeof(T)} , Key = {key}");
                }

            }



        }

    }
}
