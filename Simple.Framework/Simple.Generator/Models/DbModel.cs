namespace Simple.Generator.Models
{
    /// <summary>生成的项目配置信息</summary>
    public class Config
    {
        /// <summary>命名空间</summary>
        public string NameSpase { get; set; }

        /// <summary>生成文件存储目录</summary>
        public string FilePath { get; set; }

        /// <summary>文件后缀</summary>
        public string Suffix { get; set; }

        /// <summary>如果文件存在 是否覆盖</summary>
        public bool WillCovered { get; set; } = false;
    }
}