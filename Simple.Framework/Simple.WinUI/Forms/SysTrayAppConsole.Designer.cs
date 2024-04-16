namespace Simple.WinUI.Forms
{
    partial class SysTrayAppConsole
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

            if (disposing && (trayIcon != null))
            {
                trayIcon.Dispose();
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
            SuspendLayout();
            // 
            // SysTrayAppConsole
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 384);
            IsMdiContainer = true;
            MaximizeBox = false;
            Name = "SysTrayAppConsole";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "SysTrayAppConsole";
            TopMost = true;
            WindowState = FormWindowState.Minimized;
            ResumeLayout(false);
        }

        #endregion
    }
}