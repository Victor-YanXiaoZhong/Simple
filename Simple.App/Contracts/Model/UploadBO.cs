namespace Simple.Contracts.Model
{
    /// <summary>文件上传配置</summary>
    public class UploadConfig
    {
        /// <summary>服务器访问路径</summary>
        public string ServerUrl { get; set; }

        /// <summary>
        ///存储文件夹
        /// </summary>
        public string SaveFolder { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Upload");

        /// <summary>保存文件的前缀，默认为年月日</summary>
        public string FileFolderPrefix { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>单文件最大值 bit</summary>
        public int MaxSizeLimit { get; set; } = 1024 * 1024 * 1024;

        /// <summary>文件后缀 逗号连接 jpg,jpeg,png,exe</summary>
        public string AllowExtensions { get; set; }

        /// <summary>存在文件时，是否删除</summary>
        public bool DeleteIfExit { get; set; } = true;
    }

    /// <summary>上传成功的返回参数</summary>
    public class UploadSuccessResponse
    {
        /// <summary>文件访问全路径</summary>
        public string FileUrl { get; set; }

        /// <summary>文件名称</summary>
        public string FileName { get; set; }

        /// <summary>服务器存储路径</summary>
        public string FilePath { get; set; }

        /// <summary>文件大小</summary>
        public int FileSize { get; set; }
    }
}