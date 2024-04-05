namespace Simple.WinUI.Controls.TreeView.ProperityGrid
{
    partial class TreeViewNodeStateStyleEditor
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
            this.plPenColor = new System.Windows.Forms.Panel();
            this.lbPenColor = new System.Windows.Forms.Label();
            this.plBrushColor = new System.Windows.Forms.Panel();
            this.lbBrushColor = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // plPenColor
            // 
            this.plPenColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plPenColor.Location = new System.Drawing.Point(168, 55);
            this.plPenColor.Name = "plPenColor";
            this.plPenColor.Size = new System.Drawing.Size(66, 29);
            this.plPenColor.TabIndex = 9;
            // 
            // lbPenColor
            // 
            this.lbPenColor.AutoSize = true;
            this.lbPenColor.Location = new System.Drawing.Point(12, 59);
            this.lbPenColor.Name = "lbPenColor";
            this.lbPenColor.Size = new System.Drawing.Size(77, 12);
            this.lbPenColor.TabIndex = 8;
            this.lbPenColor.Text = "节点边框颜色";
            // 
            // plBrushColor
            // 
            this.plBrushColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plBrushColor.Location = new System.Drawing.Point(168, 20);
            this.plBrushColor.Name = "plBrushColor";
            this.plBrushColor.Size = new System.Drawing.Size(66, 29);
            this.plBrushColor.TabIndex = 7;
            // 
            // lbBrushColor
            // 
            this.lbBrushColor.AutoSize = true;
            this.lbBrushColor.Location = new System.Drawing.Point(12, 24);
            this.lbBrushColor.Name = "lbBrushColor";
            this.lbBrushColor.Size = new System.Drawing.Size(77, 12);
            this.lbBrushColor.TabIndex = 6;
            this.lbBrushColor.Text = "节点填充颜色";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(168, 104);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 33);
            this.btnOK.TabIndex = 41;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TreeViewNodeStateStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 152);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.plPenColor);
            this.Controls.Add(this.lbPenColor);
            this.Controls.Add(this.plBrushColor);
            this.Controls.Add(this.lbBrushColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TreeViewNodeStateStyleEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TreeViewNodeStateStyleEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel plPenColor;
        private System.Windows.Forms.Label lbPenColor;
        private System.Windows.Forms.Panel plBrushColor;
        private System.Windows.Forms.Label lbBrushColor;
        private System.Windows.Forms.Button btnOK;
    }
}