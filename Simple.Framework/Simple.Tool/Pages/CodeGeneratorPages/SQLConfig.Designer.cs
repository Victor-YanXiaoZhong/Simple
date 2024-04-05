namespace Simple.Tool.Pages.CodeGeneratorPages
{
    partial class SQLConfig
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
            txtServer = new WinUI.Controls.TextBox.UTextBox();
            label1 = new Label();
            label2 = new Label();
            txtDatabase = new WinUI.Controls.TextBox.UTextBox();
            label3 = new Label();
            txtUser = new WinUI.Controls.TextBox.UTextBox();
            label4 = new Label();
            txtPwd = new WinUI.Controls.TextBox.UTextBox();
            btnCancel = new WinUI.Controls.Button.UButton();
            btnTest = new WinUI.Controls.Button.UButton();
            btnSave = new WinUI.Controls.Button.UButton();
            label5 = new Label();
            cmbDataType = new WinUI.Controls.ComboBox.UComboBox();
            comBoxDataDicBindingSource = new BindingSource(components);
            txtConnectStr = new WinUI.Controls.TextBox.UTextBox();
            ((System.ComponentModel.ISupportInitialize)comBoxDataDicBindingSource).BeginInit();
            SuspendLayout();
            // 
            // txtServer
            // 
            txtServer.BorderColor = Color.FromArgb(0, 0, 0);
            txtServer.BorderWidth = 1;
            txtServer.HotColor = Color.FromArgb(0, 0, 0);
            txtServer.Location = new Point(133, 26);
            txtServer.Name = "txtServer";
            txtServer.PlaceholderColor = Color.FromArgb(224, 224, 224);
            txtServer.PlaceholderText = "SqlIte填写文件地址";
            txtServer.Size = new Size(246, 23);
            txtServer.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 30);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 1;
            label1.Text = "服务器地址";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(81, 94);
            label2.Name = "label2";
            label2.Size = new Size(44, 17);
            label2.TabIndex = 3;
            label2.Text = "数据库";
            // 
            // txtDatabase
            // 
            txtDatabase.BorderColor = Color.FromArgb(0, 0, 0);
            txtDatabase.BorderWidth = 1;
            txtDatabase.HotColor = Color.FromArgb(0, 0, 0);
            txtDatabase.Location = new Point(132, 90);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.PlaceholderColor = Color.Empty;
            txtDatabase.PlaceholderText = null;
            txtDatabase.Size = new Size(246, 23);
            txtDatabase.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(93, 120);
            label3.Name = "label3";
            label3.Size = new Size(32, 17);
            label3.TabIndex = 5;
            label3.Text = "用户";
            // 
            // txtUser
            // 
            txtUser.BorderColor = Color.FromArgb(0, 0, 0);
            txtUser.BorderWidth = 1;
            txtUser.HotColor = Color.FromArgb(0, 0, 0);
            txtUser.Location = new Point(132, 119);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderColor = Color.Empty;
            txtUser.PlaceholderText = null;
            txtUser.Size = new Size(246, 23);
            txtUser.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(93, 152);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 7;
            label4.Text = "密码";
            // 
            // txtPwd
            // 
            txtPwd.BorderColor = Color.FromArgb(0, 0, 0);
            txtPwd.BorderWidth = 1;
            txtPwd.HotColor = Color.FromArgb(0, 0, 0);
            txtPwd.Location = new Point(132, 148);
            txtPwd.Name = "txtPwd";
            txtPwd.PlaceholderColor = Color.Empty;
            txtPwd.PlaceholderText = null;
            txtPwd.Size = new Size(246, 23);
            txtPwd.TabIndex = 6;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = SystemColors.Control;
            btnCancel.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnCancel.HoverColor = Color.LightGray;
            btnCancel.ImageWidth = 18;
            btnCancel.Location = new Point(312, 283);
            btnCancel.Name = "btnCancel";
            btnCancel.PressedColor = Color.DarkGray;
            btnCancel.Radius = 8;
            btnCancel.RoundStyle = WinUI.RoundStyle.All;
            btnCancel.Size = new Size(85, 29);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "取消";
            btnCancel.UseGradientMode = false;
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnTest
            // 
            btnTest.BackColor = SystemColors.Control;
            btnTest.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnTest.HoverColor = Color.LightGray;
            btnTest.ImageWidth = 18;
            btnTest.Location = new Point(22, 283);
            btnTest.Name = "btnTest";
            btnTest.PressedColor = Color.DarkGray;
            btnTest.Radius = 8;
            btnTest.RoundStyle = WinUI.RoundStyle.All;
            btnTest.Size = new Size(85, 29);
            btnTest.TabIndex = 9;
            btnTest.Text = "测试连接";
            btnTest.UseGradientMode = false;
            btnTest.UseVisualStyleBackColor = false;
            btnTest.Click += btnTest_Click;
            // 
            // btnSave
            // 
            btnSave.BackColor = SystemColors.Control;
            btnSave.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnSave.HoverColor = Color.LightGray;
            btnSave.ImageWidth = 18;
            btnSave.Location = new Point(168, 283);
            btnSave.Name = "btnSave";
            btnSave.PressedColor = Color.DarkGray;
            btnSave.Radius = 8;
            btnSave.RoundStyle = WinUI.RoundStyle.All;
            btnSave.Size = new Size(85, 29);
            btnSave.TabIndex = 10;
            btnSave.Text = "确定";
            btnSave.UseGradientMode = false;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(58, 64);
            label5.Name = "label5";
            label5.Size = new Size(68, 17);
            label5.TabIndex = 11;
            label5.Text = "数据库类型";
            // 
            // cmbDataType
            // 
            cmbDataType.ArrowColor = Color.White;
            cmbDataType.BaseColor = SystemColors.Control;
            cmbDataType.BorderColor = Color.Silver;
            cmbDataType.DataSource = comBoxDataDicBindingSource;
            cmbDataType.DisplayMember = "Text";
            cmbDataType.DrawMode = DrawMode.OwnerDrawFixed;
            cmbDataType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDataType.FormattingEnabled = true;
            cmbDataType.ItemHoverColor = Color.Silver;
            cmbDataType.Location = new Point(132, 57);
            cmbDataType.Name = "cmbDataType";
            cmbDataType.Size = new Size(247, 24);
            cmbDataType.TabIndex = 12;
            cmbDataType.ValueMember = "Value";
            // 
            // comBoxDataDicBindingSource
            // 
            comBoxDataDicBindingSource.DataSource = typeof(WinUI.Controls.ComboBox.ComBoxListItem);
            // 
            // txtConnectStr
            // 
            txtConnectStr.BorderColor = Color.FromArgb(0, 0, 0);
            txtConnectStr.BorderWidth = 1;
            txtConnectStr.HotColor = Color.FromArgb(0, 0, 0);
            txtConnectStr.Location = new Point(58, 177);
            txtConnectStr.Multiline = true;
            txtConnectStr.Name = "txtConnectStr";
            txtConnectStr.PlaceholderColor = Color.Empty;
            txtConnectStr.PlaceholderText = null;
            txtConnectStr.Size = new Size(320, 87);
            txtConnectStr.TabIndex = 13;
            // 
            // SQLConfig
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(423, 324);
            ControlBox = false;
            Controls.Add(txtConnectStr);
            Controls.Add(cmbDataType);
            Controls.Add(label5);
            Controls.Add(btnSave);
            Controls.Add(btnTest);
            Controls.Add(btnCancel);
            Controls.Add(label4);
            Controls.Add(txtPwd);
            Controls.Add(label3);
            Controls.Add(txtUser);
            Controls.Add(label2);
            Controls.Add(txtDatabase);
            Controls.Add(label1);
            Controls.Add(txtServer);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SQLConfig";
            Text = "连接数据库";
            ((System.ComponentModel.ISupportInitialize)comBoxDataDicBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private WinUI.Controls.TextBox.UTextBox txtServer;
        private Label label1;
        private Label label2;
        private WinUI.Controls.TextBox.UTextBox txtDatabase;
        private Label label3;
        private WinUI.Controls.TextBox.UTextBox txtUser;
        private Label label4;
        private WinUI.Controls.TextBox.UTextBox txtPwd;
        private WinUI.Controls.Button.UButton btnCancel;
        private WinUI.Controls.Button.UButton btnTest;
        private WinUI.Controls.Button.UButton btnSave;
        private Label label5;
        private WinUI.Controls.ComboBox.UComboBox cmbDataType;
        private BindingSource comBoxDataDicBindingSource;
        private WinUI.Controls.TextBox.UTextBox txtConnectStr;
    }
}