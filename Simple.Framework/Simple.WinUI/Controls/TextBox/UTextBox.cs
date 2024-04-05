using System.ComponentModel;

namespace Simple.WinUI.Controls.TextBox
{
    #region 带有边框颜色的文本框

    /// <summary>WWW.CSharpSkin.COM 带有边框颜色的文本框</summary>
    [ToolboxItem(true)]
    public class UTextBox : System.Windows.Forms.TextBox
    {
        #region 获得当前进程，以便重绘控件

        /// <summary>获得当前进程，以便重绘控件</summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        #endregion 获得当前进程，以便重绘控件

        #region 构造

        /// <summary>构造</summary>
        public UTextBox() : base()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion 构造

        #region 空文本时的提示信息

        private string _placeholderText;

        [Browsable(true)]
        [Description("空文本时的提示信息")]
        [DefaultValue("")]
        public string PlaceholderText
        { get { return _placeholderText; } set { _placeholderText = value; this.Invalidate(); } }

        #endregion 空文本时的提示信息

        #region 空文本时的提示颜色

        private Color _placeholderColor;

        [Browsable(true)]
        [Description("提示文本颜色")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color PlaceholderColor
        { get { return _placeholderColor; } set { _placeholderColor = value; this.Invalidate(); } }

        #endregion 空文本时的提示颜色

        #region 边框宽度

        /// <summary>边框宽度</summary>
        private int _borderWidth = 1;

        [Browsable(true)]
        [Description("边框宽度，默认1")]
        public int BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                _borderWidth = value;
                //有大宽度边框
                if (value > 4)
                {
                    BorderStyle = BorderStyle.FixedSingle;
                    Width += value;
                    Height += value;
                }
                else
                {
                    BorderStyle = BorderStyle.Fixed3D;
                }
                this.Invalidate();
            }
        }

        #endregion 边框宽度

        #region 是否启用热点效果

        /// <summary>是否启用热点效果</summary>
        private bool _isHotTrack = true;

        [Browsable(true)]
        [Description("是否启用热点效果，表示当鼠标经过控件时控件边框是否发生变化。只在控件的BorderStyle为FixedSingle时有效")]
        [DefaultValue(true)]
        public bool IsHotTrack
        {
            get
            {
                return this._isHotTrack;
            }
            set
            {
                this._isHotTrack = value;
                this.Invalidate();
            }
        }

        #endregion 是否启用热点效果

        #region 边框颜色

        /// <summary>边框颜色</summary>
        private Color _borderColor = Color.FromArgb(0, 0, 0);

        [Browsable(true)]
        [Description("获得或设置控件的边框颜色,默认RGB(0,0,0)")]
        public Color BorderColor
        {
            get
            {
                return this._borderColor;
            }
            set
            {
                this._borderColor = value;
                this.Invalidate();
            }
        }

        #endregion 边框颜色

        #region 热点边框颜色

        /// <summary>热点边框颜色</summary>
        private Color _hotBorderColor = Color.FromArgb(0, 0, 0);

        [Browsable(true)]
        [Description("获得或设置当鼠标经过控件时控件的边框颜色。只在控件的BorderStyle为FixedSingle时有效,默认RGB(0,0,0)")]
        public Color HotColor
        {
            get
            {
                return this._hotBorderColor;
            }
            set
            {
                this._hotBorderColor = value;
                this.Invalidate();
            }
        }

        #endregion 热点边框颜色

        #region 是否鼠标MouseOver状态

        /// <summary>是否鼠标MouseOver状态</summary>
        private bool _isMouseOver = false;

        #endregion 是否鼠标MouseOver状态

        #region 鼠标移动到该控件上时触发

        /// <summary>鼠标移动到该控件上时</summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //鼠标状态
            this._isMouseOver = true;
            if (this._isHotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        #endregion 鼠标移动到该控件上时触发

        #region 当鼠标从该控件移开时

        /// <summary>当鼠标从该控件移开时</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this._isMouseOver = false;
            if (this._isHotTrack)
            {
                //重绘
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }

        #endregion 当鼠标从该控件移开时

        #region 当该控件获得焦点时

        /// <summary>当该控件获得焦点时</summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            if (this._isHotTrack)
            {
                //重绘
                this.Invalidate();
            }
            base.OnGotFocus(e);
        }

        #endregion 当该控件获得焦点时

        #region 当该控件失去焦点时

        /// <summary>当该控件失去焦点时</summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            if (this._isHotTrack)
            {
                //重绘
                this.Invalidate();
            }
            base.OnLostFocus(e);
        }

        #endregion 当该控件失去焦点时

        #region 获得操作系统消息,并重绘文本框控件

        /// <summary>获得操作系统消息,并重绘文本框控件</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WinApi.WM_PAINT || m.Msg == WinApi.WM_CTLCOLOREDIT)
            {
                IntPtr hDC = GetWindowDC(m.HWnd);
                if (hDC.ToInt32() == 0)
                {
                    return;
                }

                //只有在边框样式为FixedSingle时自定义边框样式才有效
                if (this.BorderStyle == BorderStyle.FixedSingle)
                {
                    //边框Width为1个像素
                    System.Drawing.Pen pen = new Pen(this._borderColor, this._borderWidth);

                    if (this._isHotTrack)
                    {
                        if (this.Focused)
                        {
                            pen.Color = this._hotBorderColor;
                        }
                        else
                        {
                            if (this._isMouseOver)
                            {
                                pen.Color = this._hotBorderColor;
                            }
                            else
                            {
                                pen.Color = this._borderColor;
                            }
                        }
                    }

                    //绘制边框
                    System.Drawing.Graphics g = Graphics.FromHdc(hDC);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    var bond = this.Bounds;
                    g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                    pen.Dispose();
                    //e.Graphics.DrawLine(new Pen(Color.Red), 0, 0, 100, 20);
                }

                if (Text.Length == 0
                    && (PlaceholderText is null || PlaceholderText.Length > 0)
                    && !Focused)
                {
                    DrawPlaceholderText(Graphics.FromHdc(hDC));
                }
                //返回结果
                m.Result = IntPtr.Zero;
                //释放
                ReleaseDC(m.HWnd, hDC);
            }
        }

        #endregion 获得操作系统消息,并重绘文本框控件

        #region 绘制提示文本

        private void DrawPlaceholderText(Graphics g)
        {
            TextRenderer.DrawText(g, PlaceholderText, Font, new Point(5, 5), PlaceholderColor);
        }

        #endregion 绘制提示文本
    }

    #endregion 带有边框颜色的文本框
}