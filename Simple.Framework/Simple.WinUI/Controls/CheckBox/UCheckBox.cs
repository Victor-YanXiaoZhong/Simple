using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.CheckBox
{
    #region 带边框颜色的复选框

    /// <summary>带边框颜色的复选框</summary>
    public class UCheckBox : System.Windows.Forms.CheckBox
    {
        #region 构造

        /// <summary>构造</summary>
        public UCheckBox() : base()
        {
            SetStyle(
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion 构造

        #region 复选框按钮颜色

        /// <summary>复选框按钮颜色</summary>
        private Color _checkBoxColor = Color.FromArgb(39, 134, 241);

        [Browsable(true)]
        [Description("复选框按钮颜色")]
        public Color CheckBoxColor
        {
            get
            {
                return _checkBoxColor;
            }
            set
            {
                _checkBoxColor = value;
                this.Invalidate();
            }
        }

        #endregion 复选框按钮颜色

        #region 复选框按钮状态

        /// <summary>复选框按钮状态</summary>
        private CheckBoxState _checkBoxState;

        public CheckBoxState CheckBoxState
        {
            get { return _checkBoxState; }
            set
            {
                if (_checkBoxState != value)
                {
                    _checkBoxState = value;
                    base.Invalidate();
                }
            }
        }

        #endregion 复选框按钮状态

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

        #region 复选框按钮选中宽度

        /// <summary>复选框按钮选中宽度</summary>
        protected virtual int DefaultCheckButtonWidth
        {
            get { return 12; }
        }

        #endregion 复选框按钮选中宽度

        #region 鼠标进入

        /// <summary>鼠标进入</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _checkBoxState = CheckBoxState.Hover;
        }

        #endregion 鼠标进入

        #region 鼠标离开

        /// <summary>鼠标离开</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _checkBoxState = CheckBoxState.Normal;
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
                _checkBoxState = CheckBoxState.Pressed;
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
                    _checkBoxState = CheckBoxState.Hover;
                }
                else
                {
                    _checkBoxState = CheckBoxState.Normal;
                }
            }
        }

        #endregion 鼠标松下

        #region 绘制复选框

        /// <summary>绘制复选框</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            Rectangle checkButtonRect;
            Rectangle textRect;

            CalculateRect(out checkButtonRect, out textRect);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color borderColor;
            Color innerBorderColor;
            Color checkColor;
            bool hover = false;
            if (Enabled)
            {
                switch (_checkBoxState)
                {
                    case CheckBoxState.Hover:
                        borderColor = _checkBoxColor;
                        innerBorderColor = _checkBoxColor;
                        checkColor = GetColor(_checkBoxColor, 0, 35, 24, 9);
                        hover = true;
                        break;

                    case CheckBoxState.Pressed:
                        borderColor = _checkBoxColor;
                        innerBorderColor = GetColor(_checkBoxColor, 0, -13, -8, -3);
                        checkColor = GetColor(_checkBoxColor, 0, -35, -24, -9);
                        hover = true;
                        break;

                    default:
                        borderColor = _checkBoxColor;
                        innerBorderColor = Color.Empty;
                        checkColor = _checkBoxColor;
                        break;
                }
            }
            else
            {
                borderColor = SystemColors.ControlDark;
                innerBorderColor = SystemColors.ControlDark;
                checkColor = SystemColors.ControlDark;
            }

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillRectangle(brush, checkButtonRect);
            }
            if (hover)
            {
                using (Pen pen = new Pen(innerBorderColor, 2F))
                {
                    g.DrawRectangle(pen, checkButtonRect);
                }
            }
            switch (CheckState)
            {
                case CheckState.Checked:
                    DrawCheckedFlag(
                        g,
                        checkButtonRect,
                        checkColor);
                    break;

                case CheckState.Indeterminate:
                    checkButtonRect.Inflate(-1, -1);
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddEllipse(checkButtonRect);
                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = checkColor;
                            brush.SurroundColors = new Color[] { Color.White };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.4f, 1f };
                            blend.Factors = new float[] { 0f, 0.3f, 1f };
                            brush.Blend = blend;
                            g.FillEllipse(brush, checkButtonRect);
                        }
                    }
                    checkButtonRect.Inflate(1, 1);
                    break;
            }

            using (Pen pen = new Pen(borderColor))
            {
                g.DrawRectangle(pen, checkButtonRect);
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

        #endregion 绘制复选框

        #region 计算获取按钮区域和文本区域

        /// <summary>计算获取按钮区域和文本区域</summary>
        /// <param name="checkButtonRect"></param>
        /// <param name="textRect"></param>
        private void CalculateRect(
             out Rectangle checkButtonRect, out Rectangle textRect)
        {
            checkButtonRect = new Rectangle(
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
                        checkButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = 1;

                textRect = new Rectangle(
                    checkButtonRect.Right + 2,
                    0,
                    Width - checkButtonRect.Right - 4,
                    Height);
            }
            else if ((bCheckAlignRight && !bRightToLeft)
                || (bCheckAlignLeft && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkButtonRect.Y = 2;
                        break;

                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;

                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = Width - DefaultCheckButtonWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - DefaultCheckButtonWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        checkButtonRect.Y = 2;
                        textRect.Y = checkButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;

                    case ContentAlignment.MiddleCenter:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;

                    case ContentAlignment.BottomCenter:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                }

                checkButtonRect.X = (Width - DefaultCheckButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        #endregion 计算获取按钮区域和文本区域

        #region 绘制选中边框

        /// <summary>绘制选中边框</summary>
        /// <param name="graphics"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        private void DrawCheckedFlag(Graphics graphics, Rectangle rect, Color color)
        {
            PointF[] points = new PointF[3];
            points[0] = new PointF(
                rect.X + rect.Width / 4.5f,
                rect.Y + rect.Height / 2.5f);
            points[1] = new PointF(
                rect.X + rect.Width / 2.5f,
                rect.Bottom - rect.Height / 3f);
            points[2] = new PointF(
                rect.Right - rect.Width / 4.0f,
                rect.Y + rect.Height / 4.5f);
            using (Pen pen = new Pen(color, 2F))
            {
                graphics.DrawLines(pen, points);
            }
        }

        #endregion 绘制选中边框

        #region 根据RGB获取颜色

        /// <summary>根据RGB获取颜色</summary>
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

        #endregion 根据RGB获取颜色

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

    #endregion 带边框颜色的复选框
}