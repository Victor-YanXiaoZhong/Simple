using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simple.Utils
{
    /// <summary>
    /// Xml帮助类
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// 将实体转换成xml字符串
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="t">T.</param>
        public static string ToXml<T>(T t) where T : class, new()
        {
            t = t ?? new T();
            XElement xele = new XElement(
                "xml",
                 t.GetType().GetProperties().Where(x => x.GetValue(t) != null).Select(x =>
                         new XElement(x.Name, x.GetValue(t)))
                );
            return xele.ToString();
        }
    }
}
