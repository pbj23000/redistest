using System;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace redistest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Profiling.md">HERE</see></remarks>
    public class RedisProfiling
    {
        private IConnectionMultiplexer Redis { get; set; }

        public RedisProfiling(IConnectionMultiplexer redis)
        {
            Redis = redis;
        }

        public void ProfilingOperations()
        {
            AssociateCommandsIssuedFromManyDifferentThreadsTogether1();
            AssociateCommandsIssuedFromManyDifferentThreadsTogether2();
        }

        private void AssociateCommandsIssuedFromManyDifferentThreadsTogether1()
        {
            var toyProfiler1 = new ToyProfiler();
            var thisGroupContext = new object();
            Redis.RegisterProfiler(toyProfiler1);

            var threads = new List<Thread>();

            for (var i = 0; i < 16; i++)
            {
                var db = Redis.GetDatabase(i);

                var thread =
                    new Thread(
                        delegate ()
                        {
                            var threadTasks = new List<Task>();

                            for (var j = 0; j < 1000; j++)
                            {
                                var task = db.StringSetAsync("" + j, "" + j);
                                threadTasks.Add(task);
                            }

                            Task.WaitAll(threadTasks.ToArray());
                        });

                toyProfiler1.Contexts[thread] = thisGroupContext;

                threads.Add(thread);
            }

            Redis.BeginProfiling(thisGroupContext);

            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());

            IEnumerable<IProfiledCommand> timings = Redis.FinishProfiling(thisGroupContext);
        }

        private void AssociateCommandsIssuedFromManyDifferentThreadsTogether2()
        {
            var toyProfiler2 = new ToyProfiler();

            Redis.RegisterProfiler(toyProfiler2);

            var threads = new List<Thread>();

            var perThreadTimings = new ConcurrentDictionary<Thread, List<IProfiledCommand>>();

            for (var i = 0; i < 16; i++)
            {
                var db = Redis.GetDatabase(i);

                var thread =
                    new Thread(
                        delegate ()
                        {
                            var threadTasks = new List<Task>();

                            Redis.BeginProfiling(Thread.CurrentThread);

                            for (var j = 0; j < 1000; j++)
                            {
                                var task = db.StringSetAsync("" + j, "" + j);
                                threadTasks.Add(task);
                            }

                            Task.WaitAll(threadTasks.ToArray());

                            perThreadTimings[Thread.CurrentThread] = Redis.FinishProfiling(Thread.CurrentThread).ToList();
                        });

                toyProfiler2.Contexts[thread] = thread;

                threads.Add(thread);
            }
        }

        private class ToyProfiler : IProfiler
        {
            public ConcurrentDictionary<Thread, object> Contexts = new ConcurrentDictionary<Thread, object>();

            public object GetContext()
            {
                object ctx;
                if (!Contexts.TryGetValue(Thread.CurrentThread, out ctx)) ctx = null;

                return ctx;
            }
        }

        // an example for MVC apps
        // register the following against ConnectionMultiplexer
        /* 
        * doesn't compile, needs mvc5
        public class RedisProfiler : IProfiler
        {
            const string RequestContextKey = "RequestProfilingContext";

            public object GetContext()
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;

                return ctx.Items[RequestContextKey];
            }

            public object CreateContextForCurrentRequest()
            {
                var ctx = HttpContext.Current;
                if (ctx == null) return null;

                object ret;
                ctx.Items[RequestContextKey] = ret = new object();

                return ret;
            }
        }

        // put the following in Global.asax.cs
        protected void Application_BeginRequest()
        {
            var ctxObj = RedisProfiler.CreateContextForCurrentRequest();
            if (ctxObj != null) { Redis.BeginProfiling(ctxObj); }
        }

        protected void Application_EndRequest()
        {
            var ctxObj = RedisProfiler.GetContext();
            if (ctxObj != null)
            {
                var timings = Redis.FinishProfiling(ctxObj);
                // do what you will with timings here
            }
        }
        */
    }
}