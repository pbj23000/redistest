using System;
using StackExchange.Redis;
using System.ComponentModel;
using System.Collections.Generic;

namespace redistest
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Configuration.md">HERE</see></remarks>
    public class RedisConfiuguration
    {
        private IConnectionMultiplexer Redis { get; set; }

        public RedisConfiuguration(IConnectionMultiplexer redis)
        {
            Redis = redis;
        }

        public void ConfigurationOperations()
        {
            // basic usage of configuration strings
            string basiclocalhost = "localhost";
            var conn1 = ConnectionMultiplexer.Connect(basiclocalhost);

            string configlocalhost1 = "redis0:6380,redis1:6380,allowAdmin=true";
            string configlocalhost2 = "localhost,allowAdmin=true";
            var conn2 = ConnectionMultiplexer.Connect(configlocalhost2);

            // using ConfigurationOptions
            ConfigurationOptions options1 = ConfigurationOptions.Parse(configlocalhost1);
            // or
            string configString1 = options1.ToString();
            System.Console.WriteLine(configString1);

            // common usage
            // 1. get config
            string configString2 = GetRedisConfiguration();
            System.Console.WriteLine(configString2);
            var options2 = ConfigurationOptions.Parse(configString2);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options2))
            {
                string name = descriptor?.Name;
                string value = descriptor?.GetValue(options2)?.ToString();
                Console.WriteLine("{0}={1}", name, value);
            }
            // 2. set some options
            options2.ClientName = GetAppName();
            options2.AllowAdmin = true;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options2))
            {
                string name = descriptor?.Name;
                string value = descriptor?.GetValue(options2)?.ToString();
                Console.WriteLine("{0}={1}", name, value);
            }
            // 3. connect with final options
            var conn3 = ConnectionMultiplexer.Connect(options2);

            // automatic and manual config
            ConfigurationOptions options3 = new ConfigurationOptions
            {
                EndPoints = { { "redis0", 6379 }, { "redis1", 6380 } },
                CommandMap = CommandMap.Create(new HashSet<string>
                { // EXCLUDE a few commands 
                    "INFO", "CONFIG", "CLUSTER",
                    "PING", "ECHO", "CLIENT"
                }, available: false),
                KeepAlive = 180,
                DefaultVersion = new Version(2, 8, 8),
                Password = "changeme"
            };
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options3))
            {
                string name = descriptor?.Name;
                string value = descriptor?.GetValue(options2)?.ToString();
                Console.WriteLine("{0}={1}", name, value);
            }

            // rename commands
            var commands1 = new Dictionary<string, string>
            {
                { "info", null },
                { "select", "use" }
            };
            var options4 = new ConfigurationOptions
            {
                CommandMap = CommandMap.Create(commands1)
            };
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options4))
            {
                string name = descriptor?.Name;
                string value = descriptor?.GetValue(options2)?.ToString();
                Console.WriteLine("{0}={1}", name, value);
            }

            // twemproxy - an alternate way of implementing redis clustering
            var options5 = new ConfigurationOptions
            {
                EndPoints = { "my-server" },
                Proxy = Proxy.Twemproxy
            };
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options5))
            {
                string name = descriptor?.Name;
                string value = descriptor?.GetValue(options2)?.ToString();
                Console.WriteLine("{0}={1}", name, value);
            }
        } 

        private string GetAppName()
        {
            return System.AppDomain.CurrentDomain.FriendlyName;
        }

        private string GetRedisConfiguration()
        {
            string configlocalhost1 = "redis0:6380,redis1:6380,allowAdmin=true";
            string configlocalhost2 = "localhost,allowAdmin=true";
            return ConfigurationOptions.Parse(configlocalhost2).ToString();
        }
    }
}