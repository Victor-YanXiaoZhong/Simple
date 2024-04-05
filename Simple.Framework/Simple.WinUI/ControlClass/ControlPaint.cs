using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Simple.WinUI.Controls
{
    #region 绘制控件

    /// <summary>绘制控件</summary>
    public class ControlPaint
    {
        #region 绘制选中状态

        /// <summary>绘制选中状态</summary>
        /// <param name="graphics"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public static void DrawChecked(Graphics graphics, Rectangle rect, Color color)
        {
            PointF[] points = new PointF[3];
            points[0] = new PointF(rect.X + rect.Width / 4.5f, rect.Y + rect.Height / 2.5f);
            points[1] = new PointF(rect.X + rect.Width / 2.5f, rect.Bottom - rect.Height / 3f);
            points[2] = new PointF(rect.Right - rect.Width / 4.0f, rect.Y + rect.Height / 4.5f);
            using (Pen pen = new Pen(color, 2F))
            {
                graphics.DrawLines(pen, points);
            }
        }

        #endregion 绘制选中状态

        #region 绘制区域

        /// <summary>绘制区域</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="alphaCenter"></param>
        /// <param name="alphaSurround"></param>
        public static void DrawRect(Graphics g, RectangleF rect, int alphaCenter, int alphaSurround)
        {
            DrawRect(g, rect, Color.White, alphaCenter, alphaSurround);
        }

        #endregion 绘制区域

        #region 绘制区域

        /// <summary>绘制区域</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="glassColor"></param>
        /// <param name="alphaCenter"></param>
        /// <param name="alphaSurround"></param>
        public static void DrawRect(Graphics g, RectangleF rect, Color glassColor, int alphaCenter, int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] {
                        Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(
                        rect.X + rect.Width / 2,
                        rect.Y + rect.Height / 2);
                    g.FillPath(brush, path);
                }
            }
        }

        #endregion 绘制区域

        #region 绘制背景图片

        /// <summary>绘制背景图片</summary>
        /// <param name="g"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="backColor"></param>
        /// <param name="backgroundImageLayout"></param>
        /// <param name="bounds"></param>
        /// <param name="clipRect"></param>
        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
        }

        #endregion 绘制背景图片

        #region 绘制背景图片

        /// <summary>绘制背景图片</summary>
        /// <param name="g"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="backColor"></param>
        /// <param name="backgroundImageLayout"></param>
        /// <param name="bounds"></param>
        /// <param name="clipRect"></param>
        /// <param name="scrollOffset"></param>
        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
        }

        #endregion 绘制背景图片

        #region 绘制背景图片

        /// <summary>绘制背景图片</summary>
        /// <param name="g"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="backColor"></param>
        /// <param name="backgroundImageLayout"></param>
        /// <param name="bounds"></param>
        /// <param name="clipRect"></param>
        /// <param name="scrollOffset"></param>
        /// <param name="rightToLeft"></param>
        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (TextureBrush brush = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    if (scrollOffset != Point.Empty)
                    {
                        Matrix transform = brush.Transform;
                        transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);
                        brush.Transform = transform;
                    }
                    g.FillRectangle(brush, clipRect);
                    return;
                }
            }
            Rectangle rect = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
            if ((rightToLeft == RightToLeft.Yes) && (backgroundImageLayout == ImageLayout.None))
            {
                rect.X += clipRect.Width - rect.Width;
            }
            using (SolidBrush brush2 = new SolidBrush(backColor))
            {
                g.FillRectangle(brush2, clipRect);
            }
            if (!clipRect.Contains(rect))
            {
                if ((backgroundImageLayout == ImageLayout.Stretch) || (backgroundImageLayout == ImageLayout.Zoom))
                {
                    rect.Intersect(clipRect);
                    g.DrawImage(backgroundImage, rect);
                }
                else if (backgroundImageLayout == ImageLayout.None)
                {
                    rect.Offset(clipRect.Location);
                    Rectangle destRect = rect;
                    destRect.Intersect(clipRect);
                    Rectangle rectangle3 = new Rectangle(Point.Empty, destRect.Size);
                    g.DrawImage(backgroundImage, destRect, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle rectangle4 = rect;
                    rectangle4.Intersect(clipRect);
                    Rectangle rectangle5 = new Rectangle(new Point(rectangle4.X - rect.X, rectangle4.Y - rect.Y), rectangle4.Size);
                    g.DrawImage(backgroundImage, rectangle4, rectangle5.X, rectangle5.Y, rectangle5.Width, rectangle5.Height, GraphicsUnit.Pixel);
                }
            }
            else
            {
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(backgroundImage, rect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttr);
                imageAttr.Dispose();
            }
        }

        #endregion 绘制背景图片

        #region 计算背景区域

        /// <summary>计算背景区域</summary>
        /// <param name="bounds"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="imageLayout"></param>
        /// <returns></returns>
        public static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
        {
            Rectangle rectangle = bounds;
            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        rectangle.Size = backgroundImage.Size;
                        return rectangle;

                    case ImageLayout.Tile:
                        return rectangle;

                    case ImageLayout.Center:
                        {
                            rectangle.Size = backgroundImage.Size;
                            Size size = bounds.Size;
                            if (size.Width > rectangle.Width)
                            {
                                rectangle.X = (size.Width - rectangle.Width) / 2;
                            }
                            if (size.Height > rectangle.Height)
                            {
                                rectangle.Y = (size.Height - rectangle.Height) / 2;
                            }
                            return rectangle;
                        }
                    case ImageLayout.Stretch:
                        rectangle.Size = bounds.Size;
                        return rectangle;

                    case ImageLayout.Zoom:
                        {
                            Size size2 = backgroundImage.Size;
                            float num = ((float)bounds.Width) / ((float)size2.Width);
                            float num2 = ((float)bounds.Height) / ((float)size2.Height);
                            if (num >= num2)
                            {
                                rectangle.Height = bounds.Height;
                                rectangle.Width = (int)((size2.Width * num2) + 0.5);
                                if (bounds.X >= 0)
                                {
                                    rectangle.X = (bounds.Width - rectangle.Width) / 2;
                                }
                                return rectangle;
                            }
                            rectangle.Width = bounds.Width;
                            rectangle.Height = (int)((size2.Height * num) + 0.5);
                            if (bounds.Y >= 0)
                            {
                                rectangle.Y = (bounds.Height - rectangle.Height) / 2;
                            }
                            return rectangle;
                        }
                }
            }
            return rectangle;
        }

        #endregion 计算背景区域

        #region 设置文本对齐方式

        /// <summary>设置文本对齐方式</summary>
        /// <param name="alignment"></param>
        /// <param name="rightToleft"></param>
        /// <returns></returns>
        public static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags textFormatFlags = TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
            if (rightToleft)
            {
                textFormatFlags |= (TextFormatFlags.Right | TextFormatFlags.RightToLeft);
            }
            if (alignment <= ContentAlignment.MiddleCenter)
            {
                switch (alignment)
                {
                    case ContentAlignment.TopLeft:
                        //textFormatFlags = textFormatFlags;
                        break;

                    case ContentAlignment.TopCenter:
                        textFormatFlags |= TextFormatFlags.HorizontalCenter;
                        break;

                    case (ContentAlignment)3:
                        break;

                    case ContentAlignment.TopRight:
                        textFormatFlags |= TextFormatFlags.Right;
                        break;

                    default:
                        if (alignment != ContentAlignment.MiddleLeft)
                        {
                            if (alignment == ContentAlignment.MiddleCenter)
                            {
                                textFormatFlags |= (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                            }
                        }
                        else
                        {
                            textFormatFlags |= TextFormatFlags.VerticalCenter;
                        }
                        break;
                }
            }
            else
            {
                if (alignment <= ContentAlignment.BottomLeft)
                {
                    if (alignment != ContentAlignment.MiddleRight)
                    {
                        if (alignment == ContentAlignment.BottomLeft)
                        {
                            textFormatFlags |= TextFormatFlags.Bottom;
                        }
                    }
                    else
                    {
                        textFormatFlags |= (TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    }
                }
                else
                {
                    if (alignment != ContentAlignment.BottomCenter)
                    {
                        if (alignment == ContentAlignment.BottomRight)
                        {
                            textFormatFlags |= (TextFormatFlags.Bottom | TextFormatFlags.Right);
                        }
                    }
                    else
                    {
                        textFormatFlags |= (TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
                    }
                }
            }
            return textFormatFlags;
        }

        #endregion 设置文本对齐方式
    }

    #endregion 绘制控件
}