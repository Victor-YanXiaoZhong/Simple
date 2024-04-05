using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Simple.WinUI.Controls
{
    #region 控件渲染

    /// <summary>控件渲染</summary>
    public class ControlRender
    {
        #region 渲染背景

        /// <summary>渲染背景</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="innerBorderColor"></param>
        /// <param name="style"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        /// <param name="mode"></param>
        /// <param name="useGradientMode">是否使用渐变模式</param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, bool drawBorder, bool drawGlass, LinearGradientMode mode, bool useGradientMode = false)
        {
            RenderBackground(g, rect, baseColor, borderColor, innerBorderColor, style, 8, drawBorder, drawGlass, mode, useGradientMode);
        }

        #endregion 渲染背景

        #region 渲染背景

        /// <summary>渲染背景</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="innerBorderColor"></param>
        /// <param name="style"></param>
        /// <param name="roundWidth"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        /// <param name="mode"></param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, bool drawBorder, bool drawGlass, LinearGradientMode mode, bool useGradientMode = false)
        {
            RenderBackground(g, rect, baseColor, borderColor, innerBorderColor, style, 8, 0.45f, drawBorder, drawGlass, mode, useGradientMode);
        }

        #endregion 渲染背景

        #region 渲染绘制背景

        /// <summary>渲染绘制背景</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="baseColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="innerBorderColor"></param>
        /// <param name="style"></param>
        /// <param name="roundWidth"></param>
        /// <param name="basePosition"></param>
        /// <param name="drawBorder"></param>
        /// <param name="drawGlass"></param>
        /// <param name="mode"></param>
        public static void RenderBackground(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, float basePosition, bool drawBorder, bool drawGlass, LinearGradientMode mode, bool useGradientMode = false, Control thisControl = null)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (useGradientMode)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
                {
                    Color[] colors = new Color[4] { baseColor, baseColor, baseColor, baseColor };
                    if (useGradientMode)
                    {
                        colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                        colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                        colors[2] = baseColor;
                        colors[3] = GetColor(baseColor, 0, 35, 24, 9);
                    }
                    ColorBlend blend = new ColorBlend();
                    blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                    blend.Colors = colors;
                    brush.InterpolationColors = blend;
                    if (style != RoundStyle.None)
                    {
                        using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, roundWidth, style, false))
                        {
                            g.FillPath(brush, path);
                        }
                        //if (baseColor.A > 80)
                        //{
                        //    Rectangle rectTop = rect;

                        //    if (mode == LinearGradientMode.Vertical)
                        //    {
                        //        rectTop.Height = (int)(rectTop.Height * basePosition);
                        //    }
                        //    else
                        //    {
                        //        rectTop.Width = (int)(rect.Width * basePosition);
                        //    }
                        //    using (GraphicsPath pathTop = GraphicsPathManager.CreatePath(rectTop, roundWidth, RoundStyle.Top, false))
                        //    {
                        //        using (SolidBrush brushAlpha = new SolidBrush(Color.FromArgb(128, 255, 255, 255)))
                        //        {
                        //            g.FillPath(brushAlpha, pathTop);
                        //        }
                        //    }
                        //}

                        if (drawGlass)
                        {
                            RectangleF glassRect = rect;
                            if (mode == LinearGradientMode.Vertical)
                            {
                                glassRect.Y = rect.Y + rect.Height * basePosition;
                                glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                            }
                            else
                            {
                                glassRect.X = rect.X + rect.Width * basePosition;
                                glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                            }
                            ControlPaint.DrawRect(g, glassRect, 170, 0);
                        }

                        if (drawBorder)
                        {
                            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, roundWidth, style, false))
                            {
                                using (Pen pen = new Pen(borderColor))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }

                            rect.Inflate(-1, -1);
                            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, roundWidth, style, false))
                            {
                                using (Pen pen = new Pen(innerBorderColor))
                                {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                    }
                    else
                    {
                        g.FillRectangle(brush, rect);

                        if (drawGlass)
                        {
                            RectangleF glassRect = rect;
                            if (mode == LinearGradientMode.Vertical)
                            {
                                glassRect.Y = rect.Y + rect.Height * basePosition;
                                glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                            }
                            else
                            {
                                glassRect.X = rect.X + rect.Width * basePosition;
                                glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                            }
                            ControlPaint.DrawRect(g, glassRect, 200, 0);
                        }

                        if (drawBorder)
                        {
                            using (Pen pen = new Pen(borderColor))
                            {
                                g.DrawRectangle(pen, rect);
                            }

                            rect.Inflate(-1, -1);
                            using (Pen pen = new Pen(innerBorderColor))
                            {
                                g.DrawRectangle(pen, rect);
                            }
                        }
                    }
                }
            }
            else
            {
                using (var brush = new SolidBrush(baseColor))
                {
                    if (style != RoundStyle.None)
                    {
                        g.FillRectangle(brush, rect);

                        if (drawBorder)
                        {
                            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, roundWidth, style, false))
                            {
                                using (Pen pen = new Pen(borderColor))
                                {
                                    thisControl.Region = new Region(path);
                                    g.DrawPath(pen, path);
                                }
                            }

                            rect.Inflate(-1, -1);
                            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, roundWidth, style, false))
                            {
                                using (Pen pen = new Pen(innerBorderColor))
                                {
                                    thisControl.Region = new Region(path);
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                    }
                    else
                    {
                        g.FillRectangle(brush, rect);

                        if (drawBorder)
                        {
                            using (Pen pen = new Pen(borderColor))
                            {
                                g.DrawRectangle(pen, rect);
                            }

                            rect.Inflate(-1, -1);
                            using (Pen pen = new Pen(innerBorderColor))
                            {
                                g.DrawRectangle(pen, rect);
                            }
                        }
                    }
                }
            }
        }

        #endregion 渲染绘制背景

        #region 从RGB值返回颜色

        /// <summary>从RGB值返回颜色</summary>
        /// <param name="colorBase"></param>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;
            if (a + a0 > 255) { a = 255; } else { a = Math.Max(0, a + a0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(0, r + r0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(0, g + g0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(0, b + b0); }
            return Color.FromArgb(a, r, g, b);
        }

        #endregion 从RGB值返回颜色

        #region 渲染绘制箭头

        /// <summary>渲染绘制箭头</summary>
        /// <param name="g"></param>
        /// <param name="dropDownRect"></param>
        /// <param name="direction"></param>
        /// <param name="brush"></param>
        public static void RenderArrow(Graphics g, Rectangle dropDownRect, ArrowDirection direction, Brush brush)
        {
            Point point = new Point(dropDownRect.Left + (dropDownRect.Width / 2), dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[] { new Point(point.X + 2, point.Y - 3), new Point(point.X + 2, point.Y + 3), new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] { new Point(point.X - 3, point.Y + 2), new Point(point.X + 3, point.Y + 2), new Point(point.X, point.Y - 2) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] { new Point(point.X - 2, point.Y - 3), new Point(point.X - 2, point.Y + 3), new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] { new Point(point.X - 3, point.Y - 1), new Point(point.X + 3, point.Y - 1), new Point(point.X, point.Y + 2) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        #endregion 渲染绘制箭头

        #region 渲染矩形

        /// <summary>渲染矩形</summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="pixelsBetweenDots"></param>
        /// <param name="outerColor"></param>
        public static void RenderRectangle(Graphics g, Rectangle rect, Size pixelsBetweenDots, Color outerColor)
        {
            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);
            IntPtr hdc = g.GetHdc();
            for (int x = rect.X; x <= rect.Right; x += pixelsBetweenDots.Width)
            {
                for (int y = rect.Y; y <= rect.Bottom; y += pixelsBetweenDots.Height)
                {
                    WinApi.SetPixel(hdc, x, y, outerWin32Corlor);
                }
            }

            g.ReleaseHdc(hdc);
        }

        #endregion 渲染矩形

        #region 用Alpha绘制图片

        /// <summary>用Alpha绘制图片</summary>
        /// <param name="g"></param>
        /// <param name="image"></param>
        /// <param name="imageRect"></param>
        /// <param name="alpha"></param>
        public static void RenderAlphaImage(Graphics g, Image image, Rectangle imageRect, float alpha)
        {
            using (ImageAttributes imageAttributes = new ImageAttributes())
            {
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                float[][] colorMatrixElements = {
                    new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                    new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                    new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                    new float[] {0.0f,  0.0f,  0.0f,  alpha, 0.0f},
                    new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
            }
        }

        #endregion 用Alpha绘制图片
    }

    #endregion 控件渲染
}