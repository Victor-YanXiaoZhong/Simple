using NUnit.Framework;
using System;

namespace Simple.Dapper.Test
{
    public class SlqLiteDbTest : DbComponentTest
    {
        [Test]
        public void SqlLiteConnectTest()
        {
            var isSuccess = SqlLiteDb.TestConection();
            Assert.That(isSuccess);
        }

        [Test]
        public void Insert()
        {
            var entity = SqlLiteDb.Insert(new SysUser
            {
                Name = "admin",
                Dec = "管理员",
                StateCode = "0"
            });
            Assert.That(entity.Id > 0);
        }

        [Test]
        public void Update()
        {
            var entity = SqlLiteDb.FirstOrDefault<SysUser>(new { Id = 1 });
            Assert.That(entity != null);
            var rand = new Random();

            entity.StateCode = rand.Next(100, 1000).ToString();
            entity.UpdateTime = System.DateTime.Now.ToLongDateString();
            var update = SqlLiteDb.Update(entity);

            Assert.That(update);

            entity.Dec = "哈哈";
            var updatefield = SqlLiteDb.UpdateFields(entity, new string[] { "Name" });

            SqlLiteDb.UpdateFields(entity, new { Name = "我是新名称" });

            updatefield = SqlLiteDb.Update<SysUser>(new { Name = "哈哈哈，我是最新数据" }, new { Id = 1 }) > 0;
            Assert.That(updatefield);
        }
    }
}