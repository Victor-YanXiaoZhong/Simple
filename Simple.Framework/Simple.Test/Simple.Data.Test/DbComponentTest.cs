using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Simple.Socket;
using Simple.Utils.Helper;
using System;
using System.Threading;

namespace Simple.Dapper.Test
{
    public partial class DbComponentTest
    {
        protected DbComponent SqlLiteDb;
        protected DbComponent MssqlDb;
        protected DbComponent MysqlDb;
        protected TestDbcontext TestEfDb;

        private void DbComponent_SqlPrint(string arg1, string arg2)
        {
            TcpLogClient.SendLog(arg1 + arg2);
            Console.WriteLine(arg1, arg2);
        }

        [SetUp]
        public void Setup()
        {
            ConfigHelper.Init(new string[] { "appsettings.json" }, false, true);
            DbComponent.RegistFactoty("Microsoft.Data.Sqlite", Microsoft.Data.Sqlite.SqliteFactory.Instance);
            DbComponent.RegistFactoty("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            DbComponent.RegistFactoty("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            DbComponent.SqlPrint += DbComponent_SqlPrint;
            SqlLiteDb = DbComponent.Instance("defaultDb");
            MysqlDb = DbComponent.Instance("mysqlDb");
            MssqlDb = DbComponent.Instance("AdminDb");
            var dbbuilder = new DbContextOptionsBuilder
            {
            };
            TestEfDb = new TestDbcontext(ConfigHelper.GetConnectionString("AdminDb"));
        }

        [Test]
        public void DbComponentInit()
        {
            var sqlLiteconnected = SqlLiteDb.TestConection();
            var mssqlconnected = MssqlDb.TestConection();
            //var mysqlconnected = MysqlDb.TestConection();
            Assert.That(sqlLiteconnected);
            Assert.That(mssqlconnected);
            TcpLogClient.SendLog("组件初始化成功");

            Thread.Sleep(5000);
        }
    }
}