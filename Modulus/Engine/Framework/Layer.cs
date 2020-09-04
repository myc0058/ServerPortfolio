using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Engine.Framework.Api;

namespace Engine.Framework
{
    public class Layer
    {
        public static int CurrentStrand { get => currentStrand.Value; set => currentStrand.Value = value; }
        internal static ThreadLocal<int> currentStrand = new ThreadLocal<int>();
        public class Task
        {
            private long uid = 0;
            public long UID
            {
                get => uid;
                set
                {
                    if (uid == value) { return; }
                    uid = value;
                    Strand = (int)value;
                }
            }

            public int strand { get; private set; }
            public int Strand
            {
                get => strand; 
                set {
                    strand = Math.Abs((int)(value % Engine.Framework.Api.ThreadCount));
                }
            }
            public Task(Layer layer) { 
                this.layer = layer; 
                UID = Engine.Framework.Api.UniqueKey;
            }

            public Task()
            {
                this.layer = Singleton<Engine.Framework.Layer>.Instance;
                UID = Engine.Framework.Api.UniqueKey;
            }
            internal enum State
            {
                IDLE = 0,
                WAIT,
                RUN,
                CLOSING,
                CLOSE,
            }
            protected internal int post = 0;
            protected int state = (int)State.IDLE;
            internal bool interrupted = false;
            internal Layer layer;
            internal ConcurrentQueue<Engine.Framework.AsyncCallback> messages = new ConcurrentQueue<Engine.Framework.AsyncCallback>();

            internal protected virtual void OnClose()
            {

            }

            public virtual bool IsClose()
            {
                if (state == (int)State.CLOSING || state == (int)State.CLOSE)
                {
                    return true;
                }
                return false;
            }

            protected internal virtual void OnUpdate()
            {
            }

            public void Update()
            {
                messages.Enqueue(this.OnUpdate);
                if (ToWait())
                {
                    layer.Post(this);
                }
                Api.MainThreadResetEvent.Set();
            }
            public void Close()
            {

                if (Interlocked.Exchange(ref state, (int)State.CLOSE) == (int)State.CLOSE) { return; }
                this.Interrupt();
                layer.Close(this);

                //Engine.Framework.Layers.Entity.Close(this);

            }

            public void Interrupt()
            {
                interrupted = true;
            }

            internal bool ToRun()
            {
                Interlocked.Exchange(ref post, 0);
                if (Interlocked.CompareExchange(ref state, (int)State.RUN, (int)State.WAIT) == (int)State.WAIT)
                {
                    return true;
                }
                return false;
            }
            internal bool ToWait()
            {

                Interlocked.Increment(ref post);
                if (Interlocked.CompareExchange(ref state, (int)State.WAIT, (int)State.IDLE) == (int)State.IDLE)
                {
                    return true;
                }

                return false;
            }
            internal bool ToIdle()
            {
                if (Interlocked.CompareExchange(ref state, (int)State.IDLE, (int)State.RUN) == (int)State.RUN)
                {
                    return true;
                }
                return false;
            }

            public bool PostMessage(Engine.Framework.AsyncCallback callback)
            {
                if (callback == null) { return false; }
                if (state == (int)State.CLOSE) {

                    Engine.Framework.Api.Logger.Warning($"[WARNING] PostMessage To CLOSE Entity {this.GetType()}");
                    return false;
                }
                messages.Enqueue(callback);
                if (ToWait())
                {
                    layer.Post(this);
                }
                Api.MainThreadResetEvent.Set();
                
                return true;
            }

            internal protected virtual void OnException(Exception e) {
                Engine.Framework.Api.Logger.Error(e);
            }

            protected internal bool IsPost() { 
                return (post > 0) || (messages.Count > 0); 
            }

        }

        internal ConcurrentDictionary<int, ConcurrentQueue<Task>> _entities = new ConcurrentDictionary<int, ConcurrentQueue<Task>>();
        internal ConcurrentDictionary<int, ConcurrentQueue<Task>> _waitClose = new ConcurrentDictionary<int, ConcurrentQueue<Task>>();
        internal List<Scheduler> schedules = new List<Scheduler>();
        internal static ParallelOptions options = new ParallelOptions() { MaxDegreeOfParallelism = Engine.Framework.Api.ThreadCount };
        public Layer()
        {
            if (options.MaxDegreeOfParallelism != Engine.Framework.Api.ThreadCount)
            {
                options = new ParallelOptions() { MaxDegreeOfParallelism = Engine.Framework.Api.ThreadCount };
            }

            for (int i = 0; i < Engine.Framework.Api.ThreadCount; ++i)
            {
                _entities.TryAdd(i, new ConcurrentQueue<Task>());
                _waitClose.TryAdd(i, new ConcurrentQueue<Task>());
            }

            Engine.Framework.Api.Add(this);
        }

        public virtual int OnUpdate() { return 0; }
        public static ThreadLocal<Task> CurrentTask = new ThreadLocal<Task>();
        
        internal virtual int Update()
        {
            int remainTask = 0;

            try
            {
                remainTask += OnUpdate();
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
            }

            try
            {
                remainTask += ProcessClose(remainTask);
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
            }

            try
            {
                remainTask += ProcessTask(remainTask);
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
            }

            try
            {
                ProcessSchedule();
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
            }

            return remainTask;
        }

        private int ProcessSchedule()
        {
            long now = DateTime.UtcNow.Ticks;


            Scheduler[] array = null;
            
            lock (schedules)
            {
                array = schedules.ToArray();
                schedules.Clear();
            }
            
            foreach (var e in array)
            {
                if (e == null || e.IsClose() == true) continue;
                if (e.interval < 0)
                {
                    continue;
                }

                if (e.IsPause == true)
                {
                    e.Next = DateTime.UtcNow.AddMilliseconds(e.interval).Ticks;
                    continue;
                }

                if (e.Next > now)
                {
                    Schedule(e);
                    continue;
                }

                long deltaTicks = now - e.Pre;
                e.Next = DateTime.UtcNow.AddMilliseconds(e.interval).Ticks;
                Layer.currentStrand.Value = e.Strand;
                CurrentTask.Value = e;
                e.OnSchedule(deltaTicks);
                e.Pre = now;
                Schedule(e);
            }
            return 0;
        }

        private int ProcessTask(int remainTask)
        {
            Parallel.ForEach(_entities, options, (tasks) =>
            {

                Task task = null;

                int max = tasks.Value.Count;
                while (max > 0)
                {
                    --max;
                    if (tasks.Value.TryDequeue(out task) == false)
                    {
                        break;
                    }

                    Layer.currentStrand.Value = tasks.Key;
                    task.interrupted = false;
                    if (task.ToRun())
                    {
                        CurrentTask.Value = task;
                        try
                        {
                            for (int c = 0; task.messages.Count > 0 && c < 5 && task.interrupted == false && task.Strand == tasks.Key; ++c)
                            {
                                Engine.Framework.AsyncCallback callback = null;
                                if (task.messages.TryDequeue(out callback) == false) { break; }

                                try
                                {
                                    callback();
                                }
                                catch (Exception e)
                                {
                                    task.OnException(e);

                                }

                            }

                        }
                        catch (Exception ee)
                        {
                            Engine.Framework.Api.Logger.Info(ee);
                        }
                        finally
                        {
                            CurrentTask.Value = null;
                        }

                        task.interrupted = false;
                        task.ToIdle();
                        if (task.IsPost())
                        {
                            if (task.ToWait())
                            {
                                Post(task);
                            }
                        }
                    }
                }

                if (tasks.Value.Count > 0)
                {
                    Interlocked.Increment(ref remainTask);
                }


            });
            return remainTask;
        }

        private int ProcessClose(int remainTask)
        {
            Parallel.ForEach(_waitClose, options, (tasks) =>
            {
                Task task = null;
                int max = tasks.Value.Count;
                while (max > 0)
                {
                    --max;
                    if (tasks.Value.TryDequeue(out task) == false)
                    {
                        break;
                    }

                    if (task.Strand != tasks.Key)
                    {
                        Close(task);
                        continue;
                    }

                    Layer.currentStrand.Value = tasks.Key;

                    CurrentTask.Value = task;


                    try
                    {
                        Engine.Framework.AsyncCallback callback = null;
                        while (task.messages.TryDequeue(out callback) == true)
                        {
                            try
                            {
                                callback();
                            }
                            catch (Exception e)
                            {
                                task.OnException(e);
                            }
                        }

                    }
                    catch
                    {

                    }

                    try
                    {
                        task.OnClose();
                    }
                    catch (Exception e)
                    {
                        Engine.Framework.Api.Logger.Error(e);
                    }
                    finally
                    {
                        CurrentTask.Value = null;
                    }

                }

                if (tasks.Value.Count > 0)
                {
                    Interlocked.Increment(ref remainTask);
                }


            });
            return remainTask;
        }

        internal void Post(Task e)
        {

            ConcurrentQueue<Task> tasks = null;

            if (_entities.TryGetValue(e.Strand, out tasks) == false)
            {
                return;
            }

            tasks.Enqueue(e);
        }

        internal void Schedule(Scheduler task)
        {
            lock (schedules)
            {
                schedules.Add(task);
            }
        }

        internal void Close(Task entity)
        {
            ConcurrentQueue<Task> tasks = null;
            if (_waitClose.TryGetValue(entity.Strand, out tasks) == false)
            {
                return;
            }

            tasks.Enqueue(entity);
        }

        internal void Close()
        {
        }

        static int Idendity = 1;
    }


}
