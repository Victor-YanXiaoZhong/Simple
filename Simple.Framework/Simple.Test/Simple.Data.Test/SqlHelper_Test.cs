using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Simple.Data.Test.Entity;
using Simple.Utils.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Data.Test
{
    internal class SqlHelper_Test
    {
        private SqlHelper sqlHelper;

        private void Db_SqlPrint(string sql, string param)
        {
            Console.WriteLine("SQL:" + sql + "，参数：" + param);
        }

        [SetUp]
        public void Setup()
        {
            ConfigHelper.Init(new string[] { "appsettings.json" }, false, true);
            SqlHelper.RegistFactoty("Microsoft.Data.Sqlite", Microsoft.Data.Sqlite.SqliteFactory.Instance);
            SqlHelper.RegistFactoty("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
            SqlHelper.RegistFactoty("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            SqlHelper.SqlPrint += Db_SqlPrint;
            sqlHelper = new SqlHelper("AdminDb");
        }

        [Test]
        public void Test_SqlHelper()
        {
            var r1 = sqlHelper.TestConection();
            var r2 = sqlHelper.ServerVersion();
            var r3 = sqlHelper.ExecuteDataTable("SELECT * FROM Sys_User");
            Assert.That(r3.Rows.Count > 1);
            var r41 = sqlHelper.GetList<SysRole>("SELECT * FROM Sys_User");
            var r4 = sqlHelper.GetList<SysRole>("SELECT * FROM Sys_Role");
            var r5 = sqlHelper.FirstOrDefault<SysRole>("SELECT * FROM Sys_Role");

            var r6 = sqlHelper.GetList<SysRole>("SELECT * FROM Sys_Role where Id>@id",
                parameters: new { id = 1 });
            var r7 = sqlHelper.FirstOrDefault<SysRole>("SELECT * FROM Sys_Role where Id=@id",
                 parameters: new { id = 1 });
        }
    }
}