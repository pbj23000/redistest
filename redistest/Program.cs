using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Net;

namespace redistest
{
    class Program
    {
        static void Main(string[] args)
        {
            // keep this around
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");

            // Basic Usage
            System.Console.WriteLine("Basic Usage");
            var basic = new RedisBasicUsage(redis);
            basic.BasicUsageOperations();
            System.Console.ReadLine();

            // Configuration
            System.Console.WriteLine("Configuration Usage");
            var configuation = new RedisConfiuguration(redis);
            configuation.ConfigurationOperations();
            System.Console.ReadLine();

            // Piplines and Multiplexers
            System.Console.WriteLine("Pipelines and Multiplexers");
            var pipelines = new RedisPipelinesAndMux(redis);
            pipelines.PipelinesAndMultiplexersOperations();
            System.Console.ReadLine();

            // Keys, Values, Channels
            System.Console.WriteLine("Keys, Values, Channles");
            var keysvalues = new RedisKeyValueChannel(redis);
            keysvalues.KeyValueChannelOperations();
            System.Console.ReadLine();

            // Transactions
            System.Console.WriteLine("Transactions");
            var transactions = new RedisTransactions(redis);
            transactions.TransactionOperations();
            System.Console.ReadLine();

            // Events
            System.Console.WriteLine("Events");
            var events = new RedisEvents(redis);
            events.EventsOperations();
            System.Console.ReadLine();

            // PubSub
            System.Console.WriteLine("PubSub");
            var pubsub = new RedisPubSub(redis);
            pubsub.PubSubOperations();
            System.Console.ReadLine();

            // Server vs Database 
            System.Console.WriteLine("ServerVsDb");
            var servervsdb = new RedisServerVsDb(redis);
            servervsdb.ServerVsDbOperations();
            System.Console.ReadLine();

            // Profiling
            System.Console.WriteLine("Profiling");
            var profiling = new RedisProfiling(redis);
            profiling.ProfilingOperations();
            System.Console.ReadLine();
        }
    }
}
