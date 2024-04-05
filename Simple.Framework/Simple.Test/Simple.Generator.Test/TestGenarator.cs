using Simple.Dapper;
using Simple.Generator.Models;
using System.Dynamic;

namespace Simple.Generator.Test
{
    public class Tests
    {
        private EngineCore engine;

        [SetUp]
        public void Setup()
        {
            engine = new EngineCore();
        }

        [Test]
        public async Task TestGenarator()
        {
            dynamic viewBage = new ExpandoObject();
            viewBage.Config = new Config { NameSpase = "Test" };

            var table = new DbTable
            {
                TableName = "Test1",
                ClassName = "Test",
                Description = "Test≤‚ ‘",
                Columns = new List<DbColumn>
                {
                    new DbColumn{CodeType = "string",ClassPropName = "Id"},
                    new DbColumn{CodeType = "string",ClassPropName = "Name"},
                    new DbColumn{CodeType = "string",ClassPropName = "Age"},
                }
            };

            viewBage.Table = table;
            var data = await engine.GenerateOutput("…˙≥… Ù–‘", viewBage);
            Assert.That(data != "");
        }
    }
}