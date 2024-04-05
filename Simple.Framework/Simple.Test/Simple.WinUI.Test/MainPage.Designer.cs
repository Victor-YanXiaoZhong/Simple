namespace Simple.WinUI.Test
{
    partial class MainPage
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
            uButton1 = new Controls.Button.UButton();
            btnSaveConfig = new Controls.Button.UButton();
            uComboBox2 = new Controls.ComboBox.UComboBox();
            uComboBoxDropPanel1 = new Controls.PopupControl.UComboBoxDropPanel();
            uPanel1 = new Controls.Panel.UPanel();
            cSharpNumericUpDown1 = new Controls.NumericUpDown.CSharpNumericUpDown();
            uGroupBox1 = new Controls.GroupBox.UGroupBox();
            uButton2 = new Controls.Button.UButton();
            uPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cSharpNumericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // uButton1
            // 
            uButton1.BackColor = SystemColors.ActiveBorder;
            uButton1.ButtonState = WinUI.Controls.ButtonState.Normal;
            uButton1.HoverColor = Color.LightGray;
            uButton1.ImageWidth = 18;
            uButton1.Location = new Point(26, 58);
            uButton1.Name = "uButton1";
            uButton1.PressedColor = Color.DarkGray;
            uButton1.Radius = 22;
            uButton1.RoundStyle = RoundStyle.All;
            uButton1.Size = new Size(75, 83);
            uButton1.TabIndex = 0;
            uButton1.Text = "uButton1";
            uButton1.UseGradientMode = false;
            uButton1.UseVisualStyleBackColor = false;
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.BackColor = SystemColors.ButtonFace;
            btnSaveConfig.ButtonState = WinUI.Controls.ButtonState.Normal;
            btnSaveConfig.HoverColor = Color.LightGray;
            btnSaveConfig.ImageWidth = 18;
            btnSaveConfig.Location = new Point(125, 63);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.PressedColor = Color.DarkGray;
            btnSaveConfig.Radius = 8;
            btnSaveConfig.RoundStyle = RoundStyle.All;
            btnSaveConfig.Size = new Size(94, 73);
            btnSaveConfig.TabIndex = 1;
            btnSaveConfig.Text = "保存当前模板配置";
            btnSaveConfig.UseGradientMode = false;
            btnSaveConfig.UseVisualStyleBackColor = false;
            // 
            // uComboBox2
            // 
            uComboBox2.ArrowColor = Color.White;
            uComboBox2.BaseColor = Color.Silver;
            uComboBox2.BorderColor = Color.Silver;
            uComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            uComboBox2.FormattingEnabled = true;
            uComboBox2.ItemHoverColor = Color.Silver;
            uComboBox2.Location = new Point(108, 24);
            uComboBox2.Name = "uComboBox2";
            uComboBox2.Size = new Size(121, 24);
            uComboBox2.TabIndex = 3;
            // 
            // uComboBoxDropPanel1
            // 
            uComboBoxDropPanel1.AllowResizeDropDown = true;
            uComboBoxDropPanel1.ControlSize = new Size(1, 1);
            uComboBoxDropPanel1.DropDownControl = null;
            uComboBoxDropPanel1.DropDownSizeMode = WinUI.Controls.PopupControl.UComboBoxDropPanel.SizeMode.UseControlSize;
            uComboBoxDropPanel1.DropSize = new Size(121, 106);
            uComboBoxDropPanel1.Location = new Point(111, 281);
            uComboBoxDropPanel1.Name = "uComboBoxDropPanel1";
            uComboBoxDropPanel1.Size = new Size(151, 25);
            uComboBoxDropPanel1.TabIndex = 4;
            // 
            // uPanel1
            // 
            uPanel1.BackColor = SystemColors.ActiveCaption;
            uPanel1.BorderColor = Color.Silver;
            uPanel1.Controls.Add(uComboBox2);
            uPanel1.Controls.Add(btnSaveConfig);
            uPanel1.Controls.Add(uButton1);
            uPanel1.Location = new Point(477, 56);
            uPanel1.Name = "uPanel1";
            uPanel1.Radius = 0;
            uPanel1.Size = new Size(232, 148);
            uPanel1.TabIndex = 5;
            // 
            // cSharpNumericUpDown1
            // 
            cSharpNumericUpDown1.ArrowColor = Color.White;
            cSharpNumericUpDown1.BaseColor = Color.Silver;
            cSharpNumericUpDown1.BorderColor = Color.Silver;
            cSharpNumericUpDown1.Location = new Point(145, 79);
            cSharpNumericUpDown1.Name = "cSharpNumericUpDown1";
            cSharpNumericUpDown1.Size = new Size(120, 23);
            cSharpNumericUpDown1.TabIndex = 6;
            // 
            // uGroupBox1
            // 
            uGroupBox1.GroupBoxColor = Color.Gainsboro;
            uGroupBox1.Location = new Point(313, 281);
            uGroupBox1.Name = "uGroupBox1";
            uGroupBox1.Size = new Size(200, 100);
            uGroupBox1.TabIndex = 7;
            uGroupBox1.TabStop = false;
            uGroupBox1.Text = "uGroupBox1";
            // 
            // uButton2
            // 
            uButton2.BackColor = SystemColors.AppWorkspace;
            uButton2.ButtonState = WinUI.Controls.ButtonState.Normal;
            uButton2.HoverColor = Color.LightGray;
            uButton2.ImageWidth = 18;
            uButton2.Location = new Point(206, 168);
            uButton2.Name = "uButton2";
            uButton2.PressedColor = Color.DarkGray;
            uButton2.Radius = 16;
            uButton2.RoundStyle = RoundStyle.All;
            uButton2.Size = new Size(120, 88);
            uButton2.TabIndex = 8;
            uButton2.Text = "uButton2";
            uButton2.UseGradientMode = false;
            uButton2.UseVisualStyleBackColor = false;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(uButton2);
            Controls.Add(uGroupBox1);
            Controls.Add(cSharpNumericUpDown1);
            Controls.Add(uPanel1);
            Controls.Add(uComboBoxDropPanel1);
            Name = "MainPage";
            Text = "MainPage";
            uPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)cSharpNumericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controls.Button.UButton uButton1;
        private Controls.Button.UButton btnSaveConfig;
        private Controls.ComboBox.UComboBox uComboBox2;
        private Controls.PopupControl.UComboBoxDropPanel uComboBoxDropPanel1;
        private Controls.Panel.UPanel uPanel1;
        private Controls.NumericUpDown.CSharpNumericUpDown cSharpNumericUpDown1;
        private Controls.GroupBox.UGroupBox uGroupBox1;
        private Controls.Button.UButton uButton2;
    }
}