using Simple.WinUI.Controls;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.WinUI
{
    /// <summary>Windows系统API</summary>
    public class WinApi
    {
        #region 常量

        /// <summary>WWW.CSharpSkin.COM 操作系统API</summary>
        public const int WM_PAINT = 0x000F;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CTLCOLOREDIT = 0x133;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_CREATE = 0x0001;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCMOUSEMOVE = 0x00A0;

        public const int WM_NCHITTEST = 0x0084;

        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTCAPTION = 2;
        public const int HTCLIENT = 1;

        public const int WM_FALSE = 0;
        public const int WM_TRUE = 1;

        #region MOUSEBUTTONSTATE

        public const int LBUTTON = 0x1;
        public const int RBUTTON = 0x2;

        #endregion MOUSEBUTTONSTATE

        #region TBM

        private const int WM_USER = 0x0400;
        private const int WS_HSCROLL = 0x100000;
        private const int WS_VSCROLL = 0x200000;
        private const int GWL_STYLE = (-16);
        public const int TBM_GETRANGEMIN = (WM_USER + 1);
        public const int TBM_GETRANGEMAX = (WM_USER + 2);
        public const int TBM_GETTIC = (WM_USER + 3);
        public const int TBM_SETTIC = (WM_USER + 4);
        public const int TBM_SETPOS = (WM_USER + 5);
        public const int TBM_SETRANGE = (WM_USER + 6);
        public const int TBM_SETRANGEMIN = (WM_USER + 7);
        public const int TBM_SETRANGEMAX = (WM_USER + 8);
        public const int TBM_CLEARTICS = (WM_USER + 9);
        public const int TBM_SETSEL = (WM_USER + 10);
        public const int TBM_SETSELSTART = (WM_USER + 11);
        public const int TBM_SETSELEND = (WM_USER + 12);
        public const int TBM_GETPTICS = (WM_USER + 14);
        public const int TBM_GETTICPOS = (WM_USER + 15);
        public const int TBM_GETNUMTICS = (WM_USER + 16);
        public const int TBM_GETSELSTART = (WM_USER + 17);
        public const int TBM_GETSELEND = (WM_USER + 18);
        public const int TBM_CLEARSEL = (WM_USER + 19);
        public const int TBM_SETTICFREQ = (WM_USER + 20);
        public const int TBM_SETPAGESIZE = (WM_USER + 21);
        public const int TBM_GETPAGESIZE = (WM_USER + 22);
        public const int TBM_SETLINESIZE = (WM_USER + 23);
        public const int TBM_GETLINESIZE = (WM_USER + 24);
        public const int TBM_GETTHUMBRECT = (WM_USER + 25);
        public const int TBM_GETCHANNELRECT = (WM_USER + 26);
        public const int TBM_SETTHUMBLENGTH = (WM_USER + 27);
        public const int TBM_GETTHUMBLENGTH = (WM_USER + 28);
        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_CTL = 2;
        public const int SB_BOTH = 3;
        public const int SW_SHOW = 1;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int PRF_CLIENT = 0x00000004;

        #endregion TBM

        #endregion 常量

        #region 获取设备资源

        /// <summary>获取设备资源</summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);

        #endregion 获取设备资源

        #region 释放设备资源

        /// <summary>释放设备资源</summary>
        /// <param name="hwnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        #endregion 释放设备资源

        #region 获取下拉框信息

        /// <summary>获取下拉框信息</summary>
        /// <param name="hwndCombo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(
            IntPtr hwndCombo, ref COMBOBOXINFO info);

        #endregion 获取下拉框信息

        #region 设置控件指定坐标颜色

        /// <summary>设置控件指定坐标颜色</summary>
        /// <param name="hdc"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="crColor"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);

        #endregion 设置控件指定坐标颜色

        #region 获取鼠标位置

        /// <summary>获取鼠标位置</summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);

        #endregion 获取鼠标位置

        #region 获取键盘状态

        /// <summary>获取键盘状态</summary>
        /// <param name="nVirtKey"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        #endregion 获取键盘状态

        #region 设置主题

        /// <summary>设置主题</summary>
        /// <param name="hWnd"></param>
        /// <param name="pszSubAppName"></param>
        /// <param name="pszSubIdList"></param>
        /// <returns></returns>
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        [DllImport("uxtheme.dll")]
        public static extern bool IsAppThemed();

        [DllImport("user32.dll")]
        public static extern bool PtInRect([In] ref RECT lprc, Point pt);

        #endregion 设置主题

        #region 获取窗口尺寸信息

        /// <summary>获取窗口尺寸信息</summary>
        /// <param name="hwnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        #endregion 获取窗口尺寸信息

        #region 根据窗体名称查找子窗体

        /// <summary>根据窗体名称查找子窗体</summary>
        /// <param name="hWnd1"></param>
        /// <param name="hWnd2"></param>
        /// <param name="lpsz1"></param>
        /// <param name="lpsz2"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        #endregion 根据窗体名称查找子窗体

        #region 窗口是否显示

        /// <summary>窗口是否显示</summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        #endregion 窗口是否显示

        #region 获取指定窗口尺寸

        /// <summary>获取客户端窗口尺寸</summary>
        /// <param name="hWnd"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(
            IntPtr hWnd, ref RECT r);

        #endregion 获取指定窗口尺寸

        #region 获取改变坐标后的矩形

        /// <summary>获取改变坐标后的矩形</summary>
        /// <param name="lpRect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int OffsetRect(
            ref RECT lpRect, int x, int y);

        #endregion 获取改变坐标后的矩形

        #region 获取窗口信息

        /// <summary>获取窗口信息</summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(
            IntPtr hwnd, int nIndex);

        #endregion 获取窗口信息

        #region 设置窗口信息

        /// <summary>设置窗口信息</summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(
            IntPtr hwnd, int nIndex, int dwNewLong);

        #endregion 设置窗口信息

        #region 获取窗口在屏幕的坐标

        /// <summary>获取窗口在屏幕的坐标</summary>
        /// <param name="hWnd"></param>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        #endregion 获取窗口在屏幕的坐标

        #region 指定窗口的客户端区域或者整个屏幕从一个设备上下文(DC)中提取一个句柄

        /// <summary>指定窗口的客户端区域或者整个屏幕从一个设备上下文(DC)中提取一个句柄</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr handle);

        /// <summary>指定窗口的客户端区域或者整个屏幕从一个设备上下文(DC)中提取一个句柄</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDCEx(IntPtr handle);

        #endregion 指定窗口的客户端区域或者整个屏幕从一个设备上下文(DC)中提取一个句柄

        #region 返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。

        /// <summary>返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。</summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        #endregion 返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。

        #region 创建或设置一个定时器，该函数创建的定时器与Timer控件（定时器控件）效果相同。

        /// <summary>创建或设置一个定时器，该函数创建的定时器与Timer控件（定时器控件）效果相同。</summary>
        /// <param name="hWnd"></param>
        /// <param name="nIDEvent"></param>
        /// <param name="uElapse"></param>
        /// <param name="lpTimerFunc"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(
            IntPtr hWnd,
            int nIDEvent,
            uint uElapse,
            IntPtr lpTimerFunc);

        #endregion 创建或设置一个定时器，该函数创建的定时器与Timer控件（定时器控件）效果相同。

        #region 销毁以前调用SetTimer创建的用nIDEvent标识的定时器事件。不能将此定时器有关的未处理的WM_TIMER消息都从消息队列中清除。

        /// <summary>销毁以前调用SetTimer创建的用nIDEvent标识的定时器事件。不能将此定时器有关的未处理的WM_TIMER消息都从消息队列中清除。</summary>
        /// <param name="hWnd"></param>
        /// <param name="uIDEvent"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

        #endregion 销毁以前调用SetTimer创建的用nIDEvent标识的定时器事件。不能将此定时器有关的未处理的WM_TIMER消息都从消息队列中清除。

        #region 对指定的窗口设置键盘焦点

        /// <summary>对指定的窗口设置键盘焦点</summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        #endregion 对指定的窗口设置键盘焦点

        #region 发送消息到指定句柄

        /// <summary>发送消息到指定句柄</summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref RECT lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            [MarshalAs(UnmanagedType.LPTStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        #endregion 发送消息到指定句柄

        #region 更新指定窗口的无效矩形区域，使之有效

        /// <summary>更新指定窗口的无效矩形区域，使之有效</summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        #endregion 更新指定窗口的无效矩形区域，使之有效

        #region 该函数从源矩形中复制一个位图到目标矩形，必要时按目标设备设置的模式进行图像的拉伸或压缩

        /// <summary>该函数从源矩形中复制一个位图到目标矩形，必要时按目标设备设置的模式进行图像的拉伸或压缩</summary>
        /// <param name="hDest"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="sX"></param>
        /// <param name="sY"></param>
        /// <param name="nWidthSrc"></param>
        /// <param name="nHeightSrc"></param>
        /// <param name="dwRop"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(IntPtr hDest, int X, int Y, int nWidth, int nHeight, IntPtr hdcSrc, int sX, int sY, int nWidthSrc, int nHeightSrc, int dwRop);

        #endregion 该函数从源矩形中复制一个位图到目标矩形，必要时按目标设备设置的模式进行图像的拉伸或压缩

        #region 对指定的源设备环境区域中的像素进行位块（bit_block）转换

        /// <summary>对指定的源设备环境区域中的像素进行位块（bit_block）转换</summary>
        /// <param name="hdc"></param>
        /// <param name="nXDest"></param>
        /// <param name="nYDest"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="nXSrc"></param>
        /// <param name="nYSrc"></param>
        /// <param name="dwRop"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        #endregion 对指定的源设备环境区域中的像素进行位块（bit_block）转换

        #region 创建设备

        /// <summary>创建设备</summary>
        /// <param name="lpszDriver"></param>
        /// <param name="lpszDevice"></param>
        /// <param name="lpszOutput"></param>
        /// <param name="lpInitData"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA(
            [MarshalAs(UnmanagedType.LPStr)] string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)] string lpszDevice,
            [MarshalAs(UnmanagedType.LPStr)] string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszDriver,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
            string lpszDriver,
            string lpszDevice,
            string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        #endregion 创建设备

        #region 创建与指定的设备环境相关的设备兼容的位图。

        /// <summary>创建与指定的设备环境相关的设备兼容的位图。</summary>
        /// <param name="hdc"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc, int nWidth, int nHeight);

        #endregion 创建与指定的设备环境相关的设备兼容的位图。

        #region 开始绘制

        /// <summary>开始绘制</summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        #endregion 开始绘制

        #region 结束绘制

        /// <summary>结束绘制</summary>
        /// <param name="hWnd"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        #endregion 结束绘制

        #region 动画相关

        /// <summary>自左向右滚动窗体动画效果</summary>
        public const Int32 AW_HOR_POSITIVE = 0X00000001;

        /// <summary>自右向左滚动窗体动画效果</summary>
        public const Int32 AW_HOR_NEGATIVE = 0X00000002;

        /// <summary>自上向下滚动窗体动画效果</summary>
        public const Int32 AW_VER_POSITIVE = 0X00000004;

        /// <summary>自下向上滚动窗体动画效果</summary>
        public const Int32 AW_VER_NEGATIVE = 0X00000008;

        public const Int32 AW_CENTER = 0X00000010;
        public const Int32 AW_HIDE = 0X00010000;
        public const Int32 AW_ACTIVATE = 0X00020000;
        public const Int32 AW_SLIDE = 0X00040000;
        public const Int32 AW_BLEND = 0X00080000;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        #region 以动画形式显示窗体

        /// <summary>以动画形式显示窗体</summary>
        /// <param name="hwnd"></param>
        public static void WindowsShow(IntPtr hwnd)
        {
            AnimateWindow(hwnd, 100, AW_SLIDE + AW_HOR_POSITIVE);
        }

        #endregion 以动画形式显示窗体

        #region 动画参数说明

        //自左向右滚动窗体动画效果  AnimateWindow(this.Handle,2000,AW_HOR_POSITIVE);
        //"自左向右滑动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_SLIDE+AW_HOR_POSITIVE);
        //"自右向左滚动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_HOR_NEGATIVE);
        //"自右向左滑动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_HOR_NEGATIVE);
        //"自上向下滚动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_VER_POSITIVE);
        //"自上向下滑动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_VER_POSITIVE);
        //"自下向上滚动窗体动画效果"  AnimateWindow(this.Handle, 2000, AW_VER_NEGATIVE);
        //"自下向上滑动窗体动画效果" AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_VER_NEGATIVE);
        //"向外扩展窗体动画效果") AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_CENTER);
        //"淡入窗体动画效果") AnimateWindow(this.Handle, 2000, AW_BLEND);

        #endregion 动画参数说明

        #endregion 动画相关

        #region 删除设备

        /// <summary>删除设备</summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        #endregion 删除设备

        #region 选择一对象到指定的设备上下文环境中，该新对象替换先前的相同类型的对象。

        /// <summary>选择一对象到指定的设备上下文环境中，该新对象替换先前的相同类型的对象。</summary>
        /// <param name="hdc"></param>
        /// <param name="hgdiobj"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        #endregion 选择一对象到指定的设备上下文环境中，该新对象替换先前的相同类型的对象。

        #region 删除对象，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了。

        /// <summary>删除对象，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了。</summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        #endregion 删除对象，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了。

        #region 拖动文件

        /// <summary>拖动文件</summary>
        /// <param name="hDrop"></param>
        /// <param name="iFile"></param>
        /// <param name="lpszFile"></param>
        /// <param name="cch"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern uint DragQueryFile(int hDrop, uint iFile, StringBuilder lpszFile, uint cch);

        #endregion 拖动文件

        #region 接收拖动文件

        /// <summary>接收拖动文件</summary>
        /// <param name="hWnd"></param>
        /// <param name="fAccept"></param>
        [DllImport("shell32.dll")]
        public static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        #endregion 接收拖动文件

        #region 拖动接收

        private const int WM_DROPFILES = 0x0233;

        /// <summary>拖动接收</summary>
        /// <param name="hDrop"></param>
        [DllImport("shell32.dll")]
        public static extern void DragFinish(int hDrop);

        #endregion 拖动接收

        #region 是否优化内存

        /// <summary>是否优化内存</summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize); ////// 释放内存

        #endregion 是否优化内存

        #region 将指定句柄的窗体设置靠最前显示

        /// <summary>将指定句柄的窗体设置靠最前显示</summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion 将指定句柄的窗体设置靠最前显示

        #region 显示窗体

        /// <summary>显示窗体</summary>
        /// <param name="hWnd"></param>
        /// <param name="cmdShow"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        #endregion 显示窗体

        #region 加载鼠标光标

        /// <summary>加载鼠标光标</summary>
        /// <param name="hInstance"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, UCursorType cursor);

        #endregion 加载鼠标光标

        #region 显示滚动条

        /// <summary>显示滚动条</summary>
        /// <param name="hWnd"></param>
        /// <param name="wBar"></param>
        /// <param name="bShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        #endregion 显示滚动条

        #region 返回鼠标Hand光标

        /// <summary>返回鼠标Hand光标</summary>
        public static Cursor Hand
        {
            get
            {
                IntPtr h = LoadCursor(IntPtr.Zero, UCursorType.IDC_HAND);
                return new Cursor(h);
            }
        }

        #endregion 返回鼠标Hand光标

        #region 判断是否出现垂直滚动条

        /// <summary>判断是否出现垂直滚动条</summary>
        /// <param name="ctrl">待测控件</param>
        /// <returns>出现垂直滚动条返回true，否则为false</returns>
        public static bool IsVerticalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;

            return (GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_VSCROLL) != 0;
        }

        #endregion 判断是否出现垂直滚动条

        #region 判断是否出现水平滚动条

        /// <summary>判断是否出现水平滚动条</summary>
        /// <param name="ctrl">待测控件</param>
        /// <returns>出现水平滚动条返回true，否则为false</returns>
        public static bool IsHorizontalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;
            return (GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_HSCROLL) != 0;
        }

        #endregion 判断是否出现水平滚动条

        #region 向指定句柄发送消息

        /// <summary>向指定句柄发送消息</summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #endregion 向指定句柄发送消息

        #region 返回当前系统是否是XP

        /// <summary>返回当前系统是否是XP</summary>
        public static bool IsXPOS
        {
            get
            {
                OperatingSystem operatingSystem = Environment.OSVersion;
                return (operatingSystem.Platform == PlatformID.Win32NT) &&
                    ((operatingSystem.Version.Major > 5) || ((operatingSystem.Version.Major == 5) && (operatingSystem.Version.Minor == 1)));
            }
        }

        #endregion 返回当前系统是否是XP

        #region 返回当前系统是否是Visita

        /// <summary>返回当前系统是否是Visita</summary>
        public static bool IsVistaOS
        {
            get
            {
                OperatingSystem operatingSystem = Environment.OSVersion;
                return (operatingSystem.Platform == PlatformID.Win32NT) && (operatingSystem.Version.Major >= 6);
            }
        }

        #endregion 返回当前系统是否是Visita

        #region shell32

        /// <summary>释放IntPt</summary>
        /// <param name="pidlList">要释放IntPt</param>
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern void ILFree(IntPtr pidlList);

        /// <summary>创建窗体</summary>
        /// <param name="pszPath">文件全路径</param>
        /// <returns>IntPtr对象</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr ILCreateFromPathW(String pszPath);

        /// <summary>打开指定路径并选中项</summary>
        /// <param name="pidlList">指向指定文件夹的完全限定项目ID列表的指针</param>
        /// <param name="cild">选择数组中的项目数</param>
        /// <param name="children">指向PIDL结构数组的指针</param>
        /// <param name="dwFlags">可选标志</param>
        /// <returns>Int32数值</returns>
        [DllImport("shell32.dll", ExactSpelling = true)]
        public static extern Int32 SHOpenFolderAndSelectItems(IntPtr pidlList, UInt32 cild, IntPtr children, UInt32 dwFlags);

        #endregion shell32

        #region kernel32

        /// <summary>获取当前线程</summary>
        /// <returns>IntPtr对象</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        /// <summary>查询线程周期时间</summary>
        /// <param name="threadHandle">线程对象</param>
        /// <param name="cycleTime">周期时间</param>
        /// <returns>Boolean值</returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean QueryThreadCycleTime(IntPtr threadHandle, ref UInt64 cycleTime);

        #endregion kernel32

        #region 该函数设置由不同线程产生的窗口的显示状态

        /// <summary>该函数设置由不同线程产生的窗口的显示状态。</summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns>
        [DllImport("user32.dll")]
        public static extern Boolean ShowWindowAsync(IntPtr hWnd, Int32 cmdShow);

        #endregion 该函数设置由不同线程产生的窗口的显示状态

        #region 窗体边框阴影效果变量申明

        public const int CS_DropSHADOW = 0x20000;
        public const int GCL_STYLE = (-26);

        // 声明Win32 API
        [DllImport("user32.dll ", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll ", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        #endregion 窗体边框阴影效果变量申明

        #region 一些转换

        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        #endregion 一些转换
    }
}