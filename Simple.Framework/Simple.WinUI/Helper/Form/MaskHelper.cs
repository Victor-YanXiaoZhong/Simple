using Simple.WinUI.Forms;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace Simple.WinUI.Helper
{
    /// <summary>加载等待蒙版</summary>
    [Description("加载等待蒙版")]
    public static class MaskHelper
    {
        #region 字段

        private static object maskingClassList_object = new object();

        private static List<MaskingClass> maskingClassList = new List<MaskingClass>();

        private static MaskingKeyHook maskingKeyHook = new MaskingKeyHook();

        /// <summary>旋转定时器</summary>
        private static Timer rotateTimer = new Timer();

        #endregion 字段

        static MaskHelper()
        {
            maskingKeyHook.FormList = maskingClassList;
            rotateTimer.Interval = 20;
            rotateTimer.Tick += rotateTimer_Tick;
        }

        #region 公开方法

        /// <summary>加载等待条风格类型</summary>
        [Description("加载等待条风格类型")]
        public enum MaskingLoadStyles
        {
            /// <summary>直线</summary>
            Line,

            /// <summary>圆球</summary>
            Dot
        }

        /// <summary>文本方位</summary>
        [Description("文本方位")]
        public enum MaskingTextOrientations
        {
            /// <summary>右边</summary>
            Right,

            /// <summary>底部</summary>
            Bottom
        }

        /// <summary>为加载等待蒙版所属的控件树添加事件</summary>
        /// <param name="control"></param>
        /// <param name="mc"></param>
        private static void AddSLEvent(Control control, MaskingClass mc)
        {
            mc.event_control.Add(control);
            control.Resize += mc.masking_form.Owner_Resize;
            control.LocationChanged += mc.masking_form.Owner_LocationChanged;
            control.VisibleChanged += mc.masking_form.Owner_VisibleChanged;
            control.ParentChanged += mc.masking_form.Owner_ParentChanged;
            if (control.Parent != null)
            {
                AddSLEvent(control.Parent, mc);
            }
        }

        /// <summary>为加载等待蒙版所属的控件树移除事件</summary>
        /// <param name="control"></param>
        /// <param name="mc"></param>
        /// <param name="isfirst"></param>
        private static void RemoveSLEvent(Control control, MaskingClass mc, bool isfirst = true)
        {
            mc.event_control.Add(control);
            control.Resize -= mc.masking_form.Owner_Resize;
            control.LocationChanged -= mc.masking_form.Owner_LocationChanged;
            control.VisibleChanged -= mc.masking_form.Owner_VisibleChanged;
            control.ParentChanged -= mc.masking_form.Owner_ParentChanged;
            if (control.Parent != null)
            {
                RemoveSLEvent(control.Parent, mc, false);
            }
            if (isfirst)
            {
                mc.event_control.Clear();
            }
        }

        private static void rotateTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < maskingClassList.Count; i++)
            {
                maskingClassList[i].masking_form.MaskingSetting.AccumulationTime += rotateTimer.Interval;
                if (maskingClassList[i].masking_form.MaskingSetting.AccumulationTime >= 100)
                {
                    maskingClassList[i].masking_form.MaskingSetting.AccumulationTime = 0;

                    maskingClassList[i].masking_form.MaskingSetting.Progress += 1;
                    if (maskingClassList[i].masking_form.MaskingSetting.Progress > maskingClassList[i].masking_form.MaskingSetting.LineDotNumber - 1)
                    {
                        maskingClassList[i].masking_form.MaskingSetting.Progress -= maskingClassList[i].masking_form.MaskingSetting.LineDotNumber - 1;
                    }
                    maskingClassList[i].masking_form.InvalidateLayer();
                }
            }
        }

        /// <summary>显示加载等待蒙版</summary>
        /// <param name="form">窗体</param>
        public static void Show(Form form)
        {
            Show(form, new MaskingSettings());
        }

        /// <summary>显示加载等待蒙版</summary>
        /// <param name="form">窗体</param>
        /// <param name="form">文本</param>
        public static void Show(Form form, string text)
        {
            Show(form, new MaskingSettings() { Text = text });
        }

        /// <summary>显示加载等待蒙版</summary>
        /// <param name="form">窗体</param>
        /// <param name="ms">配置</param>
        public static void Show(Form form, MaskingSettings ms)
        {
            Hide(form);

            ms.InitializeAngles();
            ms.InitializeColors();

            MaskingForm mp = new MaskingForm();
            mp.MaskingSetting = ms;

            MaskingClass mc = new MaskingClass();
            mc.owner_form = form;
            mc.masking_form = mp;

            AddSLEvent(mc.owner_form, mc);

            lock (maskingClassList_object)
            {
                maskingClassList.Add(mc);
            }
            mp.Show(form);
            mp.StartPosition = FormStartPosition.CenterParent;
            Size size = MaskingForm.GetMaskingSize(mc.owner_form);
            Point point = MaskingForm.GetMaskingLocation(mc.owner_form);
            mp.SetBounds(point.X, point.Y, size.Width, size.Height);
            mp.Location = point;
            maskingKeyHook.HookStart();
            if (rotateTimer.Enabled == false)
            {
                rotateTimer.Enabled = true;
            }
        }

        /// <summary>隐藏加载等待蒙版</summary>
        /// <param name="form">窗体</param>
        public static void Hide(Form form)
        {
            MaskingClass mc = maskingClassList.Where(a => a.owner_form == form).FirstOrDefault();
            if (mc != null)
            {
                RemoveSLEvent(mc.owner_form, mc);

                mc.masking_form.Hide();
                mc.masking_form.Dispose();

                lock (maskingClassList_object)
                {
                    maskingClassList.Remove(mc);
                    //移除加载等待蒙版所属窗体为null的选项
                    List<MaskingClass> mcList = maskingClassList.Where(a => a.owner_form == null).ToList<MaskingClass>();
                    for (int i = 0; i < mcList.Count; i++)
                    {
                        if (mc.masking_form != null)
                        {
                            mc.masking_form.Hide();
                            mc.masking_form.Dispose();
                        }
                        maskingClassList.Remove(mcList[i]);
                    }

                    if (maskingClassList.Count < 1)
                    {
                        rotateTimer.Enabled = false;
                        maskingKeyHook.HookStop();
                    }
                }
            }
        }

        /// <summary>更改加载等待蒙版文本</summary>
        /// <param name="form">窗体</param>
        public static void UpdateText(Form form, string text)
        {
            MaskingClass mc = maskingClassList.Where(a => a.owner_form == form).FirstOrDefault();
            if (mc != null)
            {
                mc.masking_form.MaskingSetting.Text = text;
                mc.masking_form.InvalidateLayer();
            }
        }

        /// <summary>清除所有加载等待蒙版</summary>
        public static void Clear()
        {
            lock (maskingClassList_object)
            {
                foreach (MaskingClass mc in maskingClassList)
                {
                    RemoveSLEvent(mc.owner_form, mc);

                    mc.masking_form.Hide();
                    mc.masking_form.Dispose();
                }
                maskingClassList.Clear();

                rotateTimer.Enabled = false;
                maskingKeyHook.HookStop();
            }
        }

        /// <summary>根据蒙版窗体获取蒙版信息</summary>
        /// <param name="form">蒙版窗体</param>
        /// <returns></returns>
        public static MaskingClass GetMasking(MaskingForm form)
        {
            return maskingClassList.Where(a => a.masking_form == form).FirstOrDefault();
        }

        /// <summary>根据控件获取MaskingClass信息</summary>
        /// <param name="form">拥有窗体</param>
        /// <returns></returns>
        public static List<MaskingClass> GetMaskingClass(Control control)
        {
            return maskingClassList.Where(a => a.event_control.Contains(control)).ToList<MaskingClass>();
        }

        /// <summary>为加载等待蒙版所属的控件树更新事件</summary>
        /// <param name="control"></param>
        /// <param name="mc"></param>
        /// <param name="controlList"></param>
        public static void UpdateSLEvent(Control control, MaskHelper.MaskingClass mc, List<Control> controlList)
        {
            controlList.Add(control);
            control.Resize += mc.masking_form.Owner_Resize;
            control.LocationChanged += mc.masking_form.Owner_LocationChanged;
            control.VisibleChanged += mc.masking_form.Owner_VisibleChanged;
            control.ParentChanged += mc.masking_form.Owner_ParentChanged;
            if (control.Parent != null)
            {
                UpdateSLEvent(control.Parent, mc, controlList);
            }
        }

        #endregion 公开方法

        #region 私有方法
        #endregion 私有方法

        #region 类

        /// <summary>蒙版信息</summary>
        [Description("蒙版信息")]
        public class MaskingClass
        {
            public MaskingClass()
            {
                this.event_control = new List<Control>();
            }

            /// <summary>蒙版所属的窗体</summary>
            public Form owner_form { get; set; }

            /// <summary>蒙版窗体</summary>
            public MaskingForm masking_form { get; set; }

            /// <summary>要监听事件的控件列表</summary>
            public List<Control> event_control { get; set; }
        }

        /// <summary>加载等待界面配置</summary>
        [Description("加载等待界面配置")]
        public class MaskingSettings
        {
            #region

            private List<double> lineAngles = new List<double>();
            private List<Color> lineColors = new List<Color>();

            private int progress = 0;

            private double accumulationTime = 0;

            private MaskingLoadStyles loadStyle = MaskingLoadStyles.Line;

            private Color backColor = Color.FromArgb(50, 0, 0, 0);

            private int lineDotCircleRadius = 16;

            private int lineDotNumber = 8;

            private string text = "加载中...";

            private MaskingTextOrientations textOrientation = MaskingTextOrientations.Bottom;

            private Font textFont = new Font("宋体", 11, FontStyle.Regular);

            private Color textColor = Color.White;

            private int brushThickness = 4;

            private int brushLenght = 8;

            private Color brushColor = Color.White;

            /// <summary>直线或圆点的角度</summary>
            [Description("直线或圆点的角度")]
            [DefaultValue(8)]
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public List<double> LineAngles
            {
                get { return this.lineAngles; }
                set
                {
                    if (this.lineAngles == value)
                        return;

                    this.lineAngles = value;
                }
            }

            /// <summary>直线或圆点的颜色</summary>
            [Description("直线或圆点的颜色")]
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public List<Color> LineColors
            {
                get { return this.lineColors; }
                set
                {
                    if (this.lineColors == value)
                        return;

                    this.lineColors = value;
                }
            }

            /// <summary>旋转进度条值</summary>
            [Description("旋转进度条值")]
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public int Progress
            {
                get { return this.progress; }
                set
                {
                    if (this.progress == value)
                        return;

                    this.progress = value;
                }
            }

            /// <summary>旋转动画已累加的时间</summary>
            [Description("旋转动画已累加的时间")]
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public double AccumulationTime
            {
                get { return this.accumulationTime; }
                set
                {
                    if (this.accumulationTime == value)
                        return;

                    this.accumulationTime = value;
                }
            }

            #endregion 类

            #region

            /// <summary>加载等待条风格类型</summary>
            [Description("加载等待条风格类型")]
            [DefaultValue(typeof(MaskingLoadStyles), "Line")]
            public MaskingLoadStyles LoadStyle
            {
                get { return this.loadStyle; }
                set
                {
                    if (this.loadStyle == value)
                        return;

                    this.loadStyle = value;
                    this.InitializeAngles();
                    this.InitializeColors();
                }
            }

            /// <summary>背景颜色</summary>
            [DefaultValue(typeof(Color), "50, 0, 0, 0")]
            [Description("背景颜色")]
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            public Color BackColor
            {
                get { return this.backColor; }
                set
                {
                    if (this.backColor == value)
                        return;

                    this.backColor = value;
                }
            }

            /// <summary>绘制直线或圆点画板半径大小</summary>
            [Description("绘制直线或圆点画板半径大小")]
            [DefaultValue(16)]
            public int LineDotCircleRadius
            {
                get
                {
                    return this.lineDotCircleRadius;
                }
                set
                {
                    if (this.lineDotCircleRadius == value && value < 1)
                        return;
                    this.lineDotCircleRadius = value;
                }
            }

            /// <summary>直线或圆点的数量</summary>
            [Description("直线或圆点的数量")]
            [DefaultValue(8)]
            [Browsable(false)]
            public int LineDotNumber
            {
                get { return this.lineDotNumber; }
                set
                {
                    if (this.lineDotNumber == value)
                        return;

                    this.lineDotNumber = value;
                    this.InitializeAngles();
                    this.InitializeColors();
                }
            }

            #endregion

            #region 文本

            /// <summary>文本</summary>
            [Description("文本")]
            [DefaultValue("加载中...")]
            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    if (this.text == value)
                        return;
                    this.text = value;
                }
            }

            /// <summary>文本方位</summary>
            [Description("文本方位")]
            [DefaultValue(MaskingTextOrientations.Bottom)]
            public MaskingTextOrientations TextOrientation
            {
                get { return this.textOrientation; }
                set
                {
                    if (this.textOrientation == value)
                        return;

                    this.textOrientation = value;
                }
            }

            /// <summary>文本字体</summary>
            [Description("文本字体")]
            public Font TextFont
            {
                get
                {
                    return this.textFont;
                }
                set
                {
                    if (this.textFont == value)
                        return;
                    this.textFont = value;
                }
            }

            /// <summary>文本颜色</summary>
            [DefaultValue(typeof(Color), "White")]
            [Description("文本颜色")]
            [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
            public Color TextColor
            {
                get { return this.textColor; }
                set
                {
                    if (this.textColor == value)
                        return;

                    this.textColor = value;
                }
            }

            #endregion

            #region 画笔

            /// <summary>画笔粗细程度</summary>
            [Description("画笔粗细程度")]
            [DefaultValue(4)]
            public int BrushThickness
            {
                get
                {
                    return this.brushThickness;
                }
                set
                {
                    if (this.brushThickness == value || value < 1)
                        return;
                    this.brushThickness = value;
                }
            }

            /// <summary>画笔长度(仅限Line)</summary>
            [Description("画笔长度(仅限Line)")]
            [DefaultValue(8)]
            public int BrushLenght
            {
                get
                {
                    return this.brushLenght;
                }
                set
                {
                    if (this.brushLenght == value)
                        return;
                    this.brushLenght = value;
                }
            }

            /// <summary>画笔颜色</summary>
            [Description("画笔颜色")]
            [DefaultValue(typeof(Color), "White")]
            public Color BrushColor
            {
                get
                {
                    return this.brushColor;
                }
                set
                {
                    if (this.brushColor == value)
                        return;
                    this.brushColor = value;
                    if (this.LoadStyle == MaskingLoadStyles.Line || this.LoadStyle == MaskingLoadStyles.Dot)
                    {
                        this.InitializeColors();
                    }
                }
            }

            #endregion

            #region 公开方法

            /// <summary>初始化每条直线或圆点角度</summary>
            /// <returns></returns>
            public void InitializeAngles()
            {
                this.LineAngles = new List<double>();
                double avgAngle = 360d / (double)this.LineDotNumber;
                for (int i = 0; i < this.LineDotNumber; i++)
                {
                    this.LineAngles.Add((i + 1) * avgAngle);
                }
            }

            /// <summary>初始化每条直线或圆点颜色</summary>
            /// <returns></returns>
            public void InitializeColors()
            {
                this.LineColors = new List<Color>();

                byte transparent = 0;//颜色透明度
                byte transparentIncrement = (byte)(byte.MaxValue / this.LineDotNumber);//颜色透明度增量
                for (int i = 0; i < this.LineDotNumber; i++)
                {
                    Color color = this.BrushColor;
                    if (i > 0)
                    {
                        transparent += transparentIncrement;
                        transparent = Math.Min(transparent, byte.MaxValue);
                        color = Color.FromArgb(transparent, Math.Min(this.BrushColor.R, byte.MaxValue), Math.Min(this.BrushColor.G, byte.MaxValue), Math.Min(this.BrushColor.B, byte.MaxValue));
                    }
                    this.LineColors.Add(color);
                }
            }

            #endregion
        }

        /// <summary>加载等待蒙版钩子</summary>
        [Description("加载等待蒙版钩子")]
        public class MaskingKeyHook : IDisposable
        {
            #region 结构

            private HookProc KeyDownHookProcedure;

            private int keyDownHookStatus = 0;

            private List<Keys> keyList = new List<Keys>();

            private List<MaskingClass> formList = null;

            /// <summary>是否回收完毕</summary>
            protected bool _disposed;

            internal const int WH_KEYBOARD_LL = 13;

            //键盘事件
            internal const int WM_KEYDOWN = 0X0100;

            //键按下
            internal const int WM_SYSKEYDOWN = 0X0104;

            internal const int WM_KEYUP = 0x0101;

            //键按下释放
            internal const int WM_SYSKEYUP = 0x0105;

            internal const byte VK_SHIFT = 0x10;

            internal const byte VK_CONTROL = 0x11;

            internal const byte VK_MENU = 0x12;

            ~MaskingKeyHook()
            {
                Dispose(false);
            }

            /// <summary>钩子回调函数</summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

            /// <summary>出现加载等待蒙版的窗体集合</summary>
            [DefaultValue(null)]
            [Description("出现加载等待蒙版的窗体集合")]
            public List<MaskingClass> FormList
            {
                get { return formList; }
                set
                {
                    if (formList == value)
                        return;

                    formList = value;
                }
            }

            /// <summary>键盘处理事件</summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            private int KeyDownHookProc(int nCode, Int32 wParam, IntPtr lParam)
            {
                bool is_ctrl = ((GetKeyState(VK_CONTROL) & 0x80) != 0);
                bool is_shift = ((GetKeyState(VK_SHIFT) & 0x80) != 0);
                bool is_alt = ((GetKeyState(VK_MENU) & 0x80) != 0);

                KeyHookStruct keyHookStruct = (KeyHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyHookStruct));//获取钩子的相关信息
                KeyEventArgs e = new KeyEventArgs((Keys)(//获取KeyEventArgs事件的相关信息
                                           keyHookStruct.vkCode |
                                           (is_ctrl ? (int)Keys.Control : 0) |
                                           (is_shift ? (int)Keys.Shift : 0) |
                                           (is_alt ? (int)Keys.Alt : 0)
                                           ));

                #region 禁用 Tab键
                IntPtr activeHandle = GetActiveWindow();
                if (e.KeyCode == Keys.Tab && this.formList != null && this.formList.Where(a => a.owner_form.Handle == activeHandle).Count() > 0)
                {
                    return 1;
                }
                #endregion

                // 启动下一次钩子
                return CallNextHookEx(this.keyDownHookStatus, nCode, wParam, lParam);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)//如果已经被回收，就中断执行
                    return;

                if (disposing)
                {
                    this.HookStop();
                    _disposed = true;
                }
            }

            /// <summary>安装钩子</summary>
            /// <param name="idHook"></param>
            /// <param name="lpfn"></param>
            /// <param name="hInstance"></param>
            /// <param name="threadId"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

            /// <summary>卸载钩子</summary>
            /// <param name="idHook"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern bool UnhookWindowsHookEx(int idHook);

            /// <summary>调用下一个钩子</summary>
            /// <param name="idHook"></param>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

            /// <summary>获取指定修改键状态</summary>
            /// <param name="vKey"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern short GetKeyState(int vKey);

            /// <summary>获取模块句柄</summary>
            /// <param name="name"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetModuleHandle(string name);

            /// <summary>获取当前激活的窗体</summary>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetActiveWindow();

            /// <summary>安装钩子</summary>
            public void HookStart()
            {
                if (this.keyDownHookStatus == 0)
                {
                    this.keyList.Clear();
                    this.KeyDownHookProcedure = new HookProc(this.KeyDownHookProc);
                    this.keyDownHookStatus = SetWindowsHookEx(WH_KEYBOARD_LL, this.KeyDownHookProcedure, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                }
            }

            /// <summary>取消钩子</summary>
            public void HookStop()
            {
                if (this.keyDownHookStatus != 0)
                {
                    UnhookWindowsHookEx(this.keyDownHookStatus);
                    this.keyDownHookStatus = 0;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>键盘钩子返回信息结构体</summary>
            [StructLayout(LayoutKind.Sequential)]
            public class KeyHookStruct
            {
                public int vkCode;
                public int scanCode;
                public int flags;
                public int time;
                public int dwExtraInfo;
            }

            #endregion

            #region 扩展
            #endregion

            #region 新增字段
            #endregion

            #region 新增属性
            #endregion

            #region 公开方法
            #endregion

            #region 私有方法
            #endregion

            #region 释放
            #endregion
        }

        #endregion

        #region 枚举
        #endregion
    }

    /// <summary>蒙版窗体</summary>
    [Description("蒙版窗体")]
    public class MaskingForm : Form
    {
        #region 扩展

        /// <summary>窗户是分层的窗户。如果窗口中有一个不能用这种风格类样式之一CS_OWNDC或CS_CLASSDC。Windows 8的：该WS_EX_LAYERED样式支持顶级窗口和子窗口。以前的Windows版本仅对顶级窗口支持WS_EX_LAYERED。</summary>
        private const int WS_EX_LAYERED = 0x80000;

        private const byte AC_SRC_OVER = 0;
        private const byte AC_SRC_ALPHA = 1;

        /// <summary>使用pblend作为混合功能。如果显示模式为256色或更小，则此值的效果与ULW_OPAQUE的效果相同。</summary>
        private const Int32 ULW_ALPHA = 0x00000002;

        private MaskHelper.MaskingSettings _MaskingSetting = new MaskHelper.MaskingSettings();

        /// <summary>窗体句柄创建完成</summary>
        private bool isHandleCreate = false;

        public MaskingForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        public MaskingForm(Form owen)
        {
            Parent = owen;
            StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= WS_EX_LAYERED;
                return cParms;
            }
        }

        /// <summary>加载等待界面配置</summary>
        [Description("加载等待界面配置")]
        public MaskHelper.MaskingSettings MaskingSetting
        {
            get { return this._MaskingSetting; }
            set
            {
                if (this._MaskingSetting == value)
                    return;

                this._MaskingSetting = value;
            }
        }

        /// <summary>绘制图片到分层控件上</summary>
        /// <param name="bitmap"></param>
        private void DrawImageToLayer()
        {
            #region 绘制界面
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(bitmap);
            this.DrawMasking(g);
            #endregion

            IntPtr hdcDst = GetDC(this.Handle);
            IntPtr hdcSrc = CreateCompatibleDC(hdcDst);

            IntPtr newBitmap = bitmap.GetHbitmap(Color.FromArgb(0));//创建一张位图
            IntPtr oldBitmap = SelectObject(hdcSrc, newBitmap);//位图绑定到DC设备上

            Point pptDst = new Point(this.Left, this.Top);
            Size psize = new Size(bitmap.Width, bitmap.Height);
            Point pptSrc = new Point(0, 0);

            BLENDFUNCTION pblend = new BLENDFUNCTION();
            pblend.BlendOp = AC_SRC_OVER;
            pblend.SourceConstantAlpha = 255;
            pblend.AlphaFormat = AC_SRC_ALPHA;
            pblend.BlendFlags = 0;

            UpdateLayeredWindow(this.Handle, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, 0, ref pblend, ULW_ALPHA);

            if (oldBitmap != IntPtr.Zero)
            {
                if (oldBitmap != IntPtr.Zero)
                    DeleteObject(newBitmap);
                if (oldBitmap != IntPtr.Zero)
                    DeleteObject(newBitmap);
            }
            if (g != null)
                g.Dispose();
            if (bitmap != null)
                bitmap.Dispose();
            ReleaseDC(this.Handle, hdcDst);
            DeleteDC(hdcSrc);
        }

        /// <summary>绘制蒙版内容</summary>
        /// <param name="g"></param>
        private void DrawMasking(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            #region 背景

            SolidBrush back_sb = new SolidBrush(this.MaskingSetting.BackColor);
            g.FillRectangle(back_sb, g.VisibleClipBounds);
            back_sb.Dispose();

            #endregion

            #region 图案

            switch (this.MaskingSetting.LoadStyle)
            {
                case MaskHelper.MaskingLoadStyles.Line:
                    {
                        #region
                        int node = this.MaskingSetting.Progress;
                        Pen line_pen = new Pen(Color.Transparent, this.MaskingSetting.BrushThickness);
                        for (int i = 0; i < this.MaskingSetting.LineDotNumber; i++)
                        {
                            node = node % this.MaskingSetting.LineDotNumber;
                            PointF point1 = this.GetCoordinate(this.ClientRectangle, this.MaskingSetting.LineDotCircleRadius - this.MaskingSetting.BrushLenght, this.MaskingSetting.LineAngles[node]);
                            PointF point2 = this.GetCoordinate(this.ClientRectangle, this.MaskingSetting.LineDotCircleRadius, this.MaskingSetting.LineAngles[node]);

                            line_pen.StartCap = LineCap.Round;
                            line_pen.EndCap = LineCap.Round;
                            line_pen.Color = this.MaskingSetting.LineColors[i];
                            g.DrawLine(line_pen, point1, point2);

                            node++;
                        }
                        line_pen.Dispose();
                        break;
                        #endregion
                    }
                case MaskHelper.MaskingLoadStyles.Dot:
                    {
                        #region
                        int node = this.MaskingSetting.Progress;
                        SolidBrush dot_sb = new SolidBrush(Color.Transparent);
                        for (int i = 0; i < this.MaskingSetting.LineDotNumber; i++)
                        {
                            node = node % this.MaskingSetting.LineDotNumber;
                            PointF point = this.GetCoordinate(this.ClientRectangle, this.MaskingSetting.LineDotCircleRadius, this.MaskingSetting.LineAngles[node]);

                            dot_sb.Color = this.MaskingSetting.LineColors[i];
                            g.FillEllipse(dot_sb, point.X - this.MaskingSetting.BrushThickness / 2, point.Y - this.MaskingSetting.BrushThickness / 2, this.MaskingSetting.BrushThickness, this.MaskingSetting.BrushThickness);

                            node++;
                        }
                        dot_sb.Dispose();
                        break;
                        #endregion
                    }
            }

            #endregion

            #region 文本

            if (!String.IsNullOrWhiteSpace(this.MaskingSetting.Text))
            {
                SolidBrush text_sb = new SolidBrush(this.MaskingSetting.TextColor);
                StringFormat text_sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far, Trimming = StringTrimming.EllipsisCharacter };
                SizeF text_sizef = g.MeasureString(this.MaskingSetting.Text, this.MaskingSetting.TextFont);

                Point center = new Point(this.ClientRectangle.X + this.ClientRectangle.Width / 2, this.ClientRectangle.Y + this.ClientRectangle.Height / 2);
                Rectangle text_rect = new Rectangle(center.X - (int)(text_sizef.Width / 2), center.Y + this.MaskingSetting.LineDotCircleRadius + this.MaskingSetting.BrushThickness + 4, (int)text_sizef.Width, (int)text_sizef.Height);
                if (this.MaskingSetting.TextOrientation == MaskHelper.MaskingTextOrientations.Right)
                {
                    text_rect = new Rectangle(center.X + this.MaskingSetting.LineDotCircleRadius + this.MaskingSetting.BrushThickness, center.Y, (int)text_sizef.Width, (int)text_sizef.Height);
                }
                g.DrawString(this.MaskingSetting.Text, this.MaskingSetting.TextFont, text_sb, text_rect, text_sf);
                text_sb.Dispose();
                text_sf.Dispose();
            }

            #endregion
        }

        /// <summary>已绘图区中心坐标根据角度获取圆环上对应坐标</summary>
        /// <param name="rect">绘图区</param>
        /// <param name="radius">圆环半径</param>
        /// <param name="angle">角度</param>
        /// <returns>圆环上对应坐标</returns>
        private PointF GetCoordinate(Rectangle rect, int radius, double angle)
        {
            //经纬度转化成弧度(弧度=经纬度 * Math.PI / 180d);
            double dblAngle = angle * Math.PI / 180d;
            return new PointF(rect.Width / 2 + radius * (float)Math.Cos(dblAngle), rect.Height / 2 + radius * (float)Math.Sin(dblAngle));
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);

            base.OnHandleCreated(e);
            isHandleCreate = true;
            this.InvalidateLayer();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                this.InvalidateLayer();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.InvalidateLayer();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.InvalidateLayer();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            MaskHelper.MaskingClass mc = MaskHelper.GetMasking(this);
            if (mc != null && mc.owner_form != null)
            {
                mc.owner_form.Activate();
            }
        }

        /// <summary>检索句柄用于指定窗口的客户区或整个屏幕的设备上下文（DC）。您可以在后续的GDI函数中使用返回的句柄来绘制DC。设备上下文是一个不透明的数据结构，其值由GDI内部使用。</summary>
        /// <param name="hWnd">要获取其DC的窗口的句柄。如果此值为NULL，则GetDC检索整个屏幕的DC。</param>
        /// <returns>如果函数成功，则返回值是指定窗口的客户区DC的句柄。如果函数失败，则返回值为NULL。</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>创建具有指定设备兼容的存储器设备上下文（DC）。</summary>
        /// <param name="hdc">现有DC的句柄。如果此句柄为NULL，则该函数将创建一个与应用程序当前屏幕兼容的内存DC。</param>
        /// <returns>如果函数成功，则返回值是内存DC的句柄。如果函数失败，则返回值为NULL。</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>释放设备上下文（DC），释放它，以供其他应用程序使用。ReleaseDC功能的效果取决于DC的类型。它仅释放公共DC和窗口DC。它对班级或专用DC无效。</summary>
        /// <param name="hWnd">要释放其DC的窗口的句柄。</param>
        /// <param name="hDC">要释放的DC的句柄。</param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>删除指定的设备上下文（DC）。。</summary>
        /// <param name="hdc">设备上下文的句柄。</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hdc);

        /// <summary>选择一个对象到指定的设备上下文（DC）。新对象将替换相同类型的先前对象。</summary>
        /// <param name="hdc">DC的句柄。</param>
        /// <param name="h">
        /// 要选择的对象的句柄。必须使用以下功能之一创建指定的对象。
        /// CreateBitmap，CreateBitmapIndirect，CreateCompatibleBitmap，CreateDIBitmap，CreateDIBSection
        /// 位图只能选择到存储器DC中。单个位图不能同时选择到多个DC中。
        /// CreateBrushIndirect，CreateDIBPatternBrush，CreateDIBPatternBrushPt，CreateHatchBrush，CreatePatternBrush，CreateSolidBrush
        /// CreateFont，CreateFontIndirect 创建笔，创建笔间接 CombineRgn，CreateEllipticRgn，CreateEllipticRgnIndirect，CreatePolygonRgn，CreateRectRgn，CreateRectRgnIndirect
        /// </param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

        /// <summary>删除逻辑笔，刷子，字体，位图，区域或调色板，释放与该对象相关联的所有系统资源。删除对象后，指定的句柄不再有效。</summary>
        /// <param name="ho">逻辑笔，画笔，字体，位图，区域或调色板的句柄。</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr ho);

        /// <summary>更新分层窗口的位置，大小，形状，内容和半透明。</summary>
        /// <param name="hWnd">
        /// 分层窗口的句柄。使用CreateWindowEx函数创建窗口时，可以通过指定WS_EX_LAYERED来创建分层窗口。Windows 8的： 该WS_EX_LAYERED样式支持顶级窗口和子窗口。以前的Windows版本仅对顶级窗口支持WS_EX_LAYERED。
        /// </param>
        /// <param name="hdcDst">屏幕DC的句柄。通过在调用函数时指定NULL可获得此句柄。当窗口内容更新时，它用于调色板颜色匹配。如果hdcDst为NULL，则将使用默认调色板。如果hdcSrc为NULL，则hdcDst必须为NULL。</param>
        /// <param name="pptDst">指向指定分层窗口的新屏幕位置的结构的指针。如果当前位置未更改，则pptDst可以为NULL。</param>
        /// <param name="psize">指向指定分层窗口新大小的结构的指针。如果窗口的大小未更改，则psize可以为NULL。如果hdcSrc为NULL，则psize必须为NULL。</param>
        /// <param name="hdcSrc">DC的句柄，用于定义分层窗口的表面。可以通过调用CreateCompatibleDC函数获得此句柄。如果窗口的形状和视觉上下文未更改，则hdcSrc可以为NULL。</param>
        /// <param name="pptSrc">指向指定设备上下文中层位置的结构的指针。如果hdcSrc为NULL，则pptSrc应该为NULL。</param>
        /// <param name="crKey">一个结构，用于指定在组成分层窗口时要使用的颜色键。要生成COLORREF，请使用RGB宏。</param>
        /// <param name="pblend">指向结构的指针，该结构指定组成分层窗口时要使用的透明度值。</param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        /// <summary>获取蒙版Size</summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Size GetMaskingSize(Form form)
        {
            Size size = Size.Empty;
            if (form is IBaseForm)
            {
                BaseForm fe = (BaseForm)form;
                size = new Size(form.ClientRectangle.Size.Width - fe.BorderWidth * 2, fe.ClientRectangle.Size.Height - fe.BorderWidth * 2);
            }
            else
            {
                size = form.ClientRectangle.Size;
            }
            return size;
        }

        /// <summary>获取蒙版Location</summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Point GetMaskingLocation(Form form)
        {
            Point point = Point.Empty;
            if (form is IBaseForm)
            {
                BaseForm fe = (BaseForm)form;
                point = form.PointToScreen(new Point(form.ClientRectangle.X + fe.BorderWidth, fe.ClientRectangle.Y + fe.BorderWidth));
            }
            else
            {
                point = form.PointToScreen(form.ClientRectangle.Location);
            }
            return point;
        }

        /// <summary>使分层控件的整个图面无效并导致重绘控件（Invalidate已失效）。</summary>
        public void InvalidateLayer()
        {
            if (this.isHandleCreate && this.Visible)
            {
                this.DrawImageToLayer();
            }
        }

        public void Owner_Resize(object sender, EventArgs e)
        {
            List<MaskHelper.MaskingClass> mcList = MaskHelper.GetMaskingClass((Control)sender);
            for (int i = 0; i < mcList.Count; i++)
            {
                Size size = GetMaskingSize(mcList[i].owner_form);
                Point point = GetMaskingLocation(mcList[i].owner_form);
                mcList[i].masking_form.SetBounds(point.X, point.Y, size.Width, size.Height);
            }
        }

        public void Owner_LocationChanged(object sender, EventArgs e)
        {
            List<MaskHelper.MaskingClass> mcList = MaskHelper.GetMaskingClass((Control)sender);
            for (int i = 0; i < mcList.Count; i++)
            {
                mcList[i].masking_form.Location = GetMaskingLocation(mcList[i].owner_form);
            }
        }

        public void Owner_VisibleChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            List<MaskHelper.MaskingClass> mcList = MaskHelper.GetMaskingClass(control);
            for (int i = 0; i < mcList.Count; i++)
            {
                if (mcList[i] != null && mcList[i].masking_form != null)
                {
                    if (control.Visible)
                    {
                        mcList[i].masking_form.Show();
                    }
                    else
                    {
                        mcList[i].masking_form.Hide();
                    }
                }
            }
        }

        public void Owner_ParentChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            List<MaskHelper.MaskingClass> mcList = MaskHelper.GetMaskingClass((Control)sender);
            for (int i = 0; i < mcList.Count; i++)
            {
                int index = mcList[i].event_control.IndexOf(control);
                if (index < 0)
                {
                    continue;
                }
                List<Control> controlList = new List<Control>();
                for (int j = 0; j < index; j++)
                {
                    controlList.Add(mcList[i].event_control[j]);
                }
                for (int j = index; j < mcList[i].event_control.Count; j++)
                {
                    mcList[i].event_control[j].Resize -= mcList[i].masking_form.Owner_Resize;
                    mcList[i].event_control[j].LocationChanged -= mcList[i].masking_form.Owner_LocationChanged;
                    mcList[i].event_control[j].VisibleChanged -= mcList[i].masking_form.Owner_VisibleChanged;
                    mcList[i].event_control[j].ParentChanged -= mcList[i].masking_form.Owner_ParentChanged;
                }
                if (control.Parent != null)
                {
                    MaskHelper.UpdateSLEvent(control.Parent, mcList[i], controlList);
                    mcList[i].event_control = controlList;
                }
            }
        }

        /// <summary>该BLENDFUNCTION结构控制通过指定源和目的地的位图的混合函数共混。</summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            /// <summary>源混合操作。当前，唯一已定义的源和目标混合操作是AC_SRC_OVER。有关详细信息，请参见以下“备注”部分。</summary>
            public byte BlendOp;

            /// <summary>必须为零。</summary>
            public byte BlendFlags;

            /// <summary>指定要在整个源位图上使用的Alpha透明度值。所述SourceConstantAlpha值与源位图任何每像素的alpha值组合。如果将SourceConstantAlpha设置为0，则假定图像是透明的。当您只想使用每像素Alpha值时，请将SourceConstantAlpha值设置为255（不透明）。</summary>
            public byte SourceConstantAlpha;

            /// <summary>该成员控制解释源位图和目标位图的方式。AlphaFormat具有以下值。 AC_SRC_ALPHA 当位图具有Alpha通道（即每像素alpha）时，将设置此标志。请注意，API使用预乘Alpha，这意味着位图中的红色，绿色和蓝色通道值必须预乘Alpha通道值。例如，如果Alpha通道值为x，则在调用之前，红色，绿色和蓝色通道必须乘以x并除以0xff。</summary>
            public byte AlphaFormat;
        }

        #endregion

        #region 新增属性
        #endregion

        #region 字段
        #endregion
        #region 重写
        #endregion

        #region 公开方法
        #endregion

        #region 私有方法
        #endregion
    }
}