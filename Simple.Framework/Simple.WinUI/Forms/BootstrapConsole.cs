using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simple.WinUI.Controls.SplitContainer;
using Simple.WinUI.Forms;

namespace Simple.WinUI.Forms
{
    public partial class BootstrapConsole : BaseForm
    {
        /// <summary>当前打开的窗体</summary>
        private Form CurrentForm;

        /// <summary>已经加载的窗体</summary>
        private Dictionary<string, Form> LoadForms = new();

        /// <summary>已经加载的窗体菜单</summary>
        private Dictionary<string, ToolStripItemCollection> LoadMenuScriptItems = new();

        /// <summary>添加菜单树</summary>
        /// <param name="treeNode"></param>
        public BootstrapConsole(TreeNode[] treeNodes = null)
        {
            InitializeComponent();
            if (treeNodes != null)
            {
                tvMenu.Nodes.AddRange(treeNodes);
            }
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void tvMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var currentMenu = e.Node;
            var tagForm = currentMenu.Tag?.ToString();
            if (string.IsNullOrEmpty(tagForm)) return;

            AddWindow(tagForm);
        }

        private void SetTitle()
        {
            this.Text = $"Simple 工具箱 【{tvMenu.SelectedNode?.Text}】";
        }

        /// <summary>获取当前的菜单对应的窗体</summary>
        private Form? CurrentMenuForm(string windowsForm)
        {
            var tempFormTp = Assembly.GetEntryAssembly().GetTypes()
                .Where(p => p.Name == windowsForm).FirstOrDefault();
            if (tempFormTp is null) return null;

            return (Form)Activator.CreateInstance(tempFormTp, true);//根据类型创建实例
        }

        /// <summary>添加窗体</summary>
        private void AddWindow(string windowsForm)
        {
            LoadForms.TryGetValue(windowsForm, out Form? find);
            if (find != null)
            {
                find.Activate();
                find.Show();

                if (CurrentForm != null)
                {
                    CurrentForm.Hide();
                }

                CurrentForm = find;
                SetMenuScript(windowsForm);
                SetTitle();
                return;
            }

            var currentMenuForm = CurrentMenuForm(windowsForm);
            if (currentMenuForm == null) return;

            //新建窗口
            LoadForms[windowsForm] = currentMenuForm;
            LoadMenuScriptItems[windowsForm] = currentMenuForm.MainMenuStrip?.Items;
            currentMenuForm.MdiParent = this;
            currentMenuForm.FormBorderStyle = FormBorderStyle.None;
            currentMenuForm.WindowState = WindowState;
            currentMenuForm.ControlBox = false;
            currentMenuForm.Parent = uSplitContainer.Panel2;
            currentMenuForm.Dock = DockStyle.Fill;
            currentMenuForm.ShowInTaskbar = false;

            SetMenuScript(windowsForm, true);

            if (CurrentForm != null)
            {
                CurrentForm.Hide();
            }

            CurrentForm = currentMenuForm;
            currentMenuForm.Activate();
            SetTitle();
            currentMenuForm.Show();
        }

        /// <summary>设置当前激活的菜单</summary>
        /// <param name="currentMenuForm"></param>
        private void SetMenuScript(string windowsForm, bool isCreateForm = false)
        {
            var count = menuStrip.Items.Count;
            for (int i = count - 1; i > 0; i--)
            {
                var item = menuStrip.Items[i];
                item.Visible = (string)(item.Tag) == windowsForm;
            }

            if (isCreateForm)
            {
                var currentMenuScriptItem = LoadMenuScriptItems[windowsForm];
                if (currentMenuScriptItem == null) return;
                for (int i = 0; i < currentMenuScriptItem.Count; i++)
                {
                    var item = currentMenuScriptItem[i];
                    item.Tag = windowsForm;
                }
                menuStrip.Items.AddRange(currentMenuScriptItem);
            }
        }

        private void BootstrapConsole_SizeChanged(object sender, EventArgs e)
        {
            foreach (var item in LoadForms.Values)
            {
                item.WindowState = WindowState;
            }
        }

        private void toolsReload_Click(object sender, EventArgs e)
        {
            var tagForm = CurrentForm.Name;
            if (string.IsNullOrEmpty(tagForm)) return;

            LoadForms.Remove(tagForm);

            AddWindow(tagForm);
        }
    }
}