using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Simple.WinUI.Controls.TreeView.ProperityGrid
{
    #region 树形节点样式编辑属性，弹出属性编辑框

    /// <summary>WWW.CSharpSkin.COM 树形节点样式编辑属性，弹出属性编辑框</summary>
    public class TreeViewNodeStateStylePropertyEditor : UITypeEditor
    {
        #region 指定为模式窗体属性编辑器类型

        /// <summary>指定为模式窗体属性编辑器类型</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            //指定为模式窗体属性编辑器类型
            return UITypeEditorEditStyle.Modal;
        }

        #endregion 指定为模式窗体属性编辑器类型

        #region 打开属性编辑器修改数据

        /// <summary>打开属性编辑器修改数据</summary>
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
                    IDesignerHost host = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
                    if (host == null) return value;
                    UTreeView csharpTreeView = context.Instance as UTreeView;
                    TreeViewNodeStateStyleEditor treeViewNodeStateStyleEditor = new TreeViewNodeStateStyleEditor((TreeViewNodeStateStyle)value);
                    svc.ShowDialog(treeViewNodeStateStyleEditor);//打开属性编辑窗体
                    treeViewNodeStateStyleEditor.Dispose();//释放内存
                    //重新序列化内容到.Designer.cs文件
                    context.PropertyDescriptor.SetValue(context.Instance, treeViewNodeStateStyleEditor.TreeViewNodeStateStyle);
                    return treeViewNodeStateStyleEditor.TreeViewNodeStateStyle;
                }
            }
            return value;
            //打开属性编辑器修改数据
            //TreeViewNodeStateStyleEditor treeViewNodeStateStyleEditor = new TreeViewNodeStateStyleEditor((TreeViewNodeStateStyle)value);
            //if (treeViewNodeStateStyleEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    return treeViewNodeStateStyleEditor.TreeViewNodeStateStyle;
            //}
            //else
            //{
            //    return value;
            //}
        }

        #endregion 打开属性编辑器修改数据
    }

    #endregion 树形节点样式编辑属性，弹出属性编辑框
}