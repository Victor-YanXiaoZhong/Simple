using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;

namespace Simple.WinUI.Controls.ToolTip.ProperityGrid
{
    #region CSharpToolTip颜色样式设计编辑属性，弹出属性编辑框

    /// <summary>WWW.CSharpSkin.COM CSharpToolTip颜色样式设计编辑属性，弹出属性编辑框</summary>
    public class ToolTipColorStylePropertyEditor : UITypeEditor
    {
        #region 指定为模式窗体属性编辑器类型

        /// <summary>指定为模式窗体属性编辑器类型</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion 指定为模式窗体属性编辑器类型

        #region 编辑属性值

        /// <summary>编辑属性值</summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (svc != null && context.Instance != null)
                {
                    //取出设计器所在的窗体(组件/控件所在窗体)
                    IDesignerHost host = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
                    if (host == null) return value;
                    //context.Instance:CSharpToolTip
                    UToolTip csharpToolTip = context.Instance as UToolTip;
                    ToolTipColorStyle toolTipColorStyle = csharpToolTip.ToolTipColorStyle;
                    if (toolTipColorStyle == null) toolTipColorStyle = new ToolTipColorStyle();
                    ToolTipColorStyleEditor toolTipColorStyleEditor = new ToolTipColorStyleEditor(toolTipColorStyle);
                    svc.ShowDialog(toolTipColorStyleEditor);//打开属性编辑窗体
                    toolTipColorStyleEditor.Dispose();//释放内存
                    //重新序列化内容到.Designer.cs文件
                    csharpToolTip.ToolTipColorStyle = toolTipColorStyleEditor.ToolTipColorStyle;
                    context.PropertyDescriptor.SetValue(context.Instance, csharpToolTip.ToolTipColorStyle);
                    return csharpToolTip.ToolTipColorStyle;//返回修改后的对象
                }
            }
            return value;
        }

        #endregion 编辑属性值
    }

    #endregion CSharpToolTip颜色样式设计编辑属性，弹出属性编辑框
}