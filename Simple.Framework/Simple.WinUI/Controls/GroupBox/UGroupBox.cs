using System.ComponentModel;

namespace Simple.WinUI.Controls.GroupBox
{
    #region 带有边框样式的分组框

    /// <summary>WWW.CSharpSkin.COM 带有边框样式的分组框</summary>
    public class UGroupBox : System.Windows.Forms.GroupBox
    {
        #region 构造

        /// <summary>构造</summary>
        public UGroupBox() : base()
        {
            base.SetStyle(
                    ControlStyles.UserPaint |                      // 控件将自行绘制，而不是通过操作系统来绘制
                    ControlStyles.OptimizedDoubleBuffer |          // 该控件首先在缓冲区中绘制，而不是直接绘制到屏幕上，这样可以减少闪烁
                    ControlStyles.AllPaintingInWmPaint |           // 控件将忽略 WM_ERASEBKGND 窗口消息以减少闪烁
                    ControlStyles.ResizeRedraw |                   // 在调整控件大小时重绘控件
                    ControlStyles.SupportsTransparentBackColor,    // 控件接受 alpha 组件小于 255 的 BackColor 以模拟透明
                    true); // 设置以上值为 true
            base.UpdateStyles();
        }

        #endregion 构造

        #region 分组框边框颜色

        /// <summary>分组框边框颜色</summary>
        private Color? _groupBoxColor = Color.Gainsboro;

        [Browsable(true)]
        [Description("分组框边框颜色")]
        public Color GroupBoxColor
        {
            get
            {
                return _groupBoxColor == null ? Color.Gainsboro : _groupBoxColor.Value;
            }
            set
            {
                _groupBoxColor = value.Name == "0" ? Color.Gainsboro : value;
                this.Invalidate();
            }
        }

        #endregion 分组框边框颜色

        #region 绘制分组框

        /// <summary>绘制分组框</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_groupBoxColor == null) return;

            e.Graphics.Clear(this.BackColor);
            Pen border = new Pen(_groupBoxColor.Value);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(ForeColor), 10, 1);
            e.Graphics.DrawLine(border, 1, 7, 8, 7);
            e.Graphics.DrawLine(border, e.Graphics.MeasureString(this.Text, this.Font).Width + 8, 7, this.Width - 2, 7);
            e.Graphics.DrawLine(border, 1, 7, 1, this.Height - 2);
            e.Graphics.DrawLine(border, 1, this.Height - 2, this.Width - 2, this.Height - 2);
            e.Graphics.DrawLine(border, this.Width - 2, 7, this.Width - 2, this.Height - 2);
        }

        #endregion 绘制分组框
    }

    #endregion 带有边框样式的分组框
}