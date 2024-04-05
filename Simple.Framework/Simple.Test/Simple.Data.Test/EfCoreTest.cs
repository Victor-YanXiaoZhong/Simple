using NUnit.Framework;
using Simple.Socket;

using Simple.Utils;
using System.Linq;

namespace Simple.Dapper.Test
{
    public class EfCoreTest : DbComponentTest
    {
        [Test]
        public void Connect()
        {
            Assert.That(TestEfDb.Database.CanConnect());
        }

        [Test]
        public void Select()
        {
            var list = TestEfDb.SysFunction.ToList();
            TcpLogClient.SendLog("查询数据：" + JsonHelper.ToJson(list));
        }

        [Test]
        public void FilterTest()
        {
            var fileter = Filter.Filters(
                new Filter { field = "name", op = "like", value = "初" },
                new Filter { field = "enabled", op = "=", value = "true" },
                new Filter { field = "DELETED", op = "=", value = "true", isAnd = false },
                new Filter { field = "Id", value = "2", isAnd = true }
            );

            var func = FiterExpressHelper.GetWhere<SysFunction>(fileter);
            var list = TestEfDb.SysFunction.Where(func).ToList();
        }
    }
}