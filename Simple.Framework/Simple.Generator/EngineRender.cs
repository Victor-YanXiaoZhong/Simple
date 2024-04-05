using Simple.Utils;
using Simple.Utils.Helper;
using System.Dynamic;

namespace Simple.Generator
{
    /// <summary>生成配置</summary>
    public class EngineRender
    {
        /// <summary>根目录</summary>
        public string TemplateRoot { get; set; }

        public string TemplateKey { get; set; }

        /// <summary>输出文件地址</summary>
        public string OutputFile { get; set; }

        /// <summary>数据载体</summary>
        public ExpandoObject ViewBag { get; set; }

        /// <summary>生成模板</summary>
        /// <returns></returns>
        public virtual async Task<string> RendTemplate(bool writeFile = false)
        {
            var engine = new EngineCore(TemplateRoot);
            var result = await engine.GenerateOutput(TemplateKey, ViewBag);
            if (result.IsNotEmpty() && writeFile)
            {
                if (File.Exists(OutputFile))
                {
                    File.Delete(OutputFile);
                }
                FileHelper.WriteFile(OutputFile, result);
            }
            return result;
        }
    }

    /// <summary>带固定数据类型的配置</summary>
    /// <typeparam name="T"></typeparam>
    public class EngineRender<T> : EngineRender where T : class
    {
        /// <summary>数据载体</summary>
        public T Model { get; set; }

        /// <summary>生成模板</summary>
        /// <returns></returns>
        public override async Task<string> RendTemplate(bool writeFile = false)
        {
            var engine = new EngineCore(TemplateRoot);
            var result = await engine.GenerateOutput(TemplateKey, Model, ViewBag);
            if (result.IsNotEmpty() && writeFile)
            {
                if (File.Exists(OutputFile))
                {
                    File.Delete(OutputFile);
                }
                FileHelper.WriteFile(OutputFile, result);
            }
            return result;
        }
    }
}