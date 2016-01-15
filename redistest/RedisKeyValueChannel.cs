// RedisKeyValueChannel.cs
// compile with /doc:RedisKeyValueChannel.xml
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redistest
{
    /// <summary>
    /// RedisValueChannel.cs
    /// </summary>
    /// <remarks>
    /// Code based on the documentation found here: 
    /// <see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/KeysValues.md">Keys Values Channel</see>
    /// </remarks>

    public class RedisKeyValueChannel
    {
        private IDatabase Db { get; set; }

        public RedisKeyValueChannel(IConnectionMultiplexer redis)
        {
            Db = redis.GetDatabase();
        }

        public void KeyValueChannelOperations()
        {
            // keys
            string key = "myKey";
            Db.StringSet(key, "1");
            Db.StringIncrement(key);
            // or
            byte[] byteKey = new byte[2] { Byte.MinValue, Byte.MaxValue };
            Db.StringSet(byteKey, "666");
            Db.StringIncrement(byteKey);
            // or
            string someKey = Db.KeyRandom();

            // values
            Db.StringSet("mykey", "myvalue");
            Db.StringSet("mykey", 123); // this is still a rediskey and redisvalue
            int i = (int)Db.StringGet("mykey");

            Db.KeyDelete("abc1");
            int j = (int)Db.StringGet("abc"); // this is ZERO

            Db.KeyDelete("abc2");
            var value1 = Db.StringGet("abc2");
            bool isNil = value1.IsNull; // this is true

            Db.KeyDelete("abc3");
            var value2 = (int?)Db.StringGet("abc3"); // behaves as you would expect
        }
    }
}
