using Simple.Utils.Models.Entity;

namespace Simple.Dapper.Test
{
    /// <summary>系统功能点 系统启动后，自动生成</summary>
    public class SysFunction : DefaultEntityInt
    {
        public SysFunction()
        {
        }

        /// <summary>功能点分组</summary>
        public string FunGroup { get; set; }

        /// <summary>功能点标识 controllername / actionname</summary>
        public string FunSign { get; set; }

        /// <summary>功能名称</summary>
        public string Name { get; set; }

        public bool Enabled { get; set; } = true;
    }
}