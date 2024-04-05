using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Simple.WinUI.Controls.NumericUpDown
{
    #region 美化数值选择器

    /// <summary>WWW.CSharpSkin.COM 美化数值选择器</summary>
    public class CSharpNumericUpDown : System.Windows.Forms.NumericUpDown
    {
        #region 扩展活动窗口

        /// <summary>扩展活动窗口</summary>
        private NumericUpDownButtonNativeWindow _upDownButtonNativeWindow;

        #endregion 扩展活动窗口

        #region 基础颜色

        private Color _baseColor = Color.Silver;

        /// <summary>基础颜色</summary>
        [Browsable(true)]
        [Description("基础颜色")]
        public Color BaseColor
        {
            get
            {
                return _baseColor;
            }
            set

            {
                _baseColor = value;
                this.Invalidate();
            }
        }

        #endregion 基础颜色

        #region 下拉框边框颜色

        private Color _borderColor = Color.Silver;

        /// <summary>边框颜色</summary>
        [Browsable(true)]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        #endregion 下拉框边框颜色

        #region 下拉框按钮箭头颜色

        private Color _arrowColor = Color.White;

        /// <summary>下拉框按钮箭头颜色</summary>
        [Browsable(true)]
        [Description("下拉框按钮箭头颜色")]
        public Color ArrowColor
        {
            get
            {
                return _arrowColor;
            }
            set
            {
                _arrowColor = value;
                this.Invalidate();
            }
        }

        #endregion 下拉框按钮箭头颜色

        public static readonly object eventNumericUpDownButtonPaint = new object();

        #region 构造

        /// <summary>构造</summary>
        public CSharpNumericUpDown() : base()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion 构造

        #region 事件管理

        public event NumericUpDownButtonPaintEventHandler NumericUpDownButtonPaintEvent
        {
            add { base.Events.AddHandler(eventNumericUpDownButtonPaint, value); }
            remove { base.Events.RemoveHandler(eventNumericUpDownButtonPaint, value); }
        }

        #endregion 事件管理

        #region 返回按钮

        /// <summary>返回按钮</summary>
        [Browsable(false)]
        public Control NumericUpDownButton
        {
            get { return base.Controls[0]; }
        }

        #endregion 返回按钮

        #region 绘制数值选择器按钮

        /// <summary>绘制数值选择器按钮</summary>
        /// <param name="e"></param>
        public virtual void OnPaintNumericUpDownButton(NumericUpDownButtonPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            Color upButtonBaseColor = _baseColor;
            Color upButtonBorderColor = _borderColor;
            Color upButtonArrowColor = _arrowColor;
            Color downButtonBaseColor = _baseColor;
            Color downButtonBorderColor = _borderColor;
            Color downButtonArrowColor = _arrowColor;
            Rectangle upButtonRect = rect;
            upButtonRect.Y += -1 + 0;
            upButtonRect.Width -= 0;
            upButtonRect.Height = rect.Height / 2;
            Rectangle downButtonRect = rect;
            downButtonRect.Y = upButtonRect.Height + upButtonRect.Y + 2;
            downButtonRect.Height = rect.Height / 2;// - upButtonRect.Bottom - 2;
            downButtonRect.Width -= 0;
            if (Enabled)
            {
                if (e.MouseOver)
                {
                    if (e.MousePress)
                    {
                        if (e.MouseInUpButton)
                        {
                            upButtonBaseColor = ControlRender.GetColor(_baseColor, 0, -35, -24, -9);
                        }
                        else
                        {
                            downButtonBaseColor = ControlRender.GetColor(_baseColor, 0, -35, -24, -9);
                        }
                    }
                    else
                    {
                        if (e.MouseInUpButton)
                        {
                            upButtonBaseColor = ControlRender.GetColor(_baseColor, 0, 35, 24, 9);
                        }
                        else
                        {
                            downButtonBaseColor = ControlRender.GetColor(_baseColor, 0, 35, 24, 9);
                        }
                    }
                }
            }
            else
            {
                upButtonBaseColor = SystemColors.Control;
                upButtonBorderColor = SystemColors.ControlDark;
                upButtonArrowColor = SystemColors.ControlDark;
                downButtonBaseColor = SystemColors.Control;
                downButtonBorderColor = SystemColors.ControlDark;
                downButtonArrowColor = SystemColors.ControlDark;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color backColor = Enabled ? base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                rect.Inflate(1, 1);
                g.FillRectangle(brush, rect);
            }
            RenderButton(g, upButtonRect, upButtonBaseColor, upButtonBorderColor, upButtonArrowColor, ArrowDirection.Up);
            RenderButton(g, downButtonRect, downButtonBaseColor, downButtonBorderColor, downButtonArrowColor, ArrowDirection.Down);
            NumericUpDownButtonPaintEventHandler handler = base.Events[eventNumericUpDownButtonPaint] as NumericUpDownButtonPaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion 绘制数值选择器按钮

        #region 控件句柄创建后触发

        /// <summary>控件句柄创建后触发</summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (_upDownButtonNativeWindow == null)
            {
                _upDownButtonNativeWindow = new NumericUpDownButtonNativeWindow(this);
            }
        }

        #endregion 控件句柄创建后触发

        #region 销毁时触发

        /// <summary>销毁时触发</summary>
        /// <param name="e"></param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (_upDownButtonNativeWindow != null)
            {
                _upDownButtonNativeWindow.Dispose();
                _upDownButtonNativeWindow = null;
            }
        }

        #endregion 销毁时触发

        #region 监听系统消息，进行绘制

        /// <summary>监听系统消息，进行绘制</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0xF:
                    base.WndProc(ref m);
                    if (BorderStyle != BorderStyle.None)
                    {
                        Color borderColor = Enabled ? _borderColor : SystemColors.ControlDark;
                        using (Graphics g = Graphics.FromHwnd(m.HWnd))
                        {
                            System.Windows.Forms.ControlPaint.DrawBorder(g, ClientRectangle, borderColor, ButtonBorderStyle.Solid);
                        }
                    }
                    break;

                case 0x85:
                    if (BorderStyle != BorderStyle.None)
                    {
                        Color backColor = Enabled ? base.BackColor : SystemColors.Control;
                        Rectangle rect = new Rectangle(0, 0, Width, Height);
                        IntPtr hdc = WinApi.GetWindowDC(m.HWnd);
                        if (hdc == IntPtr.Zero)
                        {
                            //throw new Win32Exception();
                        }
                        try
                        {
                            using (Graphics g = Graphics.FromHdc(hdc))
                            {
                                using (Brush brush = new SolidBrush(backColor))
                                {
                                    g.FillRectangle(brush, rect);
                                }
                            }
                        }
                        finally
                        {
                            WinApi.ReleaseDC(m.HWnd, hdc);
                        }
                    }
                    m.Result = IntPtr.Zero;
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 监听系统消息，进行绘制

        #region 渲染箭头

        /// <summary>渲染箭头</summary>
        /// <param name="g"></param>
        /// <param name="dropDownRect"></param>
        /// <param name="direction"></param>
        /// <param name="brush"></param>
        private void RenderArrow(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        {
            Point point = new Point(dropDownRect.Left + (dropDownRect.Width / 2), dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[] {
                        new Point(point.X + 2, point.Y - 3),
                        new Point(point.X + 2, point.Y + 3),
                        new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] {
                        new Point(point.X - 3, point.Y + 1),
                        new Point(point.X + 3, point.Y + 1),
                        new Point(point.X, point.Y - 1) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3),
                        new Point(point.X - 2, point.Y + 3),
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 3, point.Y - 1),
                        new Point(point.X + 3, point.Y - 1),
                        new Point(point.X, point.Y + 1) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        #endregion 渲染箭头

        #region 渲染按钮

        /// <summary>渲染按钮</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="arrowColor"></param>
        /// <param name="direction"></param>
        private void RenderButton(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color arrowColor, ArrowDirection direction)
        {
            RenderBackground(g, rect, baseColor, borderColor, 0.45f, true, LinearGradientMode.Vertical);
            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                RenderArrow(g, rect, direction, brush);
            }
        }

        #endregion 渲染按钮

        #region 渲染背景

        /// <summary>渲染背景</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="basePosition"></param>
        /// <param name="drawBorder"></param>
        /// <param name="mode"></param>
        private void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, float basePosition, bool drawBorder, LinearGradientMode mode)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, baseColor, Color.FromArgb(50, baseColor), mode))
            {
                //Color[] colors = new Color[4];
                //colors[0] = ControlRender.GetColor(baseColor, 0, 35, 24, 9);
                //colors[1] = ControlRender.GetColor(baseColor, 0, 13, 8, 3);
                //colors[2] = baseColor;
                //colors[3] = ControlRender.GetColor(baseColor, 0, 68, 69, 54);
                //ColorBlend blend = new ColorBlend();
                //blend.Positions =new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                //blend.Colors = colors;
                //brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80)
            {
                Rectangle rectTop = rect;
                if (mode == LinearGradientMode.Vertical)
                {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                }
                else
                {
                    rectTop.Width = (int)(rect.Width * basePosition);
                }
                using (SolidBrush brushAlpha = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }

            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        #endregion 渲染背景
    }

    #endregion 美化数值选择器
}