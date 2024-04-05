using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Tool.Models
{
    internal class TemplateSetting
    {
        [Category("模板属性"), Description("配置后按照此配置生成")]
        public bool 是否已配置 { get; set; } = false;

        [Category("模板属性")]
        public string TemplateName { get; set; } = "";

        [Category("模板属性"), Description("命名空间")]
        public string NameSpace { get; set; } = "UnsetNameSapace";

        [Category("模板属性"), Description("存储绝对路径文件夹名称")]
        public string SavePath { get; set; } = "";

        [Category("模板属性"), Description("存储绝对路径后是否自动加上表对应的类名作为文件夹")]
        public bool AutoPath { get; set; } = false;
    }
}