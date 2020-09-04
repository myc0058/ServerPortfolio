using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

using static Engine.Framework.Api;


namespace Engine.Database.Management.NoSql
{
    public sealed class Redis : Driver.ISession
    {

        internal static ConcurrentDictionary<int, Engine.Framework.Layer.Task> tasks = new ConcurrentDictionary<int, Framework.Layer.Task>();
        internal static Dictionary<string, Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>>> subscribers = new Dictionary<string, Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>>>();

        public Driver.ISession Create()
        {
            return null;
        }

        public static void Subscribe(string channel, Action<RedisChannel, RedisValue> callback, int strand = 0)
        {

            if (subscribers.TryGetValue(channel, out Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>> callbacks) == false)
            {
                callbacks = new Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>>();
                subscribers.Add(channel, callbacks);
            }


            if (callbacks.ContainsKey(callback) == false)
            {
                var task = new Framework.Layer.Task(Singleton<Database.Api.RedisLayer>.Instance);
                task = tasks.GetOrAdd(strand, task);

                task.UID = strand;


                var lambda = new Action<RedisChannel, RedisValue>((c, v) => {
                    task.PostMessage(() => { callback(c, v); });
                });

                callbacks.Add(callback, lambda);
                var sub = redis.GetSubscriber();
                sub.SubscribeAsync(channel, lambda);
            }


        }

        public static void Publish(string channel, RedisValue value)
        {
            redis.GetSubscriber().PublishAsync(channel, value);
        }

        public static void Unsubscribe(string channel, Action<RedisChannel, RedisValue> callback = null, CommandFlags flags = CommandFlags.None)
        {
            if (callback == null)
            {
                redis.GetSubscriber().Unsubscribe(channel, null, flags);
                return;
            }

            if (subscribers.TryGetValue(channel, out Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>> callbacks) == false)
            {
                callbacks = new Dictionary<Action<RedisChannel, RedisValue>, Action<RedisChannel, RedisValue>>();
                subscribers.Add(channel, callbacks);
            }



            callbacks.TryGetValue(callback, out Action<RedisChannel, RedisValue> wrapper);
            redis.GetSubscriber().Unsubscribe(channel, wrapper, flags);

        }
        public static void UnsubscribeAll(CommandFlags flags = CommandFlags.None)
        {
            redis.GetSubscriber().UnsubscribeAll(flags);
        }

        public string Name { get; set; }

        public class Address
        {
            public string IP { get; set; }
            public string Port { get; set; }
        }

        public Address Master = new Address();
        public List<Address> Slaves = new List<Address>();

        public string Id { get; set; }
        public string Pw { get; set; }
        public string Db { get; set; }

        //ServiceStack.Redis.PooledRedisClientManager redisManager = null;
        //public ThreadLocal<ServiceStack.Redis.RedisClient> Connection = new ThreadLocal<ServiceStack.Redis.RedisClient>();

        static ConnectionMultiplexer redis = null;


        public void Initialize() {

            var master = string.Format("{0}@{1}:{2}", Pw, Master.IP, Master.Port);

            
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints = { { Master.IP, short.Parse(Master.Port) }, },
                KeepAlive = 180,
                DefaultVersion = new Version(3, 2, 1),
                Password = Pw,
                AllowAdmin = true,
            };

            redis = ConnectionMultiplexer.Connect(config);


        }

        public void SetMaster(string ip, string port)
        {
            Master.IP = ip;
            Master.Port = port;
        }

        public void AddSlave(string ip, string port)
        {
            Slaves.Add(new Address() { IP = ip, Port = port });
        }

        public void BeginTransaction() { }
        public void Commit() { }
        public void Rollback() { }
        public Driver.ISession Open(bool transaction = true) { return this; }
        public void Close() { }
        public void CopyFrom(Driver.ISession value) { }

        public void Dispose() {}
    

        public IDatabase GetDatabase(int db)
        {
            return redis.GetDatabase(db);
        }
    }
}
