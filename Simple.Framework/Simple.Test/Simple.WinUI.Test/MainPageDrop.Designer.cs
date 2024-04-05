namespace Simple.WinUI.Test
{
    partial class MainPageDrop
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
            uComboBox1 = new Controls.ComboBox.UComboBox();
            uComboBox2 = new Controls.ComboBox.UComboBox();
            SuspendLayout();
            // 
            // uButton1
            // 
            uButton1.BackColor = SystemColors.ActiveBorder;
            uButton1.ButtonState = WinUI.Controls.ButtonState.Normal;
            uButton1.HoverColor = Color.LightGray;
            uButton1.ImageWidth = 18;
            uButton1.Location = new Point(270, 131);
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
            btnSaveConfig.Location = new Point(377, 131);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.PressedColor = Color.DarkGray;
            btnSaveConfig.Radius = 8;
            btnSaveConfig.RoundStyle = RoundStyle.All;
            btnSaveConfig.Size = new Size(134, 83);
            btnSaveConfig.TabIndex = 1;
            btnSaveConfig.Text = "保存当前模板配置";
            btnSaveConfig.UseGradientMode = false;
            btnSaveConfig.UseVisualStyleBackColor = false;
            // 
            // uComboBox1
            // 
            uComboBox1.ArrowColor = Color.White;
            uComboBox1.BaseColor = Color.Silver;
            uComboBox1.BorderColor = Color.Silver;
            uComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            uComboBox1.FormattingEnabled = true;
            uComboBox1.ItemHoverColor = Color.Silver;
            uComboBox1.Location = new Point(285, 281);
            uComboBox1.Name = "uComboBox1";
            uComboBox1.Size = new Size(121, 24);
            uComboBox1.TabIndex = 2;
            // 
            // uComboBox2
            // 
            uComboBox2.ArrowColor = Color.White;
            uComboBox2.BaseColor = Color.Silver;
            uComboBox2.BorderColor = Color.Silver;
            uComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            uComboBox2.FormattingEnabled = true;
            uComboBox2.ItemHoverColor = Color.Silver;
            uComboBox2.Location = new Point(550, 245);
            uComboBox2.Name = "uComboBox2";
            uComboBox2.Size = new Size(121, 24);
            uComboBox2.TabIndex = 3;
            // 
            // MainPageDrop
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(uComboBox2);
            Controls.Add(uComboBox1);
            Controls.Add(btnSaveConfig);
            Controls.Add(uButton1);
            Name = "MainPageDrop";
            Text = "MainPage";
            ResumeLayout(false);
        }

        #endregion

        private Controls.Button.UButton uButton1;
        private Controls.Button.UButton btnSaveConfig;
        private Controls.ComboBox.UComboBox uComboBox1;
        private Controls.ComboBox.UComboBox uComboBox2;
    }
}