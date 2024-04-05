using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simple.WinUI.Controls.ToolTip.ProperityGrid;

namespace Simple.WinUI.Controls.ToolTip
{
    #region 带有样式的ToolTip

    /// <summary>WWW.CSharpSkin.COM 带有样式的ToolTip</summary>
    public class UToolTip : System.Windows.Forms.ToolTip
    {
        #region 成员

        /// <summary>成员</summary>
        private ImageDc _backDc;

        private Image _toolTipImage;
        private double _toolTipOpacity = 1d;
        private ToolTipColorStyle _toolTipColorStyle;
        private Font _titleFont = new Font("宋体", 9, FontStyle.Bold);
        private Size _imageSize = SystemInformation.SmallIconSize;

        #endregion 成员

        #region CSharpToolTip颜色风格

        /// <summary>CSharpToolTip颜色风格</summary>
        [Description("CSharpToolTip颜色风格")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(ToolTipColorStylePropertyEditor), typeof(UITypeEditor))]
        public ToolTipColorStyle ToolTipColorStyle
        {
            get
            {
                if (_toolTipColorStyle == null)
                {
                    _toolTipColorStyle = new ToolTipColorStyle();
                }
                return _toolTipColorStyle;
            }
            set
            {
                _toolTipColorStyle = value;
            }
        }

        #endregion CSharpToolTip颜色风格

        #region 标题字体

        /// <summary>标题字体</summary>
        [DefaultValue(typeof(Font), "宋体, 9pt, style=Bold")]
        [Browsable(true)]
        [Description("标题字体")]
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                if (_titleFont == null)
                {
                    return;
                }
                if (!_titleFont.IsSystemFont)
                {
                    _titleFont.Dispose();
                }
                _titleFont = value;
            }
        }

        #endregion 标题字体

        #region 重写覆盖自带图标

        /// <summary>重写覆盖自带图标</summary>
        public new ToolTipIcon ToolTipIcon
        {
            get { return base.ToolTipIcon; }
            set
            {
                if (_toolTipImage != null)
                {
                    base.ToolTipIcon = ToolTipIcon.Info;
                }
                else
                {
                    base.ToolTipIcon = value;
                }
            }
        }

        #endregion 重写覆盖自带图标

        #region ToolTipICon图片

        /// <summary>ToolTipICon图片</summary>
        [DefaultValue(null)]
        [Browsable(true)]
        [Description("ToolTipICon图片")]
        public Image ToolTipImage
        {
            get { return _toolTipImage; }
            set
            {
                _toolTipImage = value;
                if (_toolTipImage == null)
                {
                    base.ToolTipIcon = ToolTipIcon.None;
                }
                else
                {
                    base.ToolTipIcon = ToolTipIcon.Info;
                }
            }
        }

        #endregion ToolTipICon图片

        #region 图片尺寸

        /// <summary>图片尺寸</summary>
        [DefaultValue(typeof(Size), "16, 16")]
        [Browsable(true)]
        [Description("图片尺寸")]
        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                if (_imageSize != value)
                {
                    _imageSize = value;
                    if (_imageSize.Width > 32)
                    {
                        _imageSize.Width = 32;
                    }

                    if (_imageSize.Height > 32)
                    {
                        _imageSize.Height = 32;
                    }
                }
            }
        }

        #endregion 图片尺寸

        #region 透明度

        /// <summary>透明度</summary>
        [DefaultValue(1d)]
        [TypeConverter(typeof(OpacityConverter))]
        [Browsable(true)]
        [Description("透明度")]
        public double ToolTipOpacity
        {
            get { return _toolTipOpacity; }
            set
            {
                if (value < 0 && value > 1)
                {
                    return;
                }
                _toolTipOpacity = value;
            }
        }

        #endregion 透明度

        #region 构造

        /// <summary>构造</summary>
        public UToolTip() : base()
        {
            InitOwnerDrawEvent();
        }

        public UToolTip(IContainer container) : base(container)
        {
            InitOwnerDrawEvent();
        }

        #endregion 构造

        #region 返回当前控件句柄

        /// <summary>返回当前控件句柄</summary>
        protected IntPtr Handle
        {
            get
            {
                if (!DesignMode)
                {
                    Type t = typeof(System.Windows.Forms.ToolTip);
                    PropertyInfo pi = t.GetProperty("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
                    IntPtr handle = (IntPtr)pi.GetValue(this, null);
                    return handle;
                }
                return IntPtr.Zero;
            }
        }

        #endregion 返回当前控件句柄

        #region 加载初始化绘制事件

        /// <summary>加载初始化绘制事件</summary>
        private void InitOwnerDrawEvent()
        {
            base.OwnerDraw = true;
            base.ReshowDelay = 800;
            base.InitialDelay = 500;
            base.Draw += CSharpToolTip_Draw;//new DrawToolTipEventHandler(ToolTipExDraw);
            base.Popup += CSharpToolTip_Popup;//new PopupEventHandler(ToolTipExPopup);
        }

        #endregion 加载初始化绘制事件

        #region 提示最初显示之前发生

        /// <summary>提示最初显示之前发生</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CSharpToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (_toolTipOpacity < 1D)
            {
                //如果使用背景透明，获取背景图。
                SetDesktopWindowBackImageToToolTip();
            }
        }

        #endregion 提示最初显示之前发生

        #region 绘制ToolTip

        /// <summary>绘制ToolTip</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CSharpToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            int alpha = (int)(_toolTipOpacity * 255);
            int defaultXOffset = 12;
            int defaultTopHeight = 36;
            int tipTextXOffset = 3;
            int tipTextYOffset = 3;
            if (Handle != IntPtr.Zero && _toolTipOpacity < 1D)
            {
                IntPtr hDC = g.GetHdc();
                WinApi.BitBlt(hDC, 0, 0, bounds.Width, bounds.Height, _backDc.IntPtr, 0, 0, 0xCC0020);
                g.ReleaseHdc(hDC);
            }
            Color backNormalColor = Color.FromArgb(alpha, _toolTipColorStyle.BackNormalColor);
            Color baseColor = Color.FromArgb(alpha, _toolTipColorStyle.BackHoverColor);
            Color borderColor = Color.FromArgb(alpha, _toolTipColorStyle.BorderColor);
            using (LinearGradientBrush brush = new LinearGradientBrush(bounds, backNormalColor, baseColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, bounds);
            }
            System.Windows.Forms.ControlPaint.DrawBorder(g, bounds, borderColor, ButtonBorderStyle.Solid);
            Rectangle imageRect = Rectangle.Empty;
            Rectangle titleRect;
            Rectangle tipRect;
            if (base.ToolTipIcon != ToolTipIcon.None)
            {
                tipTextXOffset = defaultXOffset;
                tipTextYOffset = defaultTopHeight;
                imageRect = new Rectangle(bounds.X + defaultXOffset - (ImageSize.Width - 16) / 2, bounds.Y + (defaultTopHeight - _imageSize.Height) / 2, _imageSize.Width, _imageSize.Height);
                Image image = _toolTipImage;
                bool bDispose = false;
                if (image == null)
                {
                    Icon icon = GetToolTipIcon();
                    if (icon != null)
                    {
                        image = icon.ToBitmap();
                        bDispose = true;
                    }
                }
                if (image != null)
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    if (_toolTipOpacity < 1D)
                    {
                        ControlRender.RenderAlphaImage(g, image, imageRect, (float)_toolTipOpacity);
                    }
                    else
                    {
                        g.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                    }
                }
                if (bDispose)
                {
                    image.Dispose();
                }
            }
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            if (!string.IsNullOrEmpty(base.ToolTipTitle))
            {
                tipTextXOffset = defaultXOffset;
                tipTextYOffset = defaultTopHeight;
                int x = imageRect.IsEmpty ? defaultXOffset : imageRect.Right + 3;
                titleRect = new Rectangle(x, bounds.Y, bounds.Width - x, defaultTopHeight);
                Color foreColor = Color.FromArgb(alpha, _toolTipColorStyle.TitleForeColor);
                using (Brush brush = new SolidBrush(foreColor))
                {
                    g.DrawString(base.ToolTipTitle, _titleFont, brush, titleRect, sf);
                }
            }
            if (!string.IsNullOrEmpty(e.ToolTipText))
            {
                tipRect = new Rectangle(bounds.X + tipTextXOffset, bounds.Y + tipTextYOffset, bounds.Width - tipTextXOffset * 2, bounds.Height - tipTextYOffset);
                sf = StringFormat.GenericTypographic;
                Color foreColor = Color.FromArgb(alpha, _toolTipColorStyle.TipForeColor);
                using (Brush brush = new SolidBrush(foreColor))
                {
                    g.DrawString(e.ToolTipText, e.Font, brush, tipRect, sf);
                }
            }
        }

        #endregion 绘制ToolTip

        #region 将桌面背景复制到当前资源中

        /// <summary>将桌面背景复制到当前资源中</summary>
        private void SetDesktopWindowBackImageToToolTip()
        {
            IntPtr handle = Handle;
            if (handle == IntPtr.Zero)
            {
                return;
            }
            RECT rect = new RECT();
            WinApi.GetWindowRect(handle, ref rect);
            Size size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            _backDc = new ImageDc(size.Width, size.Height);
            IntPtr pD = WinApi.GetDesktopWindow();
            IntPtr pH = WinApi.GetDC(pD);
            WinApi.BitBlt(_backDc.IntPtr, 0, 0, size.Width, size.Height, pH, rect.Left, rect.Top, 0xCC0020);
            WinApi.ReleaseDC(pD, pH);
        }

        #endregion 将桌面背景复制到当前资源中

        #region 返回ToolTip图标

        /// <summary>返回ToolTip图标</summary>
        /// <returns></returns>
        private Icon GetToolTipIcon()
        {
            switch (base.ToolTipIcon)
            {
                case ToolTipIcon.Info:
                    return SystemIcons.Information;

                case ToolTipIcon.Warning:
                    return SystemIcons.Warning;

                case ToolTipIcon.Error:
                    return SystemIcons.Error;

                default:
                    return null;
            }
        }

        #endregion 返回ToolTip图标

        #region 释放资源

        /// <summary>释放资源</summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_backDc != null)
                {
                    _backDc.Dispose();
                    _backDc = null;
                }
                if (!_titleFont.IsSystemFont)
                {
                    _titleFont.Dispose();
                }
                _titleFont = null;
                _toolTipImage = null;
                _toolTipColorStyle = null;
            }
        }

        #endregion 释放资源
    }

    #endregion 带有样式的ToolTip
}