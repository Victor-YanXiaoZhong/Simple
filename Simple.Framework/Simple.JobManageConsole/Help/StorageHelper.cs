using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.JobManageConsole.Help
{
    internal class StorageHelper<T> where T : class, new()
    {
        public StorageHelper(string fileName)
        {
            this.FileName = fileName;
            if (!File.Exists(dbFile))
            {
                File.Create(dbFile);
            }
            GetList();
        }

        private string dbFile
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName + ".db");
            }
        }

        private string FileName { get; set; }

        public T Data { get; set; } = new T();

        private T GetList()
        {
            var tmp = File.ReadAllText(dbFile, Encoding.Unicode);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                return Data;
            }
            else
            {
                Data = JsonConvert.DeserializeObject<T>(tmp);
            }
            return Data;
        }

        /// <summary>持久化存储</summary>
        public void Save()
        {
            if (Data == null) return;
            var tmp = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(dbFile, tmp, Encoding.Unicode);
        }
    }
}