namespace Simple.JobManageConsole
{
    partial class JobManageConsoleForm
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobManageConsoleForm));
            uSplitContainer1 = new WinUI.Controls.SplitContainer.USplitContainer();
            scheduleVOBindingSource = new BindingSource(components);
            uPanel1 = new WinUI.Controls.Panel.UPanel();
            btnEnable = new WinUI.Controls.Button.UButton();
            btnDisabled = new WinUI.Controls.Button.UButton();
            btnRunNow = new WinUI.Controls.Button.UButton();
            btnRefresh = new WinUI.Controls.Button.UButton();
            txt_JobMessage = new WinUI.Controls.TextBox.UTextBox();
            gdvSchedule = new DataGridView();
            nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            disabledDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)uSplitContainer1).BeginInit();
            uSplitContainer1.Panel1.SuspendLayout();
            uSplitContainer1.Panel2.SuspendLayout();
            uSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)scheduleVOBindingSource).BeginInit();
            uPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gdvSchedule).BeginInit();
            SuspendLayout();
            // 
            // uSplitContainer1
            // 
            uSplitContainer1.ArrowColor = Color.FromArgb(80, 136, 228);
            uSplitContainer1.BorderColor = Color.Silver;
            uSplitContainer1.CollapsePanel = WinUI.Controls.CollapsePanel.PanelOne;
            uSplitContainer1.Dock = DockStyle.Fill;
            uSplitContainer1.Location = new Point(0, 0);
            uSplitContainer1.Name = "uSplitContainer1";
            uSplitContainer1.Orientation = Orientation.Horizontal;
            // 
            // uSplitContainer1.Panel1
            // 
            uSplitContainer1.Panel1.Controls.Add(gdvSchedule);
            uSplitContainer1.Panel1.Controls.Add(uPanel1);
            uSplitContainer1.Panel1MinSize = 20;
            // 
            // uSplitContainer1.Panel2
            // 
            uSplitContainer1.Panel2.Controls.Add(txt_JobMessage);
            uSplitContainer1.Size = new Size(800, 450);
            uSplitContainer1.SplitColor = Color.Silver;
            uSplitContainer1.SplitterDistance = 261;
            uSplitContainer1.SplitterWidth = 10;
            uSplitContainer1.TabIndex = 0;
            // 
            // scheduleVOBindingSource
            // 
            scheduleVOBindingSource.DataSource = typeof(ScheduleVO);
            // 
            // uPanel1
            // 
            uPanel1.BackColor = Color.Transparent;
            uPanel1.BorderColor = Color.Silver;
            uPanel1.Controls.Add(btnEnable);
            uPanel1.Controls.Add(btnDisabled);
            uPanel1.Controls.Add(btnRunNow);
            uPanel1.Controls.Add(btnRefresh);
            uPanel1.Dock = DockStyle.Top;
            uPanel1.Location = new Point(0, 0);
            uPanel1.Name = "uPanel1";
            uPanel1.Radius = 0;
            uPanel1.Size = new Size(800, 47);
            uPanel1.TabIndex = 0;
            // 
            // btnEnable
            // 
            btnEnable.BackColor = SystemColors.Control;
            btnEnable.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnEnable.ForeColor = Color.FromArgb(64, 64, 64);
            btnEnable.HoverColor = Color.LightGray;
            btnEnable.ImageWidth = 18;
            btnEnable.Location = new Point(258, 9);
            btnEnable.Name = "btnEnable";
            btnEnable.PressedColor = Color.DarkGray;
            btnEnable.Radius = 8;
            btnEnable.RoundStyle = WinUI.RoundStyle.All;
            btnEnable.Size = new Size(75, 29);
            btnEnable.TabIndex = 3;
            btnEnable.Text = "启用";
            btnEnable.UseGradientMode = false;
            btnEnable.UseVisualStyleBackColor = false;
            btnEnable.Click += btnEnable_Click;
            // 
            // btnDisabled
            // 
            btnDisabled.BackColor = SystemColors.Control;
            btnDisabled.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnDisabled.ForeColor = Color.Red;
            btnDisabled.HoverColor = Color.LightGray;
            btnDisabled.ImageWidth = 18;
            btnDisabled.Location = new Point(177, 9);
            btnDisabled.Name = "btnDisabled";
            btnDisabled.PressedColor = Color.DarkGray;
            btnDisabled.Radius = 8;
            btnDisabled.RoundStyle = WinUI.RoundStyle.All;
            btnDisabled.Size = new Size(75, 29);
            btnDisabled.TabIndex = 2;
            btnDisabled.Text = "停用";
            btnDisabled.UseGradientMode = false;
            btnDisabled.UseVisualStyleBackColor = false;
            btnDisabled.Click += btnDisabled_Click;
            // 
            // btnRunNow
            // 
            btnRunNow.BackColor = Color.WhiteSmoke;
            btnRunNow.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnRunNow.ForeColor = Color.FromArgb(0, 192, 0);
            btnRunNow.HoverColor = Color.LightGray;
            btnRunNow.ImageWidth = 18;
            btnRunNow.Location = new Point(96, 9);
            btnRunNow.Name = "btnRunNow";
            btnRunNow.PressedColor = Color.DarkGray;
            btnRunNow.Radius = 8;
            btnRunNow.RoundStyle = WinUI.RoundStyle.All;
            btnRunNow.Size = new Size(75, 29);
            btnRunNow.TabIndex = 1;
            btnRunNow.Text = "立即执行";
            btnRunNow.UseGradientMode = false;
            btnRunNow.UseVisualStyleBackColor = false;
            btnRunNow.Click += btnRunNow_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = SystemColors.Control;
            btnRefresh.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnRefresh.ForeColor = Color.DodgerBlue;
            btnRefresh.HoverColor = Color.LightGray;
            btnRefresh.ImageWidth = 18;
            btnRefresh.Location = new Point(15, 9);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.PressedColor = Color.DarkGray;
            btnRefresh.Radius = 8;
            btnRefresh.RoundStyle = WinUI.RoundStyle.All;
            btnRefresh.Size = new Size(75, 29);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "刷新";
            btnRefresh.UseGradientMode = false;
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // txt_JobMessage
            // 
            txt_JobMessage.BorderColor = Color.FromArgb(0, 0, 0);
            txt_JobMessage.BorderWidth = 1;
            txt_JobMessage.Dock = DockStyle.Fill;
            txt_JobMessage.HotColor = Color.FromArgb(0, 0, 0);
            txt_JobMessage.Location = new Point(0, 0);
            txt_JobMessage.MaxLength = 100;
            txt_JobMessage.Multiline = true;
            txt_JobMessage.Name = "txt_JobMessage";
            txt_JobMessage.PlaceholderColor = Color.Empty;
            txt_JobMessage.PlaceholderText = null;
            txt_JobMessage.ScrollBars = ScrollBars.Vertical;
            txt_JobMessage.Size = new Size(800, 179);
            txt_JobMessage.TabIndex = 0;
            // 
            // gdvSchedule
            // 
            gdvSchedule.AllowUserToAddRows = false;
            gdvSchedule.AllowUserToDeleteRows = false;
            gdvSchedule.AutoGenerateColumns = false;
            gdvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gdvSchedule.BackgroundColor = SystemColors.ButtonFace;
            gdvSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gdvSchedule.Columns.AddRange(new DataGridViewColumn[] { nameDataGridViewTextBoxColumn, dataGridViewTextBoxColumn1, disabledDataGridViewCheckBoxColumn });
            gdvSchedule.DataSource = scheduleVOBindingSource;
            gdvSchedule.Dock = DockStyle.Fill;
            gdvSchedule.Location = new Point(0, 47);
            gdvSchedule.MultiSelect = false;
            gdvSchedule.Name = "gdvSchedule";
            gdvSchedule.ReadOnly = true;
            gdvSchedule.RowHeadersVisible = false;
            gdvSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gdvSchedule.Size = new Size(800, 214);
            gdvSchedule.TabIndex = 1;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.FillWeight = 50F;
            nameDataGridViewTextBoxColumn.HeaderText = "调度名称";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "NextRun";
            dataGridViewCellStyle2.Format = "yyyy-MM-dd HH:mm:ss";
            dataGridViewCellStyle2.NullValue = "\"\"";
            dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewTextBoxColumn1.FillWeight = 30F;
            dataGridViewTextBoxColumn1.HeaderText = "下次运行时间";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // disabledDataGridViewCheckBoxColumn
            // 
            disabledDataGridViewCheckBoxColumn.DataPropertyName = "Disabled";
            disabledDataGridViewCheckBoxColumn.FillWeight = 20F;
            disabledDataGridViewCheckBoxColumn.HeaderText = "是否禁用";
            disabledDataGridViewCheckBoxColumn.Name = "disabledDataGridViewCheckBoxColumn";
            disabledDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // JobManageConsoleForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(uSplitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "JobManageConsoleForm";
            Text = "调度控制台";
            FormClosing += JobManageConsoleForm_FormClosing;
            uSplitContainer1.Panel1.ResumeLayout(false);
            uSplitContainer1.Panel2.ResumeLayout(false);
            uSplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)uSplitContainer1).EndInit();
            uSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scheduleVOBindingSource).EndInit();
            uPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gdvSchedule).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private WinUI.Controls.SplitContainer.USplitContainer uSplitContainer1;
        private WinUI.Controls.Panel.UPanel uPanel1;
        private WinUI.Controls.TextBox.UTextBox txt_JobMessage;
        private WinUI.Controls.Button.UButton btnDisabled;
        private WinUI.Controls.Button.UButton btnRunNow;
        private WinUI.Controls.Button.UButton btnRefresh;
        private BindingSource scheduleVOBindingSource;
        private DataGridViewTextBoxColumn nextRunDataGridViewTextBoxColumn;
        private WinUI.Controls.Button.UButton btnEnable;
        private DataGridView gdvSchedule;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewCheckBoxColumn disabledDataGridViewCheckBoxColumn;
    }
}