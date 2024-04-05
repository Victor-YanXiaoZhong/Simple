using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Simple.Utils.Helper
{
    /// <summary>文件班助类</summary>
    public class FileHelper
    {
        /// <summary>字节数组转换为Hex字符串</summary>
        /// <param name="data"></param>
        /// <param name="toLowerCase"></param>
        /// <returns></returns>
        private static string ByteArrayToHexString(byte[] data, bool toLowerCase = true)
        {
            var hex = BitConverter.ToString(data).Replace("-", string.Empty);
            return toLowerCase ? hex.ToLower() : hex.ToUpper();
        }

        /// <summary>文件路径</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string ToFilePath(string path)
        {
            return string.Join(Path.DirectorySeparatorChar.ToString(), path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        /// <summary>读文件</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            path = ToFilePath(path);
            if (!File.Exists(path))
                return "";
            using (StreamReader stream = new StreamReader(path))
            {
                return stream.ReadToEnd(); // 读取文件
            }
        }

        /// <summary>读文件流</summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream ReadFileStream(string path)
        {
            path = ToFilePath(path);

            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(path))
            {
                fileStream = new FileStream(path, FileMode.Open);
            }
            return fileStream;
        }

        /// <summary>写文件</summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">文件内容</param>
        /// <param name="appendToLast">是否追加到最后</param>
        public static void WriteFile(string fileName, string content, bool appendToLast = false)
        {
            using (FileStream stream = File.OpenWrite(fileName))
            {
                byte[] by = Encoding.UTF8.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }

        /// <summary>写文件</summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="content">文件内容</param>
        /// <param name="appendToLast">是否追加到最后</param>
        public static void WriteFile(string path, string fileName, string content, bool appendToLast = false)
        {
            if (!path.EndsWith("\\"))
            {
                path = path + "\\";
            }
            path = ToFilePath(path);
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream stream = File.Open(path + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = Encoding.UTF8.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }

        /// <summary>删除指定目录下的所有文件及文件夹(保留目录)</summary>
        /// <param name="file">文件目录</param>
        public static void DeleteDirectory(string file)
        {
            try
            {
                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    DirectoryInfo fileInfo = new DirectoryInfo(file);
                    //去除文件夹的只读属性
                    fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //去除文件的只读属性
                            File.SetAttributes(file, FileAttributes.Normal);
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDirectory(f);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(file);
                }
            }
            catch (Exception ex) // 异常处理
            {
                throw ex;
            }
        }

        /// <summary>获取文件MD5 哈希值</summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ComputeMD5(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }

        /// <summary>获取文件MD5 哈希值</summary>
        /// <param name="bytedata"></param>
        /// <returns></returns>
        public static string ComputeMD5(byte[] bytedata)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(bytedata);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail,error:" + ex.Message);
            }
        }

        /// <summary>文件SHA256哈希字符串</summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha256Hex(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                var hash = sha256.ComputeHash(bytes);
                return ByteArrayToHexString(hash);
            }
        }

        /// <summary>SHA256 转换为 Hex字符串</summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Sha256Hex(byte[] bytes)
        {
            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                var hash = sha256.ComputeHash(bytes);
                return ByteArrayToHexString(hash);
            }
        }

        #region 复制大文件

        public static void CopyBigFile(string originalFilePath, string destFilePath)
        {
            // 定义读文件流
            using (FileStream fsr = new FileStream(originalFilePath, FileMode.Open))
            {
                // 定义写文件流
                using (FileStream fsw = new FileStream(destFilePath, FileMode.OpenOrCreate))
                {
                    // 申请1M内存空间
                    byte[] buffer = new byte[1024 * 1024];
                    // 无限循环中反复读写，直到读完写完
                    while (true)
                    {
                        int readCount = fsr.Read(buffer, 0, buffer.Length);
                        fsw.Write(buffer, 0, readCount);
                        if (readCount < buffer.Length)
                        {
                            break;
                        }
                    }
                }
            }
        }

        #endregion 复制大文件

        #region 移动大文件

        public static void MoveBigFile(string originalFilePath, string destFilePath)
        {
            try
            {
                CopyBigFile(originalFilePath, destFilePath);
                File.Delete(originalFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion 移动大文件

        #region 复制文件夹

        /// <summary>复制文件夹及文件 (不包括源文件夹根目录名称, 只是复制其中内容到目标文件夹) https://www.cnblogs.com/wangjianhui008/p/3234519.html</summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static bool CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                // 如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                // 得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                // 得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion 复制文件夹

        /// <summary>压缩文件到Zip</summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string CreateZipFile(List<string> files)
        {
            var tmpFile = Path.GetTempFileName();

            // 使用ZipArchive创建zip文件
            using (var zipStream = new FileStream(tmpFile, FileMode.Create))
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    foreach (var file in files)
                    {
                        // 确保文件存在
                        if (File.Exists(file))
                        {
                            // 将文件添加到zip存档中
                            archive.CreateEntryFromFile(file, Path.GetFileName(file));
                        }
                    }
                }
            }
            return tmpFile;
        }

        /// <summary>压缩文件到Zip</summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string CreateZipDir(string dir)
        {
            var tmpFile = Path.GetTempFileName();
            File.Delete(tmpFile);
            ZipFile.CreateFromDirectory(dir, tmpFile, CompressionLevel.Optimal, includeBaseDirectory: false);
            return tmpFile;
        }
    }
}