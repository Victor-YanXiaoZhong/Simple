namespace Simple.WinUI
{
    partial class DemoShowForm
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
            uComboBoxDropPanel1 = new Controls.PopupControl.UComboBoxDropPanel();
            SuspendLayout();
            // 
            // uComboBoxDropPanel1
            // 
            uComboBoxDropPanel1.AllowResizeDropDown = true;
            uComboBoxDropPanel1.ControlSize = new Size(1, 1);
            uComboBoxDropPanel1.DropDownControl = null;
            uComboBoxDropPanel1.DropSize = new Size(121, 106);
            uComboBoxDropPanel1.Location = new Point(33, 60);
            uComboBoxDropPanel1.Name = "uComboBoxDropPanel1";
            uComboBoxDropPanel1.Size = new Size(121, 25);
            uComboBoxDropPanel1.TabIndex = 0;
            uComboBoxDropPanel1.Text = "uComboBoxDropPanel1";
            // 
            // DemoShowForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(uComboBoxDropPanel1);
            Name = "DemoShowForm";
            Text = "Demo窗体";
            ResumeLayout(false);
        }

        #endregion

        private Controls.PopupControl.UComboBoxDropPanel uComboBoxDropPanel1;
    }
}