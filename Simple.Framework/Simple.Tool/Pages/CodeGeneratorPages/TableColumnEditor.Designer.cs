namespace Simple.Tool.Pages.CodeGeneratorPages
{
    partial class TableColumnEditor
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            uPanel1 = new WinUI.Controls.Panel.UPanel();
            label1 = new Label();
            txtDescription = new WinUI.Controls.TextBox.UTextBox();
            label4 = new Label();
            txtClassName = new WinUI.Controls.TextBox.UTextBox();
            label2 = new Label();
            txtTableName = new WinUI.Controls.TextBox.UTextBox();
            uPanel2 = new WinUI.Controls.Panel.UPanel();
            btnSave = new WinUI.Controls.Button.UButton();
            gdv_columns = new WinUI.Controls.DataGridView.DataGridViewWithFilter();
            colDbColumnName = new DataGridViewTextBoxColumn();
            colClassPropName = new DataGridViewTextBoxColumn();
            colRequired = new DataGridViewCheckBoxColumn();
            colIsIdentity = new DataGridViewCheckBoxColumn();
            colPrimaryKey = new DataGridViewCheckBoxColumn();
            colHide = new DataGridViewCheckBoxColumn();
            colDescription = new DataGridViewTextBoxColumn();
            colDbType = new DataGridViewTextBoxColumn();
            colCodeType = new DataGridViewTextBoxColumn();
            colSort = new DataGridViewTextBoxColumn();
            uPanel1.SuspendLayout();
            uPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gdv_columns).BeginInit();
            SuspendLayout();
            // 
            // uPanel1
            // 
            uPanel1.BorderColor = Color.Silver;
            uPanel1.Controls.Add(label1);
            uPanel1.Controls.Add(txtDescription);
            uPanel1.Controls.Add(label4);
            uPanel1.Controls.Add(txtClassName);
            uPanel1.Controls.Add(label2);
            uPanel1.Controls.Add(txtTableName);
            uPanel1.Dock = DockStyle.Top;
            uPanel1.Location = new Point(0, 0);
            uPanel1.Name = "uPanel1";
            uPanel1.Radius = 0;
            uPanel1.Size = new Size(800, 84);
            uPanel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 49);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 13;
            label1.Text = "描述";
            // 
            // txtDescription
            // 
            txtDescription.BorderColor = Color.FromArgb(0, 0, 0);
            txtDescription.BorderWidth = 1;
            txtDescription.HotColor = Color.FromArgb(0, 0, 0);
            txtDescription.Location = new Point(67, 46);
            txtDescription.Name = "txtDescription";
            txtDescription.PlaceholderColor = Color.Empty;
            txtDescription.PlaceholderText = null;
            txtDescription.Size = new Size(425, 23);
            txtDescription.TabIndex = 12;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(259, 20);
            label4.Name = "label4";
            label4.Size = new Size(44, 17);
            label4.TabIndex = 11;
            label4.Text = "类名称";
            // 
            // txtClassName
            // 
            txtClassName.BorderColor = Color.FromArgb(0, 0, 0);
            txtClassName.BorderWidth = 1;
            txtClassName.HotColor = Color.FromArgb(0, 0, 0);
            txtClassName.Location = new Point(309, 17);
            txtClassName.Name = "txtClassName";
            txtClassName.PlaceholderColor = Color.Empty;
            txtClassName.PlaceholderText = null;
            txtClassName.Size = new Size(183, 23);
            txtClassName.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 20);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 5;
            label2.Text = "表名称";
            // 
            // txtTableName
            // 
            txtTableName.BorderColor = Color.FromArgb(0, 0, 0);
            txtTableName.BorderWidth = 1;
            txtTableName.HotColor = Color.FromArgb(0, 0, 0);
            txtTableName.Location = new Point(67, 17);
            txtTableName.Name = "txtTableName";
            txtTableName.PlaceholderColor = Color.Empty;
            txtTableName.PlaceholderText = null;
            txtTableName.Size = new Size(183, 23);
            txtTableName.TabIndex = 4;
            // 
            // uPanel2
            // 
            uPanel2.BorderColor = Color.Silver;
            uPanel2.Controls.Add(btnSave);
            uPanel2.Dock = DockStyle.Top;
            uPanel2.Location = new Point(0, 84);
            uPanel2.Name = "uPanel2";
            uPanel2.Radius = 0;
            uPanel2.Size = new Size(800, 32);
            uPanel2.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.BackColor = SystemColors.Control;
            btnSave.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnSave.HoverColor = Color.LightGray;
            btnSave.ImageWidth = 18;
            btnSave.Location = new Point(3, 6);
            btnSave.Name = "btnSave";
            btnSave.PressedColor = Color.DarkGray;
            btnSave.Radius = 8;
            btnSave.RoundStyle = WinUI.RoundStyle.All;
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 3;
            btnSave.Text = "保存";
            btnSave.UseGradientMode = false;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // gdv_columns
            // 
            gdv_columns.AllowUserToAddRows = false;
            gdv_columns.AllowUserToDeleteRows = false;
            gdv_columns.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gdv_columns.BackgroundColor = SystemColors.ButtonHighlight;
            gdv_columns.BorderStyle = BorderStyle.Fixed3D;
            gdv_columns.ColumnHeadersHeight = 26;
            gdv_columns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gdv_columns.Columns.AddRange(new DataGridViewColumn[] { colDbColumnName, colClassPropName, colRequired, colIsIdentity, colPrimaryKey, colHide, colDescription, colDbType, colCodeType, colSort });
            gdv_columns.Dock = DockStyle.Fill;
            gdv_columns.GridColor = SystemColors.ButtonFace;
            gdv_columns.Location = new Point(0, 116);
            gdv_columns.MultiSelect = false;
            gdv_columns.Name = "gdv_columns";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gdv_columns.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gdv_columns.RowHeadersVisible = false;
            gdv_columns.RowHeadersWidth = 82;
            gdv_columns.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            gdv_columns.ScrollBars = ScrollBars.Horizontal;
            gdv_columns.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gdv_columns.ShowFilter = false;
            gdv_columns.Size = new Size(800, 334);
            gdv_columns.TabIndex = 3;
            // 
            // colDbColumnName
            // 
            colDbColumnName.DataPropertyName = "DbColumnName";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colDbColumnName.DefaultCellStyle = dataGridViewCellStyle1;
            colDbColumnName.HeaderText = "列名称";
            colDbColumnName.MinimumWidth = 10;
            colDbColumnName.Name = "colDbColumnName";
            colDbColumnName.ReadOnly = true;
            colDbColumnName.Resizable = DataGridViewTriState.True;
            colDbColumnName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colClassPropName
            // 
            colClassPropName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colClassPropName.DataPropertyName = "ClassPropName";
            colClassPropName.HeaderText = "属性名称";
            colClassPropName.Name = "colClassPropName";
            // 
            // colRequired
            // 
            colRequired.DataPropertyName = "Required";
            colRequired.FalseValue = "False";
            colRequired.HeaderText = "必填";
            colRequired.Name = "colRequired";
            colRequired.Resizable = DataGridViewTriState.True;
            colRequired.SortMode = DataGridViewColumnSortMode.Automatic;
            colRequired.TrueValue = "True";
            colRequired.Width = 60;
            // 
            // colIsIdentity
            // 
            colIsIdentity.DataPropertyName = "IsIdentity";
            colIsIdentity.FalseValue = "False";
            colIsIdentity.HeaderText = "唯一";
            colIsIdentity.Name = "colIsIdentity";
            colIsIdentity.Resizable = DataGridViewTriState.True;
            colIsIdentity.SortMode = DataGridViewColumnSortMode.Automatic;
            colIsIdentity.TrueValue = "True";
            colIsIdentity.Width = 60;
            // 
            // colPrimaryKey
            // 
            colPrimaryKey.DataPropertyName = "IsPrimaryKey";
            colPrimaryKey.FalseValue = "True";
            colPrimaryKey.HeaderText = "主键";
            colPrimaryKey.Name = "colPrimaryKey";
            colPrimaryKey.TrueValue = "False";
            colPrimaryKey.Width = 60;
            // 
            // colHide
            // 
            colHide.DataPropertyName = "IsHide";
            colHide.FalseValue = "False";
            colHide.HeaderText = "隐藏";
            colHide.Name = "colHide";
            colHide.TrueValue = "True";
            colHide.Width = 60;
            // 
            // colDescription
            // 
            colDescription.DataPropertyName = "Description";
            colDescription.HeaderText = "描述";
            colDescription.Name = "colDescription";
            colDescription.Resizable = DataGridViewTriState.True;
            colDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colDbType
            // 
            colDbType.DataPropertyName = "DbType";
            colDbType.HeaderText = "库数据类型";
            colDbType.Name = "colDbType";
            colDbType.Width = 80;
            // 
            // colCodeType
            // 
            colCodeType.DataPropertyName = "CodeType";
            colCodeType.HeaderText = "类数据类型";
            colCodeType.Name = "colCodeType";
            colCodeType.ReadOnly = true;
            colCodeType.Width = 80;
            // 
            // colSort
            // 
            colSort.DataPropertyName = "Sort";
            colSort.HeaderText = "排序";
            colSort.Name = "colSort";
            colSort.Width = 60;
            // 
            // TableColumnEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(gdv_columns);
            Controls.Add(uPanel2);
            Controls.Add(uPanel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TableColumnEditor";
            Text = "表属性编辑";
            FormClosing += TableColumnEditor_FormClosing;
            uPanel1.ResumeLayout(false);
            uPanel1.PerformLayout();
            uPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gdv_columns).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private WinUI.Controls.Panel.UPanel uPanel1;
        private WinUI.Controls.Panel.UPanel uPanel2;
        private WinUI.Controls.DataGridView.DataGridViewWithFilter gdv_columns;
        private WinUI.Controls.Button.UButton btnSave;
        private Label label4;
        private WinUI.Controls.TextBox.UTextBox txtClassName;
        private Label label2;
        private WinUI.Controls.TextBox.UTextBox txtTableName;
        private Label label1;
        private WinUI.Controls.TextBox.UTextBox txtDescription;
        private DataGridViewTextBoxColumn colDbColumnName;
        private DataGridViewTextBoxColumn colClassPropName;
        private DataGridViewCheckBoxColumn colRequired;
        private DataGridViewCheckBoxColumn colIsIdentity;
        private DataGridViewCheckBoxColumn colPrimaryKey;
        private DataGridViewCheckBoxColumn colHide;
        private DataGridViewTextBoxColumn colDescription;
        private DataGridViewTextBoxColumn colDbType;
        private DataGridViewTextBoxColumn colCodeType;
        private DataGridViewTextBoxColumn colSort;
    }
}