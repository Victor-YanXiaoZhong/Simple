using Microsoft.AspNetCore.Http;
using Simple.Contracts.Model;

namespace Simple.CommonApplication
{
    [Singleton]
    public class UploadService : IUploadService
    {
        public UploadService()
        {
            config = new UploadConfig();
        }

        public UploadService(UploadConfig config)
        {
            this.config = config;
        }

        private UploadConfig config { get; set; }

        public IUploadService SetConfig(UploadConfig config)
        {
            this.config = config;
            return this;
        }

        /// <summary>异步上传文件</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<UploadSuccessResponse> FileUploadAsync(IFormFile file)
        {
            var result = CheckFile(file);
            if (!result.Item1)
            {
                throw new CustomException(result.Item2, "检查上传文件异常");
            }
            var filePath = GetFilePath(file);

            var response = new UploadSuccessResponse();
            response.FilePath = filePath.Replace(config.SaveFolder, "");
            response.FileSize = Convert.ToInt32(Math.Round(Convert.ToDecimal(file.Length) / 1024M));
            if (File.Exists(filePath) && config.DeleteIfExit)
            {
                File.Delete(filePath);
            }

            if (!Directory.Exists(GetFileDir()))
            {
                Directory.CreateDirectory(GetFileDir());
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return response;
        }

        /// <summary>异步上传文件</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<UploadSuccessResponse>> FileUploadAsync(IFormFileCollection files)
        {
            var uploadResult = new List<UploadSuccessResponse>();
            foreach (var file in files)
            {
                var result = await FileUploadAsync(file);
                uploadResult.Add(result);
            }
            return uploadResult;
        }

        #region 一些辅助方法

        /// <summary>文件检查</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private (bool, string) CheckFile(IFormFile file)
        {
            if (file.Length > config.MaxSizeLimit)
            {
                return (false, $"文件超过允许上传的上限值[{config.MaxSizeLimit}]");
            }
            return (true, $"文件检查正常"); ;
        }

        /// <summary>获取文件存储路径</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFilePath(IFormFile file)
        {
            if (config.SaveFolder == null)
                config.SaveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Upload");
            return Path.Combine(config.SaveFolder, config.FileFolderPrefix, file.FileName);
        }

        /// <summary>获取文件存储路径</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetFileDir()
        {
            return Path.Combine(config.SaveFolder, config.FileFolderPrefix);
        }

        #endregion 一些辅助方法
    }
}