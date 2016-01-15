using System;
using StackExchange.Redis;

namespace redistest
{
    internal class RedisEvents
    {
        private IConnectionMultiplexer Redis { get; set; }

        public RedisEvents(IConnectionMultiplexer redis)
        {
            Redis = redis;
        }

        public void EventsOperations()
        {
            /*
             * IConnectionMultiplexer exposes the following events:
             *
             * Redis.ConfigurationChanged;
             * Redis.ConfigurationChangedBroadcast;
             * Redis.ConnectionFailed;
             * Redis.ConnectionRestored;
             * Redis.ErrorMessage;
             * Redis.HashSlotMoved;
             * Redis.InternalError;
             *
            */
        }
    }
}