using Simple.Utils.Helper;

namespace Simple.Dapper
{
    public partial class DbComponent
    {
        public static readonly Dictionary<string, string> dbTypes = new Dictionary<string, string>
        {
             {  "bigint","long"},
             {  "bit","bool"},
             {  "int",  "int"  },
             {  "smallint",  "short"  },
             { "tinyint",  "byte"  },
             { "float",  "float"  },
             { "real",  "float"  },
             { "decimal",  "decimal"  },
             { "numeric",  "decimal"  },
             { "char",  "char"  },
             { "varchar",  "string"  },
             { "nchar",  "char"  },
             { "nvarchar",  "string"  },
             { "date",  "DateTime"  },
             { "time",  "TimeSpan"  },
             { "datetime",  "DateTime"  },
             { "datetime2",  "DateTime"  },
             { "smalldatetime",  "DateTime"  },
             { "timestamp",  "DateTimeOffset"  },
             { "rowversion",  "byte[]"  },
             { "tinyblob",  "byte[]"  },
             { "blob",  "byte[]"  },
             { "mediumblob",  "byte[]"  },
             { "longblob",  "byte[]"  },
             { "image",  "byte[]"  },
             { "text",  "string"  },
             { "ntext",  "string"  },
             { "xml",  "XElement"  }
        };

        /// <summary>获取where语句及参数</summary>
        /// <param name="whereObj">new {Id=1,Name = "张三"}</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected string GetWhere(object whereObj, out DynamicParameters parameters)
        {
            parameters = new DynamicParameters();
            var whereDict = ReflectionHelper.GetDictionary(whereObj);
            var whereFields = new List<string>();
            foreach (var column in whereDict)
            {
                whereFields.Add($"{column.Key}={sqlGenerator.ParameterPrefix}{column.Key}");
                parameters.Add($"{sqlGenerator.ParameterPrefix}{column.Key}", column.Value);
            }
            return string.Join(",", whereFields);
        }
    }
}