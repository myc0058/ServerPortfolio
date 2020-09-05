using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Engine.Framework;
using static Engine.Framework.Api;
using System.Diagnostics;

namespace Engine.Database.Management
{
	public class Driver
	{

        public interface ISession : IDisposable
		{
			void Initialize();
			void BeginTransaction();
			void Commit();
			void Rollback();
            ISession Open(bool transaction = true);
			void Close();
			void CopyFrom(ISession value);
            ISession Create();
		}

        public class Query
        {
            public class Rollback : System.Exception {
                public int ErrorCode { get; set; }
            }

            Layer.Task task = null;
            public Query() {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                OverrideQuery = async () => { };
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
                Strand = Layer.CurrentStrand;

                if (Layer.CurrentTask.Value == null)
                {
                    task = Singleton<Layer.Task>.Instance;
                    UID = task.UID;
                }
                else
                {
                    UID = Layer.CurrentTask.Value.UID;
                    task = Layer.CurrentTask.Value;
                }

                
            }

         
            internal void _Rollback()
            {
                foreach (var e in sessions)
                {
                    try
                    {
                        e.Rollback();
                    }
                    catch
                    {

                    }
                }
            }

            internal void Commit()
            {
                foreach (var e in sessions)
                {
                    try
                    {
                        e.Commit();
                    }
                    catch
                    {

                    }
                }
            }

            internal void Close()
            {
                foreach (var e in sessions)
                {
                    try
                    {
                        e.Close();
                    }
                    catch
                    {

                    }

                    try
                    {
                        e.Dispose();
                    }
                    catch
                    {

                    }
                }
                sessions = null;
            }
            List<Driver.ISession> sessions { get; set; } = new List<ISession>();
            public Driver.ISession GetSession(string name, bool open = true, bool transaction = true)
            {
                var session = Driver.GetSession(name, open, transaction);
                sessions.Add(session);
                return session;
            }

            public dynamic ResultSet { get; set; } = Singleton<Api.ResultSet>.Instance;
            public int RecordsAffected { get; set; }
            public Engine.Framework.AsyncCallback ResponseCallBack { get; set; }
            public virtual string Host { get; }

            public Func<Task> OverrideQuery { get; set; }
			public int Error { get; protected set; }
			public System.Exception Exception { get; protected set; }
            public int Strand { get; private set; }
            public long UID { get; private set; }
            public DateTime Timeout { get; internal set; }

            internal protected virtual void SetResult(int result)
			{
				this.Error = result;
                if (TCS == null)
                {
                    task?.PostMessage(ResponseCallBack);
                    return;
                }
                TCS?.SetResult(result);
			}

			internal protected virtual void SetResult(Exception e)
			{
				this.Exception = e;
				this.Error = -1;// e.HResult;
                TCS?.SetException(e);

			}
            public virtual void Execute() { }

            global::System.Threading.Tasks.TaskCompletionSource<int> TCS = null;
            //public virtual global::System.Threading.Tasks.Task<int> ExecuteAwaitable()
            //{
            //    TCS = new global::System.Threading.Tasks.TaskCompletionSource<int>();
            //    ResponseCallBack = null;
            //    Engine.Database.Management.Driver.Instance.Request(this);
            //    return TCS.Task;
            //}

            public virtual void ExecuteAsync(Engine.Framework.AsyncCallback callback = null)
			{
                if (callback != null) { ResponseCallBack = callback; }

                funcs.TryGetValue(UID, out ConcurrentQueue<Query> queue);
                if (queue == null)
                {
                    queue = new ConcurrentQueue<Query>();
                    funcs.TryAdd(UID, queue);
                }

                //lock (queue)
                {
                    queue.Enqueue(this);
                }

				//Engine.Database.Management.Driver.Instance.Request(this);
			}

   
			public virtual IEnumerable<string> GetHost()
			{
				yield break;
			}

		}
        public static Driver Instance = new Engine.Database.Management.Driver();

        
        public class NotFoundProcedure : Exception
		{
			public NotFoundProcedure(string value) { Name = value; }
			private string Name;
			public override string Message
			{
				get
				{
					return "NotFoundProcedure " + Name;
				}
			}
		};

        public class ResultError : Exception
        {
            public ResultError(int code) { ErrorCode = code; }
            public int ErrorCode { get; private set; }
        };


		//private BlockingCollection<Query>[] queries;
		private static ConcurrentQueue<Query>[] responses;
		private int process = 0;
		//private Thread[] threads;
		private bool isOpen = false;

		static protected Dictionary<string, ISession> sessions = new Dictionary<string, ISession>();

        static protected ISession[] arrSessions;
        static public int SessionCount { get { return sessions.Count; }}
		static public void AddSession<T>(string db, T value) where T : ISession
		{

			ISession session;
			if (sessions.TryGetValue(db, out session) == true)
			{
				session.CopyFrom(value);
			}
			else
			{
				sessions.Add(db, value);
			}

            arrSessions = sessions.Values.ToArray();

		}

        static internal void DisposeSessions()
        {
            foreach (var s in arrSessions)
            {
                try
                {
                    s.Dispose();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Dispose Sessions Exception !!!!!!\n" + e);

                }
            }
        }

        static public void BeginTransaction()
        {
            foreach (var s in arrSessions)
            {
                try
                {
                    s.Open();
                    s.BeginTransaction();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Begin Sessions Exception !!!!!!\n" + e);
                    throw;
                }

            }
        }
        static public void CommitSessions()
        {
            foreach (var s in arrSessions)
            {
                try
                {
                    s.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Commit Sessions Exception !!!!!!\n" + e);
                    throw;
                }

            }
        }

        static public void RollbackSessions()
        {
            foreach (var s in arrSessions)
            {
                try
                {
                    s.Rollback();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Rollback Sessions Exception !!!!!!\n" + e);
                }
            }
        }

        static internal ISession GetSession(string db, bool open = true, bool transaction = true)
		{
			ISession session;
			if (sessions.TryGetValue(db, out session) == true)
			{
                session = session.Create();
                if (open == true)
                {
                    session.Open(transaction);
                }
				return session;
			}
			return null;
		}

        public static ConcurrentDictionary<long, ConcurrentQueue<Query>> funcs = new ConcurrentDictionary<long, ConcurrentQueue<Query>>();


        public void Run()
		{

            if (isOpen == true) return;

            isOpen = true;



            foreach (var e in sessions)
            {
                e.Value.Initialize();
            }

            //threads = new Thread[Engine.Framework.Api.ThreadCount];
            //queries = new BlockingCollection<Query>[Engine.Framework.Api.ThreadCount];
            responses = new ConcurrentQueue<Query>[Engine.Framework.Api.ThreadCount];
            for (int i = 0; i < Engine.Framework.Api.ThreadCount; ++i)
			{
                int strand = i;
                //queries[strand] = new BlockingCollection<Query>();
                responses[strand] = new ConcurrentQueue<Query>();

			}

            new Thread(() =>
            {
                try
                {
                    Poll(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }).Start();
        }

		virtual protected bool IsOk()
		{
			return isOpen || (process > 0);
		}

		//public void Request(Query query)
		//{
  //          queries[query.Strand].Add(query);
		//}
	
		public void Close()
		{
			isOpen = false;

   //         for (int i = 0; i < Engine.Framework.Api.ThreadCount; ++i)
			//{

			//	threads[i].Join();

			//}

		}

		//protected bool Pop(int strand, out Query query)
		//{
		//	return queries[strand].TryTake(out query, 1000);
		//}


        protected void Poll(int strand)
		{
            ConcurrentDictionary<long, Query> inProgress = new ConcurrentDictionary<long, Query>();

            while (IsOk())
            {
                bool wait = true;

                if (inProgress.Count > 16)
                {
                    System.Threading.Thread.Sleep(16);
                    continue;
                }

                var now = DateTime.UtcNow;
                foreach (var p in inProgress)
                {
                    if (p.Value.Timeout > now)
                    {
                        continue;

                    }
                    //if (inProgress.TryRemove(p.Key, out Query query) == false)
                    //{
                    //    continue;
                    //}

                }

                foreach (var e in funcs)
                {
                    if (e.Value.Count == 0) { continue; }
                    if (inProgress.ContainsKey(e.Key)) { continue; }

                    if (e.Value.TryDequeue(out Query query) == false)
                    {
                        continue;
                    }

                    query.Timeout = DateTime.UtcNow.AddSeconds(10);
                    inProgress.TryAdd(e.Key, query);
                    wait |= e.Value.Count == 0;
                    Task.Run(async () =>
                    {

                        try
                        {
                            await query.OverrideQuery();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                query._Rollback();
                                query.SetResult(ex);
                            }
                            catch
                            {

                            }

                        }
                        finally
                        {
                            try
                            {
                                query.Commit();
                            }
                            catch
                            {

                            }

                            try
                            {
                                query.Close();
                            }
                            catch
                            {

                            }

                            try
                            {
                                inProgress.TryRemove(query.UID, out query);
                                query.SetResult(0);
                            }
                            catch
                            {

                            }
                        }
                    });
                }

                if (wait == true)
                {
                    System.Threading.Thread.Sleep(16);
                }
            }

            //         var task = new Engine.Framework.Layer.Task(Singleton<Database.Api.Layer>.Instance);
            //         task.UID = strand;
            //         Sessions.Value = new HashSet<ISession>();

            //while (IsOk())
            //{
            //             try
            //             {
            //                 Query o = null;

            //                 var sw = new Stopwatch();

            //                 while (Pop(strand, out o) == true)
            //                 {
            //                     sw.Restart();
            //                     try
            //                     {
            //                         o.OverrideQuery();
            //                         o.SetResult(0);
            //                         foreach (var s in Sessions.Value)
            //                         {
            //                             s.Commit();
            //                         }

            //                     }
            //                     catch (Query.Rollback e)
            //                     {
            //                         foreach (var s in Sessions.Value)
            //                         {
            //                             try
            //                             {
            //                                 s.Rollback();
            //                             }
            //                             catch (Exception re)
            //                             {
            //                                 Console.WriteLine($"------------ Exception in Rollback On CancelQueryException at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
            //                                 Console.WriteLine(re);
            //                             }
            //                         }
            //                         o.SetResult(e);
            //                     }
            //                     catch (Exception e)
            //                     {
            //                         foreach (var s in Sessions.Value)
            //                         {
            //                             try
            //                             {
            //                                 s.Rollback();
            //                             }
            //                             catch (Exception re)
            //                             {
            //                                 Console.WriteLine($"------------ Exception in Rollback at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
            //                                 Console.WriteLine(re);
            //                             }

            //                         }
            //                         o.SetResult(e);
            //                         Console.WriteLine($"==== SQL Error Begin at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")} ====");
            //                         Console.WriteLine(e);
            //                         Console.WriteLine("==== SQL Error End ====");
            //                     }
            //                     finally
            //                     {
            //                         foreach (var s in Sessions.Value)
            //                         {
            //                             try
            //                             {
            //                                 s.Dispose();
            //                             }
            //                             catch (Exception de)
            //                             {
            //                                 Console.WriteLine($"------------ Exception in Dispose at {DateTime.UtcNow.ConvertTimeFromUtc("Korea Standard Time")}");
            //                                 Console.WriteLine(de);
            //                             }

            //                         }

            //                         Sessions.Value.Clear();
            //                         task.PostMessage(o.ResponseCallBack);

            //                     }
            //                 }
            //             }
            //             catch (Exception e)
            //             {

            //             }


            //}


        }


		public uint GetQueryCount()
		{

			uint count = 0;
			//foreach (var p in queries)
			//{
			//	count += (uint)p.Count;
			//}

			return count;
		}
	} 
}
