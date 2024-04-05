using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Simple.WinUI.Controls.TrackBar.ProperityGrid
{
    #region 滑动条样式设计编辑属性，弹出属性编辑框

    /// <summary>WWW.CSharpSkin.COM 滑动条样式设计编辑属性，弹出属性编辑框</summary>
    public class TrackBarColorStylePropertyEditor : UITypeEditor
    {
        #region 指定为模式窗体属性编辑器类型

        /// <summary>指定为模式窗体属性编辑器类型</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            //指定为模式窗体属性编辑器类型
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
                    //context.Instance:可以得到当前的CSharpTrackBar。
                    UTrackBar csharpTrackBar = context.Instance as UTrackBar;
                    TrackBarColorStyle trackBarColorStyle = csharpTrackBar.TrackBarColorStyle;
                    if (trackBarColorStyle == null) trackBarColorStyle = new TrackBarColorStyle();
                    TrackBarColorStyleEditor trackBarColorStyleEditor = new TrackBarColorStyleEditor(trackBarColorStyle);
                    svc.ShowDialog(trackBarColorStyleEditor);//打开属性编辑窗体
                    trackBarColorStyleEditor.Dispose();//释放内存
                    //重新序列化内容到.Designer.cs文件
                    context.PropertyDescriptor.SetValue(context.Instance, trackBarColorStyleEditor.TrackBarColorStyle);
                    return trackBarColorStyleEditor.TrackBarColorStyle;//返回修改后的对象
                }
            }
            return value;
            //打开属性编辑器修改数据
            //        TrackBarColorStyleEditor trackBarColorStyleEditor = new TrackBarColorStyleEditor((TrackBarColorStyle)value);
            //if (trackBarColorStyleEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    return trackBarColorStyleEditor.TrackBarColorStyle;
            //}
            //else
            //{
            //    return value;
            //}
        }

        #endregion 编辑属性值
    }

    #endregion 滑动条样式设计编辑属性，弹出属性编辑框
}