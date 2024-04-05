using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Dapper.Test
{
    public partial class DbComponentTest
    {
        [Test]
        public void GetTable()
        {
            var table = MssqlDb.GetDbtables();

            Assert.That(table != null);

            foreach (var item in table)
            {
                var column = MssqlDb.GetDbColumns(item.TableName);
            }
        }
    }
}
