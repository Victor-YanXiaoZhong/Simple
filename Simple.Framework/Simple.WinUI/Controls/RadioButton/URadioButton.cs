using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.RadioButton
{
    #region 带颜色圆框的单选框

    /// <summary>WWW.CSharpSkin.COM 带颜色圆框的单选框</summary>
    public class URadioButton : System.Windows.Forms.RadioButton
    {
        #region 单选框按钮颜色

        /// <summary>单选框按钮颜色</summary>
        private Color _radioColor = Color.FromArgb(39, 134, 241);

        [Browsable(true)]
        [Description("单选框按钮颜色")]
        public Color RadioColor
        {
            get
            {
                return _radioColor;
            }
            set
            {
                _radioColor = value;
                this.Invalidate();
            }
        }

        #endregion 单选框按钮颜色

        #region 单选框按钮状态

        /// <summary>单选框按钮状态</summary>
        private RadioButtonState _radiobuttonState;

        public RadioButtonState RadiobuttonState
        {
            get { return _radiobuttonState; }
            set
            {
                if (_radiobuttonState != value)
                {
                    _radiobuttonState = value;
                    base.Invalidate();
                }
            }
        }

        #endregion 单选框按钮状态

        #region 内容右对齐

        /// <summary>内容右对齐</summary>
        private static readonly ContentAlignment RightAlignment =
            ContentAlignment.TopRight |
            ContentAlignment.BottomRight |
            ContentAlignment.MiddleRight;

        #endregion 内容右对齐

        #region 内容左对齐

        /// <summary>内容左对齐</summary>
        private static readonly ContentAlignment LeftAligbment =
            ContentAlignment.TopLeft |
            ContentAlignment.BottomLeft |
            ContentAlignment.MiddleLeft;

        #endregion 内容左对齐

        #region 构造

        /// <summary>构造</summary>
        public URadioButton()
           : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion 构造

        #region 单选框按钮选中宽度

        /// <summary>单选框按钮选中宽度</summary>
        protected virtual int DefaultCheckButtonWidth
        {
            get { return 12; }
        }

        #endregion 单选框按钮选中宽度

        #region 鼠标进入

        /// <summary>鼠标进入</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _radiobuttonState = RadioButtonState.Hover;
        }

        #endregion 鼠标进入

        #region 鼠标离开

        /// <summary>鼠标离开</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _radiobuttonState = RadioButtonState.Normal;
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
                _radiobuttonState = RadioButtonState.Pressed;
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
                if (ClientRectangle.Contains(e.Location))
                {
                    _radiobuttonState = RadioButtonState.Hover;
                }
                else
                {
                    _radiobuttonState = RadioButtonState.Normal;
                }
            }
        }

        #endregion 鼠标松下

        #region 绘制单选框

        /// <summary>绘制单选框</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            Rectangle radioButtonrect;
            Rectangle textRect;
            CalculateRect(out radioButtonrect, out textRect);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color borderColor;
            Color innerBorderColor;
            Color checkColor;
            bool hover = false;
            if (Enabled)
            {
                switch (_radiobuttonState)
                {
                    case RadioButtonState.Hover:
                        borderColor = _radioColor;
                        innerBorderColor = _radioColor;
                        checkColor = GetColor(_radioColor, 0, 35, 24, 9);
                        hover = true;
                        break;

                    case RadioButtonState.Pressed:
                        borderColor = _radioColor;
                        innerBorderColor = GetColor(_radioColor, 0, -13, -8, -3);
                        checkColor = GetColor(_radioColor, 0, -35, -24, -9);
                        hover = true;
                        break;

                    default:
                        borderColor = _radioColor;
                        innerBorderColor = Color.Empty;
                        checkColor = _radioColor;
                        break;
                }
            }
            else
            {
                borderColor = SystemColors.ControlDark;
                innerBorderColor = SystemColors.ControlDark;
                checkColor = SystemColors.ControlDark;
            }

            //绘制按钮背景
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, radioButtonrect);
            }

            if (hover)
            {
                using (Pen pen = new Pen(innerBorderColor, 2F))
                {
                    g.DrawEllipse(pen, radioButtonrect);
                }
            }
            //如果选中，则绘制选中状态圆形部分
            if (Checked)
            {
                radioButtonrect.Inflate(-2, -2);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(radioButtonrect);
                    using (PathGradientBrush brush = new PathGradientBrush(path))
                    {
                        brush.CenterColor = checkColor;
                        brush.SurroundColors = new Color[] { Color.White };
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, 0.4f, 1f };
                        blend.Factors = new float[] { 0f, 0.4f, 1f };
                        brush.Blend = blend;
                        g.FillEllipse(brush, radioButtonrect);
                    }
                }
                radioButtonrect.Inflate(2, 2);
            }
            //绘制按钮圆形部分
            using (Pen pen = new Pen(borderColor))
            {
                g.DrawEllipse(pen, radioButtonrect);
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        #endregion 绘制单选框

        #region 获取单选按钮和文本区域

        /// <summary>获取单选按钮和文本区域</summary>
        /// <param name="radioButtonRect"></param>
        /// <param name="textRect"></param>
        private void CalculateRect(out Rectangle radioButtonRect, out Rectangle textRect)
        {
            radioButtonRect = new Rectangle(
                0, 0, DefaultCheckButtonWidth, DefaultCheckButtonWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAligbment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = RightToLeft == RightToLeft.Yes;

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        radioButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = 1;

                textRect = new Rectangle(
                    radioButtonRect.Right + 2,
                    0,
                    Width - radioButtonRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        radioButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = Width - DefaultCheckButtonWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - DefaultCheckButtonWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        radioButtonRect.Y = 2;
                        textRect.Y = radioButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;

                    case ContentAlignment.MiddleCenter:
                        radioButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;

                    case ContentAlignment.BottomCenter:
                        radioButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                }

                radioButtonRect.X = (Width - DefaultCheckButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        #endregion 获取单选按钮和文本区域

        #region 根据RGB值获取颜色

        /// <summary>根据RGB值获取颜色</summary>
        /// <param name="colorBase"></param>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;
            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }
            return Color.FromArgb(a, r, g, b);
        }

        #endregion 根据RGB值获取颜色

        #region 获取文本对齐格式

        /// <summary>获取文本对齐格式</summary>
        /// <param name="alignment"></param>
        /// <param name="rightToleft"></param>
        /// <returns></returns>
        private static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;

                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;

                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;

                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;

                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;

                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;

                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;

                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;

                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        #endregion 获取文本对齐格式
    }

    #endregion 带颜色圆框的单选框
}