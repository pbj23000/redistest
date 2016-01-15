using System;
using StackExchange.Redis;

namespace redistest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/PubSubOrder.md">HERE</see></remarks>
    internal class RedisPubSub
    {
        private IConnectionMultiplexer Redis { get; set; } 

        public RedisPubSub(IConnectionMultiplexer redis)
        {
            Redis = redis;
        }

        public void PubSubOperations()
        {
            System.Console.WriteLine("Default sequential processing: Preserve sync order = {0}", Redis.PreserveAsyncOrder.ToString());
            // default: true - sequential processing in the same order in which they are received; but one message can delay another.
            // false - concurrent processing which can corrupt internal state, if messages depend on each other; however this is much faster and more scalable.
            Redis.PreserveAsyncOrder = false;
            System.Console.WriteLine("Modified for concurrent performance: Preserve sync order = {0}", Redis.PreserveAsyncOrder.ToString());
        }
    }
}