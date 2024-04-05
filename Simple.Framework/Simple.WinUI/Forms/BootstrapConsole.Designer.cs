namespace Simple.WinUI.Forms
{
    partial class BootstrapConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BootstrapConsole));
            menuStrip = new MenuStrip();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            toolsReload = new ToolStripMenuItem();
            tvMenu = new Controls.TreeView.UTreeView();
            uSplitContainer = new Controls.SplitContainer.USplitContainer();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uSplitContainer).BeginInit();
            uSplitContainer.Panel1.SuspendLayout();
            uSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { 关于ToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(890, 25);
            menuStrip.TabIndex = 5;
            menuStrip.Text = "menuStrip1";
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolsReload });
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(28, 21);
            关于ToolStripMenuItem.Text = "V";
            // 
            // toolsReload
            // 
            toolsReload.Name = "toolsReload";
            toolsReload.Size = new Size(124, 22);
            toolsReload.Text = "重载窗口";
            toolsReload.Click += toolsReload_Click;
            // 
            // tvMenu
            // 
            tvMenu.BackColor = Color.Transparent;
            tvMenu.BorderStyle = BorderStyle.None;
            tvMenu.Dock = DockStyle.Fill;
            tvMenu.ExpandIcon = null;
            tvMenu.ItemHeight = 30;
            tvMenu.Location = new Point(0, 0);
            tvMenu.Name = "tvMenu";
            tvMenu.ShrinkIcon = null;
            tvMenu.Size = new Size(161, 425);
            tvMenu.TabIndex = 9;
            tvMenu.NodeMouseClick += tvMenu_NodeMouseClick;
            // 
            // uSplitContainer
            // 
            uSplitContainer.ArrowColor = Color.FromArgb(80, 136, 228);
            uSplitContainer.BorderColor = Color.Silver;
            uSplitContainer.CollapsePanel = WinUI.Controls.CollapsePanel.PanelOne;
            uSplitContainer.Dock = DockStyle.Fill;
            uSplitContainer.FixedPanel = FixedPanel.Panel1;
            uSplitContainer.Location = new Point(0, 25);
            uSplitContainer.Name = "uSplitContainer";
            // 
            // uSplitContainer.Panel1
            // 
            uSplitContainer.Panel1.Controls.Add(tvMenu);
            uSplitContainer.Panel1MinSize = 50;
            uSplitContainer.Size = new Size(890, 425);
            uSplitContainer.SplitColor = Color.White;
            uSplitContainer.SplitterDistance = 161;
            uSplitContainer.SplitterWidth = 10;
            uSplitContainer.TabIndex = 11;
            // 
            // BootstrapConsole
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderEnabled = false;
            BorderWidth = 0;
            ClientSize = new Size(890, 450);
            Controls.Add(uSplitContainer);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "BootstrapConsole";
            Text = "Simple启动器";
            SizeChanged += BootstrapConsole_SizeChanged;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            uSplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uSplitContainer).EndInit();
            uSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip;
        private ToolStripMenuItem 关于ToolStripMenuItem;
        private WinUI.Controls.TreeView.UTreeView tvMenu;
        private WinUI.Controls.SplitContainer.USplitContainer uSplitContainer;
        private ToolStripMenuItem toolsReload;
    }
}