using RazorLight;
using Simple.Utils;
using System.Dynamic;

namespace Simple.Generator
{
    /// <summary>渲染引擎 使用 RazorLight 进行渲染 doc: https://github.com/toddams/RazorLight</summary>
    public class EngineCore
    {
        /// <summary>模板根目录</summary>
        private readonly string rootTemplatePath;

        private readonly RazorLightEngine engine;

        /// <summary>模板名称和渲染路径</summary>
        private Dictionary<string, string> fileTemplates = new();

        /// <summary>默认模板根目录</summary>
        /// <param name="rootTemplatePath"></param>
        public EngineCore()
        {
            this.rootTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
            engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(rootTemplatePath)
            .UseMemoryCachingProvider()
            .DisableEncoding()
            .Build();
            loadTemplate();
        }

        /// <summary>模板根目录</summary>
        /// <param name="rootTemplatePath"></param>
        public EngineCore(string rootTemplatePath)
        {
            if (rootTemplatePath.IsNullOrEmpty()) throw new Exception("模板根目录不能为空");

            this.rootTemplatePath = rootTemplatePath;
            engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(rootTemplatePath)
            .UseMemoryCachingProvider()
            .DisableEncoding()
            .Build();

            loadTemplate();
        }

        public void loadTemplate(string templateFold = "Default")
        {
            fileTemplates.Clear();
            var dir = Path.Combine(rootTemplatePath, templateFold);
            if (Directory.Exists(dir))
            {
                var ds = Directory.GetFiles(dir);
                if (ds != null && ds.Length > 0)
                {
                    foreach (var item in ds)
                    {
                        var fi = new FileInfo(item);
                        fileTemplates[fi.Name.Replace(fi.Extension, "")] = $"{templateFold}/{fi.Name}";
                    }
                }
            }
        }

        /// <summary>数据渲染</summary>
        /// <param name="model">数据</param>
        /// <param name="template">
        /// 模板相对根目录位置 engine.CompileRenderAsync("Subfolder/View.cshtml", model);
        /// </param>
        /// <returns></returns>
        public async Task<string> GenerateOutput(string templatekey, ExpandoObject model, ExpandoObject viewBage = null)
        {
            try
            {
                return await engine.CompileRenderAsync(fileTemplates[templatekey], model, viewBage);
            }
            catch (Exception ex)
            {
                LogHelper.Error("模板渲染出现异常", ex);
                return "";
            }
        }

        /// <summary>数据渲染</summary>
        /// <param name="model">数据</param>
        /// <param name="template">
        /// 模板相对根目录位置 engine.CompileRenderAsync("Subfolder/View.cshtml", model);
        /// </param>
        /// <returns></returns>
        public async Task<string> GenerateOutput<T>(string templatekey, T model, ExpandoObject viewBage = null)
        {
            try
            {
                return await engine.CompileRenderAsync(fileTemplates[templatekey], model, viewBage);
            }
            catch (Exception ex)
            {
                LogHelper.Error("模板渲染出现异常", ex);
                return "";
            }
        }
    }
}