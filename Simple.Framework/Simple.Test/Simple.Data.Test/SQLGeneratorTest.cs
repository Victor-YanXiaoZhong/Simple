using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Dapper.Test
{
    public class SQLGeneratorTest
    {
        private ISQLGenerator sqlserverGenerator;
        private ISQLGenerator mysqlGenerator;
        private ISQLGenerator sqlLiteGenerator;
        private ClassMap classMap;

        [SetUp]
        public void Setup()
        {
            sqlserverGenerator = new SQLServerGenerator();
            mysqlGenerator = new MySqlGenerator();
            sqlLiteGenerator = new SQLiteGenerator();
            classMap = ClassMapHelper.GetMap<SysUser>();
        }

        [Test]
        public void SqlServer()
        {
            var select = sqlserverGenerator.Select(classMap);
            var insert = sqlserverGenerator.Insert(classMap);
            var update = sqlserverGenerator.Update(classMap);
        }

        [Test]
        public void mySqlServer()
        {
            var select = mysqlGenerator.Select(classMap);
            var insert = mysqlGenerator.Insert(classMap);
            var update = mysqlGenerator.Update(classMap);
        }

        [Test]
        public void SqlLiteServer()
        {
            var select = sqlLiteGenerator.Select(classMap);
            var insert = sqlLiteGenerator.Insert(classMap);
            var update = sqlLiteGenerator.Update(classMap);
        }
    }
}