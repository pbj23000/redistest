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
    /// <remarks><see href="https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/Transactions.md"></see></remarks>
    public class RedisTransactions
    {
        IDatabase Db { get; set; }

        public RedisTransactions(IConnectionMultiplexer redis)
        {
            Db = redis.GetDatabase();
        }

        public void TransactionOperations()
        {
            string custKey = "custKey";
            var newId = CreateNewId();
            var tran = Db.CreateTransaction();
            tran.AddCondition(Condition.HashNotExists(custKey, "UniqueID"));
            tran.HashSetAsync(custKey, "UniqueID", newId);
            bool committed = tran.Execute(); // true if applied, false if rolled back

            // using When
            var newId2 = CreateNewId();
            bool wasSet = Db.HashSet(custKey, "UniqueID", newId2, When.NotExists);
        }

        private string CreateNewId()
        {
            return new Guid().ToString();
        }
    }
}
