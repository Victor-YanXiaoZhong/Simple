using Simple.Utils.Models.Entity;

namespace Simple.Dapper
{
    /// <summary>数据库表信息</summary>
    public class DbTable : DefaultEntityInt
    {
        public string ClassName { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public bool IsLock { get; set; } = false;
        public int Sort { get; set; } = 0;

        public virtual List<DbColumn> Columns { get; set; }
    }

    /// <summary>数据表字段信息</summary>
    public class DbColumn : DefaultEntityInt
    {
        public int TableId { get; set; } = 0;
        public string ClassPropName { get; set; }
        public string DbColumnName { get; set; }
        public bool Required { get; set; } = false;
        public bool IsIdentity { get; set; } = false;
        public bool IsPrimaryKey { get; set; } = false;
        public bool IsHide { get; set; } = false;
        public string Description { get; set; }
        public string DbType { get; set; }
        public string CodeType { get; set; }
        public string DefaultValue { get; set; }
        public int Sort { get; set; } = 0;
    }
}