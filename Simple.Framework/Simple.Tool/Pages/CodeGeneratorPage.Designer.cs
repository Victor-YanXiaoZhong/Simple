namespace Simple.Tool.Pages
{
    partial class CodeGeneratorPage
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            uSplitContainer1 = new WinUI.Controls.SplitContainer.USplitContainer();
            gdv_tables = new WinUI.Controls.DataGridView.DataGridViewWithFilter();
            选中 = new DataGridViewCheckBoxColumn();
            表名 = new DataGridViewTextBoxColumn();
            uPanel1 = new WinUI.Controls.Panel.UPanel();
            menuStrip = new MenuStrip();
            测试ToolStripMenuItem = new ToolStripMenuItem();
            测试11ToolStripMenuItem = new ToolStripMenuItem();
            打开数据库ToolStripMenuItem = new ToolStripMenuItem();
            保存模型ToolStripMenuItem = new ToolStripMenuItem();
            编辑模板ToolStripMenuItem = new ToolStripMenuItem();
            导入模板ToolStripMenuItem = new ToolStripMenuItem();
            测试1ToolStripMenuItem = new ToolStripMenuItem();
            测试2ToolStripMenuItem = new ToolStripMenuItem();
            生成文件ToolStripMenuItem = new ToolStripMenuItem();
            uSplitContainer2 = new WinUI.Controls.SplitContainer.USplitContainer();
            cmbTemplateRoot = new WinUI.Controls.ComboBox.UComboBox();
            label5 = new Label();
            uTabControl = new WinUI.Controls.TableControl.UTabControl();
            uPanel2 = new WinUI.Controls.Panel.UPanel();
            pptg_template = new PropertyGrid();
            uPanel3 = new WinUI.Controls.Panel.UPanel();
            btnSaveConfig = new WinUI.Controls.Button.UButton();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)uSplitContainer1).BeginInit();
            uSplitContainer1.Panel1.SuspendLayout();
            uSplitContainer1.Panel2.SuspendLayout();
            uSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gdv_tables).BeginInit();
            uPanel1.SuspendLayout();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)uSplitContainer2).BeginInit();
            uSplitContainer2.Panel1.SuspendLayout();
            uSplitContainer2.Panel2.SuspendLayout();
            uSplitContainer2.SuspendLayout();
            uPanel2.SuspendLayout();
            uPanel3.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // uSplitContainer1
            // 
            uSplitContainer1.ArrowColor = Color.FromArgb(80, 136, 228);
            uSplitContainer1.BorderColor = Color.Silver;
            uSplitContainer1.CollapsePanel = WinUI.Controls.CollapsePanel.PanelOne;
            uSplitContainer1.Dock = DockStyle.Fill;
            uSplitContainer1.FixedPanel = FixedPanel.Panel1;
            uSplitContainer1.Location = new Point(0, 0);
            uSplitContainer1.Name = "uSplitContainer1";
            // 
            // uSplitContainer1.Panel1
            // 
            uSplitContainer1.Panel1.Controls.Add(gdv_tables);
            uSplitContainer1.Panel1.Controls.Add(uPanel1);
            // 
            // uSplitContainer1.Panel2
            // 
            uSplitContainer1.Panel2.Controls.Add(uSplitContainer2);
            uSplitContainer1.Size = new Size(726, 465);
            uSplitContainer1.SplitColor = Color.Silver;
            uSplitContainer1.SplitterDistance = 218;
            uSplitContainer1.SplitterWidth = 10;
            uSplitContainer1.TabIndex = 0;
            // 
            // gdv_tables
            // 
            gdv_tables.AllowUserToAddRows = false;
            gdv_tables.AllowUserToDeleteRows = false;
            gdv_tables.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gdv_tables.BackgroundColor = SystemColors.ButtonHighlight;
            gdv_tables.BorderStyle = BorderStyle.Fixed3D;
            gdv_tables.ColumnHeadersHeight = 26;
            gdv_tables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gdv_tables.Columns.AddRange(new DataGridViewColumn[] { 选中, 表名 });
            gdv_tables.Dock = DockStyle.Fill;
            gdv_tables.GridColor = SystemColors.ButtonFace;
            gdv_tables.Location = new Point(0, 144);
            gdv_tables.MultiSelect = false;
            gdv_tables.Name = "gdv_tables";
            gdv_tables.RowHeadersVisible = false;
            gdv_tables.RowHeadersWidth = 82;
            gdv_tables.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            gdv_tables.ScrollBars = ScrollBars.Horizontal;
            gdv_tables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gdv_tables.ShowFilter = false;
            gdv_tables.Size = new Size(218, 321);
            gdv_tables.TabIndex = 1;
            gdv_tables.DoubleClick += gdv_tables_DoubleClick;
            // 
            // 选中
            // 
            选中.FalseValue = "False";
            选中.IndeterminateValue = "False";
            选中.MinimumWidth = 10;
            选中.Name = "选中";
            选中.TrueValue = "True";
            选中.Width = 60;
            // 
            // 表名
            // 
            表名.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            表名.DataPropertyName = "TableName";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            表名.DefaultCellStyle = dataGridViewCellStyle1;
            表名.MinimumWidth = 10;
            表名.Name = "表名";
            表名.ReadOnly = true;
            表名.Resizable = DataGridViewTriState.True;
            表名.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // uPanel1
            // 
            uPanel1.BorderColor = Color.Silver;
            uPanel1.Controls.Add(menuStrip);
            uPanel1.Dock = DockStyle.Top;
            uPanel1.Location = new Point(0, 0);
            uPanel1.Name = "uPanel1";
            uPanel1.Radius = 0;
            uPanel1.Size = new Size(218, 144);
            uPanel1.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.Silver;
            menuStrip.Dock = DockStyle.None;
            menuStrip.GripStyle = ToolStripGripStyle.Visible;
            menuStrip.ImageScalingSize = new Size(32, 32);
            menuStrip.Items.AddRange(new ToolStripItem[] { 测试ToolStripMenuItem, 测试1ToolStripMenuItem });
            menuStrip.Location = new Point(36, 55);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(100, 25);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            menuStrip.Visible = false;
            // 
            // 测试ToolStripMenuItem
            // 
            测试ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 测试11ToolStripMenuItem, 打开数据库ToolStripMenuItem, 保存模型ToolStripMenuItem, 编辑模板ToolStripMenuItem, 导入模板ToolStripMenuItem });
            测试ToolStripMenuItem.Name = "测试ToolStripMenuItem";
            测试ToolStripMenuItem.Size = new Size(44, 21);
            测试ToolStripMenuItem.Text = "文件";
            // 
            // 测试11ToolStripMenuItem
            // 
            测试11ToolStripMenuItem.Name = "测试11ToolStripMenuItem";
            测试11ToolStripMenuItem.Size = new Size(136, 22);
            测试11ToolStripMenuItem.Text = "打开模型";
            // 
            // 打开数据库ToolStripMenuItem
            // 
            打开数据库ToolStripMenuItem.Name = "打开数据库ToolStripMenuItem";
            打开数据库ToolStripMenuItem.Size = new Size(136, 22);
            打开数据库ToolStripMenuItem.Text = "打开数据库";
            打开数据库ToolStripMenuItem.Click += OpenSlqDb_Click;
            // 
            // 保存模型ToolStripMenuItem
            // 
            保存模型ToolStripMenuItem.Name = "保存模型ToolStripMenuItem";
            保存模型ToolStripMenuItem.Size = new Size(136, 22);
            保存模型ToolStripMenuItem.Text = "保存模型";
            保存模型ToolStripMenuItem.Click += SaveModel_Click;
            // 
            // 编辑模板ToolStripMenuItem
            // 
            编辑模板ToolStripMenuItem.Name = "编辑模板ToolStripMenuItem";
            编辑模板ToolStripMenuItem.Size = new Size(136, 22);
            编辑模板ToolStripMenuItem.Text = "编辑模板";
            编辑模板ToolStripMenuItem.Click += EditTemplate_Click;
            // 
            // 导入模板ToolStripMenuItem
            // 
            导入模板ToolStripMenuItem.Name = "导入模板ToolStripMenuItem";
            导入模板ToolStripMenuItem.Size = new Size(136, 22);
            导入模板ToolStripMenuItem.Text = "导入模板";
            导入模板ToolStripMenuItem.Click += ImportTemplate_Click;
            // 
            // 测试1ToolStripMenuItem
            // 
            测试1ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 测试2ToolStripMenuItem, 生成文件ToolStripMenuItem });
            测试1ToolStripMenuItem.Name = "测试1ToolStripMenuItem";
            测试1ToolStripMenuItem.Size = new Size(44, 21);
            测试1ToolStripMenuItem.Text = "生成";
            // 
            // 测试2ToolStripMenuItem
            // 
            测试2ToolStripMenuItem.Name = "测试2ToolStripMenuItem";
            测试2ToolStripMenuItem.Size = new Size(148, 22);
            测试2ToolStripMenuItem.Text = "选中行表配置";
            测试2ToolStripMenuItem.Click += EditDbTable_Click;
            // 
            // 生成文件ToolStripMenuItem
            // 
            生成文件ToolStripMenuItem.Name = "生成文件ToolStripMenuItem";
            生成文件ToolStripMenuItem.Size = new Size(148, 22);
            生成文件ToolStripMenuItem.Text = "生成文件";
            生成文件ToolStripMenuItem.Click += GeneralFile_Click;
            // 
            // uSplitContainer2
            // 
            uSplitContainer2.ArrowColor = Color.FromArgb(80, 136, 228);
            uSplitContainer2.BorderColor = Color.Silver;
            uSplitContainer2.CollapsePanel = WinUI.Controls.CollapsePanel.PanelOne;
            uSplitContainer2.Dock = DockStyle.Fill;
            uSplitContainer2.FixedPanel = FixedPanel.Panel1;
            uSplitContainer2.Location = new Point(0, 0);
            uSplitContainer2.Name = "uSplitContainer2";
            uSplitContainer2.Orientation = Orientation.Horizontal;
            // 
            // uSplitContainer2.Panel1
            // 
            uSplitContainer2.Panel1.Controls.Add(cmbTemplateRoot);
            uSplitContainer2.Panel1.Controls.Add(label5);
            // 
            // uSplitContainer2.Panel2
            // 
            uSplitContainer2.Panel2.Controls.Add(uTabControl);
            uSplitContainer2.Panel2.Controls.Add(uPanel2);
            uSplitContainer2.Size = new Size(498, 465);
            uSplitContainer2.SplitColor = Color.Silver;
            uSplitContainer2.SplitterDistance = 41;
            uSplitContainer2.SplitterWidth = 10;
            uSplitContainer2.TabIndex = 0;
            // 
            // cmbTemplateRoot
            // 
            cmbTemplateRoot.ArrowColor = Color.White;
            cmbTemplateRoot.BaseColor = SystemColors.Control;
            cmbTemplateRoot.BorderColor = Color.Silver;
            cmbTemplateRoot.DisplayMember = "Text";
            cmbTemplateRoot.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTemplateRoot.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTemplateRoot.FormattingEnabled = true;
            cmbTemplateRoot.ItemHoverColor = Color.Silver;
            cmbTemplateRoot.Location = new Point(76, 10);
            cmbTemplateRoot.Name = "cmbTemplateRoot";
            cmbTemplateRoot.Size = new Size(152, 24);
            cmbTemplateRoot.TabIndex = 14;
            cmbTemplateRoot.ValueMember = "Value";
            cmbTemplateRoot.SelectedValueChanged += cmbTemplateRoot_SelectedValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 13);
            label5.Name = "label5";
            label5.Size = new Size(68, 17);
            label5.TabIndex = 13;
            label5.Text = "选择模板组";
            // 
            // uTabControl
            // 
            uTabControl.Alignment = TabAlignment.Bottom;
            uTabControl.Dock = DockStyle.Fill;
            uTabControl.Location = new Point(0, 0);
            uTabControl.Name = "uTabControl";
            uTabControl.SelectedIndex = 0;
            uTabControl.Size = new Size(280, 414);
            uTabControl.TabBackNormalColor = Color.FromArgb(224, 224, 224);
            uTabControl.TabBackSelectedColor = Color.DarkGray;
            uTabControl.TabCloseBackColor = Color.FromArgb(255, 255, 255);
            uTabControl.TabCloseSize = new Size(6, 6);
            uTabControl.TabIndex = 2;
            uTabControl.TabTextAlignment = StringAlignment.Center;
            uTabControl.TabTextNormalColor = Color.FromArgb(255, 255, 255);
            uTabControl.TabTextSelectedColor = Color.FromArgb(255, 255, 255);
            uTabControl.SelectedIndexChanged += uTabControl_SelectedIndexChanged;
            // 
            // uPanel2
            // 
            uPanel2.BorderColor = Color.Silver;
            uPanel2.Controls.Add(pptg_template);
            uPanel2.Controls.Add(uPanel3);
            uPanel2.Controls.Add(menuStrip1);
            uPanel2.Dock = DockStyle.Right;
            uPanel2.Location = new Point(280, 0);
            uPanel2.Name = "uPanel2";
            uPanel2.Radius = 0;
            uPanel2.Size = new Size(218, 414);
            uPanel2.TabIndex = 1;
            // 
            // pptg_template
            // 
            pptg_template.Dock = DockStyle.Fill;
            pptg_template.Location = new Point(0, 50);
            pptg_template.Name = "pptg_template";
            pptg_template.PropertySort = PropertySort.NoSort;
            pptg_template.Size = new Size(218, 364);
            pptg_template.TabIndex = 2;
            // 
            // uPanel3
            // 
            uPanel3.BorderColor = Color.Silver;
            uPanel3.Controls.Add(btnSaveConfig);
            uPanel3.Dock = DockStyle.Top;
            uPanel3.Location = new Point(0, 0);
            uPanel3.Name = "uPanel3";
            uPanel3.Radius = 0;
            uPanel3.Size = new Size(218, 50);
            uPanel3.TabIndex = 1;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.BackColor = SystemColors.ButtonFace;
            btnSaveConfig.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnSaveConfig.HoverColor = Color.LightGray;
            btnSaveConfig.ImageWidth = 18;
            btnSaveConfig.Location = new Point(7, 17);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.PressedColor = Color.DarkGray;
            btnSaveConfig.Radius = 8;
            btnSaveConfig.RoundStyle = WinUI.RoundStyle.All;
            btnSaveConfig.Size = new Size(119, 23);
            btnSaveConfig.TabIndex = 0;
            btnSaveConfig.Text = "保存当前模板配置";
            btnSaveConfig.UseGradientMode = false;
            btnSaveConfig.UseVisualStyleBackColor = false;
            btnSaveConfig.Click += btnSaveConfig_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.Silver;
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.GripStyle = ToolStripGripStyle.Visible;
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem7 });
            menuStrip1.Location = new Point(36, 55);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(100, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6 });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(44, 21);
            toolStripMenuItem1.Text = "文件";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(136, 22);
            toolStripMenuItem2.Text = "打开模型";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(136, 22);
            toolStripMenuItem3.Text = "打开数据库";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(136, 22);
            toolStripMenuItem4.Text = "保存模型";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(136, 22);
            toolStripMenuItem5.Text = "编辑模板";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(136, 22);
            toolStripMenuItem6.Text = "导入模板";
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem8, toolStripMenuItem9 });
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(44, 21);
            toolStripMenuItem7.Text = "生成";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(148, 22);
            toolStripMenuItem8.Text = "选中行表配置";
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(148, 22);
            toolStripMenuItem9.Text = "生成文件";
            // 
            // CodeGeneratorPage
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(726, 465);
            Controls.Add(uSplitContainer1);
            MainMenuStrip = menuStrip;
            Margin = new Padding(2, 3, 2, 3);
            Name = "CodeGeneratorPage";
            Text = "CodeGeneratorPage";
            uSplitContainer1.Panel1.ResumeLayout(false);
            uSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uSplitContainer1).EndInit();
            uSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gdv_tables).EndInit();
            uPanel1.ResumeLayout(false);
            uPanel1.PerformLayout();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            uSplitContainer2.Panel1.ResumeLayout(false);
            uSplitContainer2.Panel1.PerformLayout();
            uSplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)uSplitContainer2).EndInit();
            uSplitContainer2.ResumeLayout(false);
            uPanel2.ResumeLayout(false);
            uPanel2.PerformLayout();
            uPanel3.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private WinUI.Controls.SplitContainer.USplitContainer uSplitContainer1;
        private WinUI.Controls.DataGridView.DataGridViewWithFilter gdv_tables;
        private WinUI.Controls.SplitContainer.USplitContainer uSplitContainer2;
        private WinUI.Controls.Panel.UPanel uPanel1;
        private MenuStrip menuStrip;
        private ToolStripMenuItem 测试ToolStripMenuItem;
        private ToolStripMenuItem 测试11ToolStripMenuItem;
        private ToolStripMenuItem 测试1ToolStripMenuItem;
        private ToolStripMenuItem 测试2ToolStripMenuItem;
        private ToolStripMenuItem 打开数据库ToolStripMenuItem;
        private ToolStripMenuItem 生成文件ToolStripMenuItem;
        private DataGridViewCheckBoxColumn 选中;
        private DataGridViewTextBoxColumn 表名;
        private WinUI.Controls.ComboBox.UComboBox cmbTemplateRoot;
        private Label label5;
        private ToolStripMenuItem 保存模型ToolStripMenuItem;
        private ToolStripMenuItem 编辑模板ToolStripMenuItem;
        private ToolStripMenuItem 导入模板ToolStripMenuItem;
        private WinUI.Controls.Panel.UPanel uPanel2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private WinUI.Controls.TableControl.UTabControl uTabControl;
        private PropertyGrid pptg_template;
        private WinUI.Controls.Panel.UPanel uPanel3;
        private WinUI.Controls.Button.UButton btnSaveConfig;
    }
}