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
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/KeysScan.md"/></remarks>
    public class RedisServerVsDb
    {
        private IConnectionMultiplexer Redis { get; set; }

        public RedisServerVsDb(IConnectionMultiplexer redis) 
        {
            Redis = redis;
        }

        public void ServerVsDbOperations()
        {
            // get the target server
            var server = Redis.GetServer("localhost:6379");

            // show all keys in database 0 that include "foo" in their name
            foreach(var key in server.Keys(pattern: "*foo*"))
            {
                Console.WriteLine(key);
            }

            // completely wipe ALL keys from database 0
            // needs admin mode
            //server.FlushDatabase();
        }
    }
}
