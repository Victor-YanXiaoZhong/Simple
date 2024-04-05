namespace Simple.WinUI.Controls.NumericUpDown
{
    #region 数字选择框扩展窗口

    /// <summary>WWW.CSharpSkin.COM 数字选择框扩展窗口</summary>
    public class NumericUpDownButtonNativeWindow : NativeWindow, IDisposable
    {
        private const int WM_PAINT = 0xF;
        private const int VK_LBUTTON = 0x1;
        private const int VK_RBUTTON = 0x2;
        private CSharpNumericUpDown _owner;
        private Control _upDownButton;
        private IntPtr _upDownButtonWnd;
        private bool _bPainting;

        #region 构造

        /// <summary>构造</summary>
        /// <param name="owner"></param>
        public NumericUpDownButtonNativeWindow(CSharpNumericUpDown owner) : base()
        {
            _owner = owner;
            _upDownButton = owner.NumericUpDownButton;
            _upDownButtonWnd = _upDownButton.Handle;
            base.AssignHandle(_upDownButtonWnd);
        }

        #endregion 构造

        #region 是否按下鼠标左键

        /// <summary>是否按下鼠标左键</summary>
        /// <returns></returns>
        private bool IsMouseLeftKeyPressed()
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                return (WinApi.GetKeyState(VK_RBUTTON) < 0);
            }
            else
            {
                return (WinApi.GetKeyState(VK_LBUTTON) < 0);
            }
        }

        #endregion 是否按下鼠标左键

        #region 绘制按钮

        /// <summary>绘制按钮</summary>
        private void DrawUpDownButton()
        {
            bool mouseOver = false;
            bool mousePress = IsMouseLeftKeyPressed();
            bool mouseInUpButton = false;
            Rectangle clipRect = _upDownButton.ClientRectangle;
            RECT windowRect = new RECT();
            Point cursorPoint = new Point();
            WinApi.GetCursorPos(ref cursorPoint);
            WinApi.GetWindowRect(_upDownButtonWnd, ref windowRect);
            mouseOver = WinApi.PtInRect(ref windowRect, cursorPoint);
            cursorPoint.X -= windowRect.Left;
            cursorPoint.Y -= windowRect.Top;
            mouseInUpButton = cursorPoint.Y < clipRect.Height / 2;
            using (Graphics g = Graphics.FromHwnd(_upDownButtonWnd))
            {
                NumericUpDownButtonPaintEventArgs e = new NumericUpDownButtonPaintEventArgs(g, clipRect, mouseOver, mousePress, mouseInUpButton);
                _owner.OnPaintNumericUpDownButton(e);
            }
        }

        #endregion 绘制按钮

        #region 监听系统消息绘制

        /// <summary>监听系统消息绘制</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    if (!_bPainting)
                    {
                        _bPainting = true;
                        PAINTSTRUCT ps = new PAINTSTRUCT();
                        WinApi.BeginPaint(m.HWnd, ref ps);
                        DrawUpDownButton();
                        WinApi.EndPaint(m.HWnd, ref ps);
                        _bPainting = false;
                        m.Result = WMRESULT.TRUE;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 监听系统消息绘制

        #region 释放

        /// <summary>释放</summary>
        public void Dispose()
        {
            _owner = null;
            _upDownButton = null;
            base.ReleaseHandle();
        }

        #endregion 释放
    }

    #endregion 数字选择框扩展窗口
}