using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;

namespace Simple.WinUI.Controls.Button
{
    #region 按钮

    /// <summary>按钮</summary>
    public class UButton : System.Windows.Forms.Button
    {
        #region 按钮基础颜色

        private Color _baseColor
        {
            get { return BackColor; }
        }

        #endregion 按钮基础颜色

        #region 鼠标进入按钮颜色

        private Color _hoverColor = Color.LightGray;

        /// <summary>鼠标进入按钮颜色</summary>
        [Browsable(true)]
        [Description("鼠标进入按钮颜色")]
        public Color HoverColor
        {
            get
            {
                return this._hoverColor;
            }
            set
            {
                this._hoverColor = value;
                base.Invalidate();
            }
        }

        #endregion 鼠标进入按钮颜色

        #region 鼠标按下按钮颜色

        private Color _pressedColor = Color.DarkGray;

        /// <summary>鼠标按下按钮颜色</summary>
        [Browsable(true)]
        [Description("鼠标按下按钮颜色")]
        public Color PressedColor
        {
            get
            {
                return this._pressedColor;
            }
            set
            {
                this._pressedColor = value;
                base.Invalidate();
            }
        }

        #endregion 鼠标按下按钮颜色

        #region 按钮状态

        private ButtonState _buttonState;

        /// <summary>按钮状态</summary>
        [Browsable(true)]
        [Description("按钮状态")]
        public ButtonState ButtonState
        {
            get
            {
                return this._buttonState;
            }
            set
            {
                if (this._buttonState != value)
                {
                    this._buttonState = value;
                    base.Invalidate();
                }
            }
        }

        #endregion 按钮状态

        #region 是否使用渐变色

        private bool _useGradientMode = false;

        [Browsable(true)]
        [Description("是否使用渐变色")]
        public bool UseGradientMode
        {
            get { return _useGradientMode; }
            set { _useGradientMode = value; base.Invalidate(); }
        }

        #endregion 是否使用渐变色

        #region 图标宽度

        private int _imageWidth = 18;

        /// <summary>图标宽度</summary>
        [Browsable(true)]
        [Description("图标宽度")]
        public int ImageWidth
        {
            get
            {
                return this._imageWidth;
            }
            set
            {
                if (value != this._imageWidth)
                {
                    this._imageWidth = ((value < 12) ? 12 : value);
                    base.Invalidate();
                }
            }
        }

        #endregion 图标宽度

        #region 按钮圆角样式

        private RoundStyle _roundStyle = RoundStyle.All;

        /// <summary>按钮圆角样式</summary>
        [Browsable(true)]
        [Description("按钮圆角样式")]
        public RoundStyle RoundStyle
        {
            get
            {
                return this._roundStyle;
            }
            set
            {
                if (this._roundStyle != value)
                {
                    this._roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        #endregion 按钮圆角样式

        #region 圆角弧度

        private int _radius = 8;

        /// <summary>圆角弧度</summary>
        [Browsable(true)]
        [Description("圆角弧度")]
        public int Radius
        {
            get
            {
                return this._radius;
            }
            set
            {
                if (this._radius != value)
                {
                    this._radius = ((value < 0) ? 0 : value);
                    base.Invalidate();
                }
            }
        }

        #endregion 圆角弧度

        #region 构造

        /// <summary>构造</summary>
        public UButton()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion 构造

        #region 鼠标进入

        /// <summary>鼠标进入</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.ButtonState = ButtonState.Hover;
        }

        #endregion 鼠标进入

        #region 鼠标离开

        /// <summary>鼠标离开</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.ButtonState = ButtonState.Normal;
        }

        #endregion 鼠标离开

        #region 鼠标按下

        /// <summary>鼠标按下</summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                this.ButtonState = ButtonState.Pressed;
            }
        }

        #endregion 鼠标按下

        #region 鼠标松下

        /// <summary>鼠标松下</summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (base.ClientRectangle.Contains(e.Location))
                {
                    this.ButtonState = ButtonState.Hover;
                }
                else
                {
                    this.ButtonState = ButtonState.Normal;
                }
            }
        }

        #endregion 鼠标松下

        #region 重绘按钮

        private bool HasText()
        {
            if (this.Text != null && this.Text != "")
                return true;
            return false;
        }

        /// <summary>重绘按钮</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Graphics graphics = e.Graphics;
            Rectangle destRect;
            Rectangle bounds;
            this.CalculateRect(out destRect, out bounds);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color innerBorderColor = Color.FromArgb(200, 255, 255, 255);
            Color baseColor;
            Color borderColor;
            if (base.Enabled)
            {
                switch (this.ButtonState)
                {
                    case ButtonState.Hover:
                        baseColor = this.HoverColor;
                        borderColor = this._baseColor;
                        break;

                    case ButtonState.Pressed:
                        baseColor = this.PressedColor;
                        borderColor = this._baseColor;
                        break;

                    default:
                        baseColor = this._baseColor;
                        borderColor = this._baseColor;
                        break;
                }
            }
            else
            {
                baseColor = SystemColors.ControlDark;
                borderColor = SystemColors.ControlDark;
            }
            ControlRender.RenderBackground(graphics, base.ClientRectangle, baseColor, borderColor, innerBorderColor, this.RoundStyle, this.Radius, 0.35f, true, true, LinearGradientMode.Vertical, _useGradientMode, this);

            if (base.Image != null)
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.DrawImage(base.Image, destRect, 0, 0, HasText() ? base.Image.Width : this.Width,
                    HasText() ? base.Image.Height : this.Height, GraphicsUnit.Pixel);
            }
            TextRenderer.DrawText(graphics, this.Text, this.Font, bounds, this.ForeColor, ControlPaint.GetTextFormatFlags(this.TextAlign, this.RightToLeft == RightToLeft.Yes));
        }

        #endregion 重绘按钮

        #region 计算区域

        /// <summary>计算区域</summary>
        /// <param name="imageRect"></param>
        /// <param name="textRect"></param>
        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (base.Image == null)
            {
                textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
            }
            else
            {
                switch (base.TextImageRelation)
                {
                    case TextImageRelation.Overlay:
                        imageRect = new Rectangle(2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, 0, base.Width - 4, base.Height);
                        break;

                    case TextImageRelation.ImageAboveText:
                        imageRect = new Rectangle((base.Width - this.ImageWidth) / 2, 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, imageRect.Bottom, base.Width, base.Height - imageRect.Bottom - 2);
                        break;

                    case TextImageRelation.TextAboveImage:
                        imageRect = new Rectangle((base.Width - this.ImageWidth) / 2, base.Height - this.ImageWidth - 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(0, 2, base.Width, base.Height - imageRect.Y - 2);
                        break;

                    case TextImageRelation.ImageBeforeText:
                        imageRect = new Rectangle(2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(imageRect.Right + 2, 0, base.Width - imageRect.Right - 4, base.Height);
                        break;

                    case TextImageRelation.TextBeforeImage:
                        imageRect = new Rectangle(base.Width - this.ImageWidth - 2, (base.Height - this.ImageWidth) / 2, this.ImageWidth, this.ImageWidth);
                        textRect = new Rectangle(2, 0, imageRect.X - 2, base.Height);
                        break;
                }
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    imageRect.X = base.Width - imageRect.Right;
                    textRect.X = base.Width - textRect.Right;
                }
            }
        }

        #endregion 计算区域

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // UButton
            this.Size = new System.Drawing.Size(75, 30);
            this.ResumeLayout(false);
        }
    }

    #endregion 按钮
}