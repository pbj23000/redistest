using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace redistest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Basics.md">Here</see></remarks>
    public class RedisBasicUsage
    {
        private IConnectionMultiplexer Redis { get; set; }

        public RedisBasicUsage(IConnectionMultiplexer redis)
        {
            Redis = redis;
        }

        public void BasicUsageOperations()
        {
            // sync
            IDatabase db = Redis.GetDatabase();

            // useing the redis api
            string value = "abcdefg";
            db.StringSet("mkey", value);

            // ... later ...
            string latervalue = db.StringGet("mkey");
            Console.WriteLine(latervalue);

            // pub/sub
            ISubscriber sub = Redis.GetSubscriber();
            sub.Subscribe("messages", (channel, message) =>
            {
                Console.WriteLine((string)message);
            });

            sub.Publish("messages", "hello");

            // server specific - the exception to the rule
            IServer server = Redis.GetServer("localhost", 6379);
            EndPoint[] endpoints = Redis.GetEndPoints();
            foreach (EndPoint ep in endpoints)
            {
                System.Console.WriteLine(ep);
            }

            DateTime lastSave = server.LastSave();
            System.Console.WriteLine(lastSave);

            /*
             * needs admin authz 
             *
            ClientInfo[] clients = server.ClientList();
            foreach (ClientInfo client in clients)
            {
                System.Console.WriteLine(client);
            }
            */
        }
    }
}
