using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redistest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/PipelinesMultiplexers.md"></see></remarks>
    public class RedisPipelinesAndMux
    {

        private IDatabase Db { get; set; }
        private ISubscriber Sub { get; set; }

        public RedisPipelinesAndMux(IConnectionMultiplexer redis)
        {
            Db = redis.GetDatabase();
            Sub = redis.GetSubscriber();
        }

        public async void PipelinesAndMultiplexersOperations()
        {
            // pipelining
            // sync
            string a1 = Db.StringGet("a");
            string b1 = Db.StringGet("b");

            // async
            var aPending = Db.StringGetAsync("a");
            var bPending = Db.StringGetAsync("b");
            var a2 = Db.Wait(aPending);
            var b2 = Db.Wait(bPending);

            // fire and forget
            // sliding expiration
            string key = "a";
            Db.KeyExpire(key, TimeSpan.FromMinutes(5), flags: CommandFlags.FireAndForget);
            var value = (string)Db.StringGet(key);

            // multiplexing
            var channel = "work";
            Sub.Subscribe(channel, delegate
            {
                string work = Db.ListRightPop(key);
                if (work != null) Process(work);
            });

            string newWork = "newWork";
            Db.ListLeftPush(key, newWork, flags: CommandFlags.FireAndForget);
            Sub.Publish(channel, "");

            // concurrency
            string result = await Concurrency();

        }

        private void Process(string work)
        {
            System.Console.WriteLine("Work:{0}", work);
        }

        private async Task<string> Concurrency()
        {
            var key = "myKey";
            string value = await Db.StringGetAsync(key);
            if (value == null)
            {
                value = await ComputeValueFromDatabase(key);
                Db.StringSet(key, value, flags: CommandFlags.FireAndForget);
            }
            return value;
        }

        private Task<string> ComputeValueFromDatabase(string key)
        {
            var value = String.Format("{0}:{1}", key, key + "value");
            return Task.FromResult<string>(value);
        }
    }
}
