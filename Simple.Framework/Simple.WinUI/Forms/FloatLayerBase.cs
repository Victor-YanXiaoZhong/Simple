using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Simple.WinUI.Forms
{
    /// <summary>浮动层基类</summary>
    //Update:201508251451
    //- 将由OnShow中负责的首控件激活改为设TopMost=true实现，同时移除OnShow重写
    //- 解决子控件无聚焦框（焦点虚线框，FocusCues）的问题
    //Update:201508261806
    //- 重绘右下角调整大小手柄，解决系统自绘在XP下太靠边角从而覆盖边框的问题
    //- 支持边缘和边角拖动改变窗体大小
    //- 启用双缓冲
    public class FloatLayerBase : Form
    {
        /// <summary>鼠标消息筛选器</summary>
        //由于本窗体为WS_CHILD，所以不会收到在窗体以外点击鼠标的消息
        //该消息筛选器的作用就是让本窗体获知鼠标点击情况，进而根据鼠标是否在本窗体以外的区域点击，做出相应处理
        private readonly AppMouseMessageHandler _mouseMsgFilter;

        /// <summary>指示本窗体是否已ShowDialog过</summary>
        //由于多次ShowDialog会使OnLoad/OnShown重入，故需设置此标记以供重入时判断
        private bool _isShowDialogAgain;

        //边框相关字段
        private BorderStyle _borderType;

        private Border3DStyle _border3DStyle;
        private ButtonBorderStyle _borderSingleStyle;
        private Color _borderColor;
        private int _borderWidth;//边框宽度，用于绘制SizeGrip时计算边角偏移

        //构造函数
        public FloatLayerBase()
        {
            //初始化消息筛选器。添加和移除在显示/隐藏时负责
            _mouseMsgFilter = new AppMouseMessageHandler(this);

            this.DoubleBuffered = true;

            //初始化基类属性
            InitBaseProperties();

            //初始化边框相关
            _borderType = BorderStyle.Fixed3D;
            _border3DStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            _borderSingleStyle = ButtonBorderStyle.Solid;
            _borderColor = Color.DarkGray;
            this.UpdateBorderWidth();
        }

        protected override sealed CreateParams CreateParams
        {
            get
            {
                CreateParams prms = base.CreateParams;

                //prms.Style = 0;
                //prms.Style |= -2147483648;   //WS_POPUP
                prms.Style |= 0x40000000;      //WS_CHILD  重要，只有CHILD窗体才不会抢父窗体焦点
                prms.Style |= 0x4000000;       //WS_CLIPSIBLINGS
                prms.Style |= 0x10000;         //WS_TABSTOP
                prms.Style &= ~0x40000;        //WS_SIZEBOX       去除
                prms.Style &= ~0x800000;       //WS_BORDER        去除
                prms.Style &= ~0x400000;       //WS_DLGFRAME      去除
                //prms.Style &= ~0x20000;      //WS_MINIMIZEBOX   去除
                //prms.Style &= ~0x10000;      //WS_MAXIMIZEBOX   去除

                prms.ExStyle = 0;
                //prms.ExStyle |= 0x1;         //WS_EX_DLGMODALFRAME 立体边框
                //prms.ExStyle |= 0x8;         //WS_EX_TOPMOST
                prms.ExStyle |= 0x10000;       //WS_EX_CONTROLPARENT
                //prms.ExStyle |= 0x80;        //WS_EX_TOOLWINDOW
                //prms.ExStyle |= 0x100;       //WS_EX_WINDOWEDGE
                //prms.ExStyle |= 0x8000000;   //WS_EX_NOACTIVATE
                //prms.ExStyle |= 0x4;         //WS_EX_NOPARENTNOTIFY

                return prms;
            }
        }

        /// <summary>获取所绘制的边框尺寸（边框宽度x2）</summary>
        [Browsable(false)]
        public Size BorderSize
        {
            get { return new Size(_borderWidth, _borderWidth); }
        }

        /// <summary>指示窗体是否处于可调整大小状态</summary>
        [Browsable(false)]
        public bool CanReSize
        {
            get
            {
                return this.SizeGripStyle == System.Windows.Forms.SizeGripStyle.Show
                || (this.SizeGripStyle == System.Windows.Forms.SizeGripStyle.Auto && Modal);
            }
        }

        /// <summary>获取或设置边框类型</summary>
        [Description("获取或设置边框类型。")]
        [DefaultValue(BorderStyle.Fixed3D)]
        public BorderStyle BorderType
        {
            get { return _borderType; }
            set
            {
                if (_borderType == value) { return; }
                _borderType = value;
                this.UpdateBorderWidth();
                Invalidate();
            }
        }

        /// <summary>获取或设置三维边框样式</summary>
        [Description("获取或设置三维边框样式。")]
        [DefaultValue(Border3DStyle.RaisedInner)]
        public Border3DStyle Border3DStyle
        {
            get { return _border3DStyle; }
            set
            {
                if (_border3DStyle == value) { return; }
                _border3DStyle = value;
                this.UpdateBorderWidth();
                Invalidate();
            }
        }

        /// <summary>获取或设置线型边框样式</summary>
        [Description("获取或设置线型边框样式。")]
        [DefaultValue(ButtonBorderStyle.Solid)]
        public ButtonBorderStyle BorderSingleStyle
        {
            get { return _borderSingleStyle; }
            set
            {
                if (_borderSingleStyle == value) { return; }
                _borderSingleStyle = value;
                this.UpdateBorderWidth();
                Invalidate();
            }
        }

        /// <summary>获取或设置边框颜色（仅当边框类型为线型时有效）</summary>
        [Description("获取或设置边框颜色（仅当边框类型为线型时有效）。")]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value) { return; }
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>获取控件在窗体中的坐标</summary>
        private static Point GetControlLocationInForm(Control c)
        {
            Point pt = c.Location;
            while (!((c = c.Parent) is Form))
            {
                pt.Offset(c.Location);
            }
            return pt;
        }

        /// <summary>统计二进制中1的个数</summary>
        private static int CountOneInBits(uint num)
        {
            int count = 0;
            while (num != 0)
            {
                num &= num - 1;
                count++;
            }
            return count;
        }

        /// <summary>ShowDialog内部方法</summary>
        private DialogResult ShowDialogInternal(Component controlOrItem, Point offset)
        {
            //快速连续弹出本窗体将有可能遇到尚未Hide的情况下再次弹出，这会引发异常，故需做处理
            if (this.Visible) { return System.Windows.Forms.DialogResult.None; }

            this.SetLocationAndOwner(controlOrItem, offset);
            return base.ShowDialog();
        }

        /// <summary>Show内部方法</summary>
        private void ShowInternal(Component controlOrItem, Point offset)
        {
            if (this.Visible) { return; }//原因见ShowDialogInternal

            this.SetLocationAndOwner(controlOrItem, offset);
            base.Show();
        }

        /// <summary>设置坐标及所有者</summary>
        /// <param name="controlOrItem">控件或工具栏项</param>
        /// <param name="offset">相对偏移</param>
        private void SetLocationAndOwner(Component controlOrItem, Point offset)
        {
            Point pt = Point.Empty;

            if (controlOrItem is ToolStripItem)
            {
                ToolStripItem item = (ToolStripItem)controlOrItem;
                pt.Offset(item.Bounds.Location);
                controlOrItem = item.Owner;
            }

            Control c = (Control)controlOrItem;
            pt.Offset(GetControlLocationInForm(c));
            pt.Offset(offset);
            this.Location = pt;

            //设置Owner属性与Show[Dialog](Owner)有不同，当Owner是MDIChild时，后者会改Owner为MDIParent
            this.Owner = c.FindForm();
        }

        /// <summary>更新边框宽度</summary>
        private void UpdateBorderWidth()
        {
            if (_borderType == BorderStyle.None)
            {
                _borderWidth = 0;
            }
            else if (_borderType == BorderStyle.Fixed3D)
            {
                if (_border3DStyle == System.Windows.Forms.Border3DStyle.Adjust) { _borderWidth = 0; }
                else if (_border3DStyle == System.Windows.Forms.Border3DStyle.Flat) { _borderWidth = 1; }
                else { _borderWidth = CountOneInBits((uint)_border3DStyle); }
            }
            else
            {
                if (_borderSingleStyle == ButtonBorderStyle.None) { _borderWidth = 0; }
                else if (_borderSingleStyle == ButtonBorderStyle.Outset) { _borderWidth = 2; }
                else { _borderWidth = 1; }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            //防止重入
            if (_isShowDialogAgain) { return; }

            //为首次ShowDialog设标记
            if (Modal) { _isShowDialogAgain = true; }

            //需得减掉两层边框宽度，运行时尺寸才与设计时完全相符，原因不明
            //确定与ControlBox、FormBorderStyle有关，但具体联系不明
            if (!DesignMode)
            {
                Size size = SystemInformation.FrameBorderSize;
                this.Size -= size + size;//不可以用ClientSize，后者会根据窗口风格重新调整Size
            }
            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            //当本窗体作为ShowDialog弹出时，在收到WM_SHOWWINDOW前，Owner会被Disable
            //故需在收到该消息后立即Enable它，不然Owner窗体和本窗体都将处于无响应状态
            if (m.Msg == 0x18 && m.WParam != IntPtr.Zero && m.LParam == IntPtr.Zero
                && Modal && Owner != null && !Owner.IsDisposed)
            {
                if (Owner.IsMdiChild)
                {
                    //当Owner是MDI子窗体时，被Disable的是MDI主窗体
                    //并且Parent也会指向MDI主窗体，故需改回为Owner，这样弹出窗体的Location才会相对于Owner而非MDIParent
                    NativeMethods.EnableWindow(Owner.MdiParent.Handle, true);
                    NativeMethods.SetParent(this.Handle, Owner.Handle);//只能用API设置Parent，因为模式窗体是TopLevel，.Net拒绝为顶级窗体设置Parent
                }
                else
                {
                    NativeMethods.EnableWindow(Owner.Handle, true);
                }
            }
            else if (m.Msg == 0x84 && this.CanReSize)//WM_NCHITTEST。实现边缘和边角拖动改变窗体大小
            {
                Point pt = this.PointToClient(NativeMethods.MakePoint(m.LParam));
                Size size = this.ClientSize;
                if (new Rectangle(0, 0, 5, 5).Contains(pt))
                {
                    m.Result = (IntPtr)13;//HTTOPLEFT
                    return;
                }
                if (new Rectangle(5, 0, size.Width - 10, 3).Contains(pt))
                {
                    m.Result = (IntPtr)12;//HTTOP
                    return;
                }
                if (new Rectangle(size.Width - 5, 0, 5, 5).Contains(pt))
                {
                    m.Result = (IntPtr)14;//HTTOPRIGHT
                    return;
                }
                if (new Rectangle(size.Width - 3, 5, 3, size.Height - 5 - 16).Contains(pt))
                {
                    m.Result = (IntPtr)11;//HTRIGHT
                    return;
                }
                if (new Rectangle(5, size.Height - 3, size.Width - 5 - 16, 3).Contains(pt))
                {
                    m.Result = (IntPtr)15;//HTBOTTOM
                    return;
                }
                if (new Rectangle(0, size.Height - 5, 5, 5).Contains(pt))
                {
                    m.Result = (IntPtr)16;//HTBOTTOMLEFT
                    return;
                }
                if (new Rectangle(0, 5, 3, size.Height - 10).Contains(pt))
                {
                    m.Result = (IntPtr)10;//HTLEFT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        //画边框
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (_borderType == BorderStyle.Fixed3D)//绘制3D边框
            {
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle);
            }
            else if (_borderType == BorderStyle.FixedSingle)//绘制线型边框
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, BorderSingleStyle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.CanReSize)
            {
                Size clientSize = this.ClientSize;
                Rectangle rect = new Rectangle(clientSize.Width - 16, clientSize.Height - 16, 16, 16);

                //画手柄
                DrawSizeGrip(e.Graphics, new Rectangle(rect.Location - BorderSize - new Size(1, 1), rect.Size));

                //刨掉SizeGrip区域，防止基类再画
                e.Graphics.SetClip(rect, System.Drawing.Drawing2D.CombineMode.Exclude);
            }
            base.OnPaint(e);
            e.Graphics.ResetClip();
        }

        /// <summary>绘制SizeGrip（调整大小的手柄），子类可重写</summary>
        /// <param name="g">绘制器</param>
        /// <param name="rect">建议作图区域</param>
        protected virtual void DrawSizeGrip(Graphics g, Rectangle rect)
        {
            Color backColor = this.BackColor;
            Brush color1 = new SolidBrush(ControlPaint.Dark(backColor));
            Brush color2 = new SolidBrush(ControlPaint.Dark(backColor, -0.5F));
            Brush color3 = new SolidBrush(ControlPaint.Dark(backColor, -0.1F));
            Brush color4 = new SolidBrush(ControlPaint.Light(backColor));
            Point pt = new Point(rect.X + 5, rect.Y + 5);//左上角偏移

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j >= 3 - i)
                    {
                        g.FillRectangle(color1, new Rectangle(pt.X + j * 3, pt.Y + i * 3, 1, 1));
                        g.FillRectangle(color2, new Rectangle(pt.X + j * 3 + 1, pt.Y + i * 3, 1, 1));
                        g.FillRectangle(color3, new Rectangle(pt.X + j * 3, pt.Y + i * 3 + 1, 1, 1));
                        g.FillRectangle(color4, new Rectangle(pt.X + j * 3 + 1, pt.Y + i * 3 + 1, 1, 1));
                    }
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Visible)
                {
                    //使焦点子控件拥有聚焦框，重写ShowFocusCues较麻烦
                    NativeMethods.SendMessage(this.Handle, 0x127/*WM_CHANGEUISTATE*/, (IntPtr)0x10002/*UISF_HIDEFOCUS | UIS_CLEAR*/, IntPtr.Zero);
                    NativeMethods.SendMessage(this.Handle, 0x128/*WM_UPDATEUISTATE*/, (IntPtr)0x10002/*UISF_HIDEFOCUS | UIS_CLEAR*/, IntPtr.Zero);

                    //显示后添加鼠标消息筛选器以开始捕捉
                    Application.AddMessageFilter(_mouseMsgFilter);
                }
                else
                {
                    //隐藏时则移除筛选器。之所以不放Dispose中是想尽早移除筛选器
                    Application.RemoveMessageFilter(_mouseMsgFilter);
                }
            }
            base.OnVisibleChanged(e);
        }

        //实现窗体客户区拖动
        //在WndProc中实现这个较麻烦，所以放到这里做
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //让鼠标点击客户区时达到与点击标题栏一样的效果，以此实现客户区拖动
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, 0xA1/*WM_NCLBUTTONDOWN*/, (IntPtr)2/*CAPTION*/, IntPtr.Zero);

            base.OnMouseDown(e);
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="control">显示在该控件下方</param>
        public DialogResult ShowDialog(Control control)
        {
            return ShowDialog(control, 0, control.Height);
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="control">触发弹出窗体的控件</param>
        /// <param name="offsetX">相对control水平偏移</param>
        /// <param name="offsetY">相对control垂直偏移</param>
        public DialogResult ShowDialog(Control control, int offsetX, int offsetY)
        {
            return ShowDialog(control, new Point(offsetX, offsetY));
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="control">触发弹出窗体的控件</param>
        /// <param name="offset">相对control偏移</param>
        public DialogResult ShowDialog(Control control, Point offset)
        {
            return this.ShowDialogInternal(control, offset);
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="item">显示在该工具栏项的下方</param>
        public DialogResult ShowDialog(ToolStripItem item)
        {
            return ShowDialog(item, 0, item.Height);
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="item">触发弹出窗体的工具栏项</param>
        /// <param name="offsetX">相对item水平偏移</param>
        /// <param name="offsetY">相对item垂直偏移</param>
        public DialogResult ShowDialog(ToolStripItem item, int offsetX, int offsetY)
        {
            return ShowDialog(item, new Point(offsetX, offsetY));
        }

        /// <summary>显示为模式窗体</summary>
        /// <param name="item">触发弹出窗体的工具栏项</param>
        /// <param name="offset">相对item偏移</param>
        public DialogResult ShowDialog(ToolStripItem item, Point offset)
        {
            return this.ShowDialogInternal(item, offset);
        }

        /// <summary>显示窗体</summary>
        /// <param name="control">显示在该控件下方</param>
        public void Show(Control control)
        {
            Show(control, 0, control.Height);
        }

        /// <summary>显示窗体</summary>
        /// <param name="control">触发弹出窗体的控件</param>
        /// <param name="offsetX">相对control水平偏移</param>
        /// <param name="offsetY">相对control垂直偏移</param>
        public void Show(Control control, int offsetX, int offsetY)
        {
            Show(control, new Point(offsetX, offsetY));
        }

        /// <summary>显示窗体</summary>
        /// <param name="control">触发弹出窗体的控件</param>
        /// <param name="offset">相对control偏移</param>
        public void Show(Control control, Point offset)
        {
            this.ShowInternal(control, offset);
        }

        /// <summary>显示窗体</summary>
        /// <param name="item">显示在该工具栏下方</param>
        public void Show(ToolStripItem item)
        {
            Show(item, 0, item.Height);
        }

        /// <summary>显示窗体</summary>
        /// <param name="item">触发弹出窗体的工具栏项</param>
        /// <param name="offsetX">相对item水平偏移</param>
        /// <param name="offsetY">相对item垂直偏移</param>
        public void Show(ToolStripItem item, int offsetX, int offsetY)
        {
            Show(item, new Point(offsetX, offsetY));
        }

        /// <summary>显示窗体</summary>
        /// <param name="item">触发弹出窗体的工具栏项</param>
        /// <param name="offset">相对item偏移</param>
        public void Show(ToolStripItem item, Point offset)
        {
            this.ShowInternal(item, offset);
        }

        #region 屏蔽对本类影响重大的基类方法和属性

        //屏蔽原属性
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool ControlBox
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("设置边框请使用Border相关属性！", true)]
        public new FormBorderStyle FormBorderStyle
        { get { return System.Windows.Forms.FormBorderStyle.SizableToolWindow; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public override sealed string Text
        { get { return string.Empty; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool HelpButton
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new Image Icon
        { get { return null; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool IsMdiContainer
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool MaximizeBox
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool MinimizeBox
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool ShowIcon
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool ShowInTaskbar
        { get { return false; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new FormStartPosition StartPosition
        { get { return FormStartPosition.Manual; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new bool TopMost
        { get { return true; } set { } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("禁用该属性！", true)]
        public new FormWindowState WindowState
        { get { return FormWindowState.Normal; } set { } }

        /// <summary>初始化部分基类属性</summary>
        private void InitBaseProperties()
        {
            base.ControlBox = false;                           //重要
            //必须得是SizableToolWindow才能支持调整大小的同时，不受SystemInformation.MinWindowTrackSize的限制
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            base.Text = string.Empty;                          //重要
            base.HelpButton = false;
            base.Icon = null;
            base.IsMdiContainer = false;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;     //重要
            base.TopMost = true; //使本窗体像普通窗体一样显示后自动激活首控件
            base.WindowState = FormWindowState.Normal;
        }

        //屏蔽原方法
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("请使用别的重载！", true)]
        public new DialogResult ShowDialog()
        { throw new NotImplementedException(); }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("请使用别的重载！", true)]
        public new DialogResult ShowDialog(IWin32Window owner)
        { throw new NotImplementedException(); }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("请使用别的重载！", true)]
        public new void Show()
        { throw new NotImplementedException(); }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("请使用别的重载！", true)]
        public new void Show(IWin32Window owner)
        { throw new NotImplementedException(); }

        #endregion 屏蔽对本类影响重大的基类方法和属性

        /// <summary>API封装类</summary>
        private static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            public static Rectangle GetWindowRect(IntPtr hwnd)
            {
                RECT rect;
                GetWindowRect(hwnd, out rect);
                return (Rectangle)rect;
            }

            public static int LOWORD(IntPtr n)
            {
                return ((int)n) & 0xFFFF;
            }

            public static int HIWORD(IntPtr n)
            {
                return (((int)n) >> 16) & 0xFFFF;
            }

            public static Point MakePoint(IntPtr n)
            {
                return new Point(LOWORD(n), HIWORD(n));
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;

                public static explicit operator Rectangle(RECT rect)
                {
                    return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                }
            }
        }

        /// <summary>程序鼠标消息筛选器</summary>
        private class AppMouseMessageHandler : IMessageFilter
        {
            private readonly FloatLayerBase _layerForm;

            public AppMouseMessageHandler(FloatLayerBase layerForm)
            {
                _layerForm = layerForm;
            }

            public bool PreFilterMessage(ref Message m)
            {
                //如果在本窗体以外点击鼠标，隐藏本窗体
                //若想在点击标题栏、滚动条等非客户区也要让本窗体消失，取消0xA1的注释即可
                //本例是根据坐标判断，亦可以改为根据句柄，但要考虑子孙控件
                //之所以用API而不用Form.DesktopBounds是因为后者不可靠
                if ((m.Msg == 0x201/*|| m.Msg==0xA1*/)
                    && _layerForm.Visible && !NativeMethods.GetWindowRect(_layerForm.Handle).Contains(MousePosition))
                {
                    _layerForm.Hide();//之所以不Close是考虑应该由调用者负责销毁
                }

                return false;
            }
        }
    }
}