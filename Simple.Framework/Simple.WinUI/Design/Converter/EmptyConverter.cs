using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Design
{
    /// <summary>
    /// 展开属性选型去除描述
    /// </summary>
    [Description("展开属性选型去除描述")]
    public class EmptyConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// 当该属性为展开属性选型时，属性编辑器删除该属性的描述
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(string))
                return (object)String.Empty;
            return base.ConvertTo(context, culture, value, destinationType);

        }
    }
}
