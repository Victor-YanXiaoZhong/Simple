using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Simple.WinUI.Controls.ComboBox
{
    #region 下拉框

    /// <summary>WWW.CSharpSkin.COM 下拉框</summary>
    public class UComboBox : System.Windows.Forms.ComboBox
    {
        #region 下拉框句柄

        /// <summary>下拉框句柄</summary>
        private IntPtr _comboxHandle;

        #endregion 下拉框句柄

        #region 下拉框状态

        /// <summary>下拉框状态</summary>
        private ComboBoxState _comboBoxState;

        #endregion 下拉框状态

        #region 基础颜色

        private Color _baseColor = Color.Silver;

        /// <summary>基础颜色</summary>
        [Browsable(true)]
        [Description("下拉框基础颜色")]
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

        /// <summary>下拉框边框颜色</summary>
        [Browsable(true)]
        [Description("下拉框边框颜色")]
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

        #region 下拉列表获取鼠标焦点时颜色

        private Color _itemHoverColor = Color.Silver;

        /// <summary>下拉框边框颜色</summary>
        [Browsable(true)]
        [Description("下拉列表获取鼠标焦点时颜色")]
        public Color ItemHoverColor
        {
            get
            {
                return _itemHoverColor;
            }
            set
            {
                _itemHoverColor = value;
                this.Invalidate();
            }
        }

        #endregion 下拉列表获取鼠标焦点时颜色

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

        #region 是否重绘

        /// <summary>是否重绘</summary>
        protected bool _isOpenPainting = true;

        #endregion 是否重绘

        #region 构造

        /// <summary>构造</summary>
        public UComboBox() : base()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        #endregion 构造

        #region 返回下拉框按钮编辑区域

        /// <summary>返回下拉框按钮编辑区域</summary>
        private Rectangle ButtonRect
        {
            get
            {
                return GetDropDownButtonRect();
            }
        }

        #endregion 返回下拉框按钮编辑区域

        #region 下拉框按钮是否按下

        /// <summary>下拉框按钮是否按下</summary>
        private bool IsComboBoxButtonPressed
        {
            get
            {
                if (IsHandleCreated)
                {
                    return GetComboBoxButtonPressed();
                }
                return false;
            }
        }

        #endregion 下拉框按钮是否按下

        #region 返回可绘制区域

        /// <summary>返回可绘制区域</summary>
        private Rectangle EditRect
        {
            get
            {
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Rectangle rect = new Rectangle(
                        3, 3, Width - ButtonRect.Width - 6, Height - 6);
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        rect.X += ButtonRect.Right;
                    }
                    return rect;
                }
                if (IsHandleCreated && _comboxHandle != IntPtr.Zero)
                {
                    RECT rcClient = new RECT();
                    WinApi.GetWindowRect(_comboxHandle, ref rcClient);
                    return RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }

        #endregion 返回可绘制区域

        #region 重写初始化控件事件，获取控件句柄

        /// <summary>重写初始化控件事件，获取控件句柄</summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            COMBOBOXINFO cbi = GetComboBoxInfo();
            _comboxHandle = cbi.hwndEdit;
        }

        #endregion 重写初始化控件事件，获取控件句柄

        #region 重写鼠标移动事件

        /// <summary>重写鼠标移动事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (ButtonRect.Contains(point))
            {
                _comboBoxState = ComboBoxState.Hover;
            }
            else
            {
                _comboBoxState = ComboBoxState.Normal;
            }
        }

        #endregion 重写鼠标移动事件

        #region 重写鼠标进入事件

        /// <summary>重写鼠标进入事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Point point = PointToClient(Cursor.Position);
            if (ButtonRect.Contains(point))
            {
                _comboBoxState = ComboBoxState.Hover;
            }
        }

        #endregion 重写鼠标进入事件

        #region 重写鼠标离开事件

        /// <summary>重写鼠标离开事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _comboBoxState = ComboBoxState.Normal;
        }

        #endregion 重写鼠标离开事件

        #region 重写鼠标松下事件

        /// <summary>重写鼠标松下事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _comboBoxState = ComboBoxState.Normal;
        }

        #endregion 重写鼠标松下事件

        #region 监听系统消息，进行控件绘制

        /// <summary>监听系统消息，进行控件绘制</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_PAINT:
                    WMPaint(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 监听系统消息，进行控件绘制

        #region 根据系统消息进行绘制

        /// <summary>根据系统消息进行绘制</summary>
        /// <param name="m"></param>
        private void WMPaint(ref Message m)
        {
            if (base.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.WndProc(ref m);
                return;
            }
            if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (_isOpenPainting)
                {
                    PAINTSTRUCT ps = new PAINTSTRUCT();
                    _isOpenPainting = false;
                    WinApi.BeginPaint(m.HWnd, ref ps);
                    RenderComboBox(ref m);
                    WinApi.EndPaint(m.HWnd, ref ps);
                    _isOpenPainting = true;
                    m.Result = WMRESULT.TRUE;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
                RenderComboBox(ref m);
            }
        }

        #endregion 根据系统消息进行绘制

        #region 渲染下拉框

        /// <summary>渲染下拉框</summary>
        /// <param name="m"></param>
        private void RenderComboBox(ref Message m)
        {
            Rectangle rect = new Rectangle(Point.Empty, Size);
            Rectangle buttonRect = ButtonRect;
            ComboBoxState state = IsComboBoxButtonPressed ? ComboBoxState.Pressed : _comboBoxState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd))
            {
                RenderComboBoxBackground(g, rect, buttonRect);
                RenderConboBoxDropDownButton(g, ButtonRect, state);
                RenderConboBoxBorder(g, rect);
            }
        }

        #endregion 渲染下拉框

        #region 渲染下拉框边框

        /// <summary>渲染下拉框边框</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void RenderConboBoxBorder(Graphics g, Rectangle rect)
        {
            Color borderColor = base.Enabled ? _borderColor : SystemColors.ControlDarkDark;
            using (Pen pen = new Pen(borderColor))
            {
                rect.Width--;
                rect.Height--;
                g.DrawRectangle(pen, rect);
            }
        }

        #endregion 渲染下拉框边框

        #region 渲染下拉框背景

        /// <summary>渲染下拉框背景</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="buttonRect"></param>
        private void RenderComboBoxBackground(Graphics g, Rectangle rect, Rectangle buttonRect)
        {
            Color backColor = base.Enabled ? base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor))
            {
                buttonRect.Inflate(-1, -1);
                rect.Inflate(-1, -1);
                using (Region region = new Region(rect))
                {
                    region.Exclude(buttonRect);
                    region.Exclude(EditRect);
                    g.FillRegion(brush, region);
                }
            }
        }

        #endregion 渲染下拉框背景

        #region 渲染下拉框下拉按钮

        /// <summary>渲染下拉框下拉按钮</summary>
        /// <param name="g"></param>
        /// <param name="buttonRect"></param>
        /// <param name="state"></param>
        private void RenderConboBoxDropDownButton(Graphics g, Rectangle buttonRect, ComboBoxState state)
        {
            Color baseColor;
            Color backColor = Color.FromArgb(160, 250, 250, 250);
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            Color arrowColor = base.Enabled ?
                _arrowColor : SystemColors.ControlDarkDark;
            Rectangle rect = buttonRect;

            if (base.Enabled)
            {
                switch (state)
                {
                    case ComboBoxState.Hover:
                        baseColor = ControlRender.GetColor(
                            _baseColor, 0, -33, -22, -13);
                        break;

                    case ComboBoxState.Pressed:
                        baseColor = ControlRender.GetColor(
                            _baseColor, 0, -65, -47, -25);
                        break;

                    default:
                        baseColor = _baseColor;
                        break;
                }
            }
            else
            {
                baseColor = SystemColors.ControlDark;
            }

            rect.Inflate(-1, -1);
            RenderComBoBoxScrollBarArrow(g, rect, baseColor, borderColor, backColor, arrowColor, RoundStyle.None, true, false, ArrowDirection.Down, LinearGradientMode.Vertical);
        }

        #endregion 渲染下拉框下拉按钮

        #region 绘制下拉框滚动条箭头

        /// <summary>绘制下拉框滚动条箭头</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="innerBorderColor"></param>
        /// <param name="arrowColor"></param>
        /// <param name="roundStyle"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        /// <param name="arrowDirection"></param>
        /// <param name="mode"></param>
        private void RenderComBoBoxScrollBarArrow(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, Color arrowColor, RoundStyle roundStyle, bool drawBorder, bool drawGlass, ArrowDirection arrowDirection, LinearGradientMode mode)
        {
            ControlRender.RenderBackground(g, rect, baseColor, borderColor, innerBorderColor, roundStyle, 0, 0.45F, drawBorder, drawGlass, mode);
            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                RenderComBoBoxArrow(g, rect, arrowDirection, brush);
            }
        }

        #endregion 绘制下拉框滚动条箭头

        #region 渲染下拉框箭头

        /// <summary>渲染下拉框箭头</summary>
        /// <param name="g"></param>
        /// <param name="dropDownRect"></param>
        /// <param name="direction"></param>
        /// <param name="brush"></param>
        private void RenderComBoBoxArrow(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
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
                        new Point(point.X - 3, point.Y + 2),
                        new Point(point.X + 3, point.Y + 2),
                        new Point(point.X, point.Y - 2) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3),
                        new Point(point.X - 2, point.Y + 3),
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 1),
                        new Point(point.X + 3, point.Y - 1),
                        new Point(point.X, point.Y + 2) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        #endregion 渲染下拉框箭头

        #region 返回系统下拉框

        /// <summary>返回系统下拉框</summary>
        /// <returns></returns>
        private COMBOBOXINFO GetComboBoxInfo()
        {
            COMBOBOXINFO comBoBoxInfo = new COMBOBOXINFO();
            comBoBoxInfo.cbSize = Marshal.SizeOf(comBoBoxInfo);
            WinApi.GetComboBoxInfo(base.Handle, ref comBoBoxInfo);
            return comBoBoxInfo;
        }

        #endregion 返回系统下拉框

        #region 获取下拉框按钮状态

        /// <summary>获取下拉框按钮状态</summary>
        /// <returns></returns>
        private bool GetComboBoxButtonPressed()
        {
            COMBOBOXINFO comBoBoxInfo = GetComboBoxInfo();
            return comBoBoxInfo._comBoboxButtonState == COMBOBOXBUTTONSTATE.STATE_SYSTEM_PRESSED;
        }

        #endregion 获取下拉框按钮状态

        #region 获取下拉框按钮绘制区域

        /// <summary>获取下拉框按钮绘制区域</summary>
        /// <returns></returns>
        private Rectangle GetDropDownButtonRect()
        {
            COMBOBOXINFO comBoBoxInfo = GetComboBoxInfo();
            return comBoBoxInfo.rcButton.Rect;
        }

        #endregion 获取下拉框按钮绘制区域

        /// <summary>绘制下拉列表背景</summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            //获取要在其上绘制项的图形表面
            Graphics g = e.Graphics;
            //获取表示所绘制项的边界的矩形
            System.Drawing.Rectangle rect = e.Bounds;
            //定义要绘制到控件中的图标图像
            //Image ico = Image.FromFile("head.png");
            //获得当前Item的文本

            if (e.Index < 0) return;

            string tempString = this.Items[e.Index].ToString();
            if (DataSource != null)
            {
                tempString = GetItemText(this.Items[e.Index]);
            }
            //定义字体对象
            if (e.Index >= 0)
            {
                //如果当前项是没有状态的普通项
                if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                {
                    //在当前项图形表面上划一个矩形
                    g.FillRectangle(new SolidBrush(BackColor), rect);
                    //在当前项图形表面上划上当前Item的文本
                    g.DrawString(tempString, Font, new SolidBrush(Color.Black), rect.Left + 5, rect.Top);
                    //将绘制聚焦框
                    e.DrawFocusRectangle();
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(ItemHoverColor), rect);
                    g.DrawString(tempString, Font, new SolidBrush(Color.Black), rect.Left + 5, rect.Top);
                    e.DrawFocusRectangle();
                }
            }
        }
    }

    #endregion 下拉框
}