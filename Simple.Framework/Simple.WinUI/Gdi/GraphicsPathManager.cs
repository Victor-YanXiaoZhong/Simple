using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Simple.WinUI.Controls.TrackBar;

namespace Simple.WinUI
{
    #region 创建绘制路径

    /// <summary>创建绘制路径</summary>
    public class GraphicsPathManager
    {
        #region 建立带有圆角样式的路径

        /// <summary>建立带有圆角样式的路径。</summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePath(
            Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;
            switch (style)
            {
                case RoundStyle.None:
                    path.AddRectangle(rect);
                    break;

                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius, 0, 90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;

                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Y,
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;

                case RoundStyle.Right:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                       rect.Right - radius - radiusCorrection,
                       rect.Bottom - radius - radiusCorrection,
                       radius,
                       radius,
                       0,
                       90);
                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    break;

                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection,
                        rect.X, rect.Bottom - radiusCorrection);
                    break;

                case RoundStyle.Bottom:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        0,
                        90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;

                case RoundStyle.BottomLeft:
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    path.AddLine(
                        rect.Right - radiusCorrection,
                        rect.Y,
                        rect.Right - radiusCorrection,
                        rect.Bottom - radiusCorrection);
                    break;

                case RoundStyle.BottomRight:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        0,
                        90);
                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
            }
            path.CloseFigure();

            return path;
        }

        #endregion 建立带有圆角样式的路径

        #region 根据控件对齐方式，圆角大小绘制路径

        /// <summary>根据控件对齐方式，圆角大小绘制路径</summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <param name="tabAlignment"></param>
        /// <returns></returns>
        public static GraphicsPath CreateTabPath(Rectangle rect, int radius, TabAlignment tabAlignment)
        {
            GraphicsPath path = new GraphicsPath();
            switch (tabAlignment)
            {
                case TabAlignment.Top:
                    rect.X++;
                    rect.Width -= 2;
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius / 2);
                    path.AddArc(rect.X, rect.Y, radius, radius, 180F, 90F);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270F, 90F);
                    path.AddLine(rect.Right, rect.Y + radius / 2, rect.Right, rect.Bottom);
                    break;

                case TabAlignment.Bottom:
                    rect.X++;
                    rect.Width -= 2;
                    path.AddLine(rect.X, rect.Y, rect.X, rect.Bottom - radius / 2);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 180, -90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 90, -90);
                    path.AddLine(rect.Right, rect.Bottom - radius / 2, rect.Right, rect.Y);
                    break;

                case TabAlignment.Left:
                    rect.Y++;
                    rect.Height -= 2;
                    path.AddLine(rect.Right, rect.Y, rect.X + radius / 2, rect.Y);
                    path.AddArc(rect.X, rect.Y, radius, radius, 270F, -90F);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 180F, -90F);
                    path.AddLine(rect.X + radius / 2, rect.Bottom, rect.Right, rect.Bottom);
                    break;

                case TabAlignment.Right:
                    rect.Y++;
                    rect.Height -= 2;
                    path.AddLine(rect.X, rect.Y, rect.Right - radius / 2, rect.Y);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270F, 90F);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0F, 90F);
                    path.AddLine(rect.Right - radius / 2, rect.Bottom, rect.X, rect.Bottom);
                    break;
            }
            path.CloseFigure();
            return path;
        }

        #endregion 根据控件对齐方式，圆角大小绘制路径

        #region 建立滑块路径

        /// <summary>建立滑块路径</summary>
        /// <param name="rect"></param>
        /// <param name="arrowDirection"></param>
        /// <returns></returns>
        public static GraphicsPath CreateTrackBarTickPath(Rectangle rect, TrackTickArrowDirection arrowDirection)
        {
            GraphicsPath path = new GraphicsPath();
            PointF centerPoint = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
            float offset = 0;
            switch (arrowDirection)
            {
                case TrackTickArrowDirection.Left:
                case TrackTickArrowDirection.Right:
                    offset = rect.Width / 2f - 4;
                    break;

                case TrackTickArrowDirection.Up:
                case TrackTickArrowDirection.Down:
                    offset = rect.Height / 2f - 4;
                    break;
            }
            switch (arrowDirection)
            {
                case TrackTickArrowDirection.Left:
                    path.AddLine(
                        rect.X, centerPoint.Y, rect.X + offset, rect.Y);
                    path.AddLine(
                        rect.Right, rect.Y, rect.Right, rect.Bottom);
                    path.AddLine(
                        rect.X + offset, rect.Bottom, rect.X, centerPoint.Y);
                    break;

                case TrackTickArrowDirection.Right:
                    path.AddLine(rect.Right, centerPoint.Y, rect.Right - offset, rect.Bottom);
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y);
                    path.AddLine(rect.Right - offset, rect.Y, rect.Right, centerPoint.Y);
                    break;

                case TrackTickArrowDirection.Up:
                    path.AddLine(centerPoint.X, rect.Y, rect.X, rect.Y + offset);
                    path.AddLine(rect.X, rect.Bottom, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Y + offset, centerPoint.X, rect.Y);
                    break;

                case TrackTickArrowDirection.Down:
                    path.AddLine(centerPoint.X, rect.Bottom, rect.X, rect.Bottom - offset);
                    path.AddLine(rect.X, rect.Y, rect.Right, rect.Y);
                    path.AddLine(rect.Right, rect.Bottom - offset, centerPoint.X, rect.Bottom);
                    break;

                case TrackTickArrowDirection.LeftRight:
                    break;

                case TrackTickArrowDirection.UpDown:
                    break;

                case TrackTickArrowDirection.None:
                    path.AddRectangle(rect);
                    break;
            }
            path.CloseFigure();
            return path;
        }

        #endregion 建立滑块路径

        /// <summary>转换成圆角</summary>
        /// <param name="rectf">要转换的rectf</param>
        /// <param name="leftTopRadius">左上角</param>
        /// <param name="rightTopRadius">右上角</param>
        /// <param name="rightBottomRadius">右下角</param>
        /// <param name="leftBottomRadius">左下角</param>
        /// <returns></returns>
        public static GraphicsPath TransformCircular(RectangleF rectf, float leftTopRadius = 0f, float rightTopRadius = 0f, float rightBottomRadius = 0f, float leftBottomRadius = 0f)
        {
            GraphicsPath gp = new GraphicsPath();
            if (leftTopRadius > 0)
            {
                RectangleF lefttop_rect = new RectangleF(rectf.X, rectf.Y, leftTopRadius * 2, leftTopRadius * 2);
                gp.AddArc(lefttop_rect, 180, 90);
            }
            else
            {
                gp.AddLine(new PointF(rectf.X, rectf.Y), new PointF(rightTopRadius > 0 ? rectf.Right - rightTopRadius * 2 : rectf.Right, rectf.Y));
            }
            if (rightTopRadius > 0)
            {
                RectangleF righttop_rect = new RectangleF(rectf.Right - rightTopRadius * 2, rectf.Y, rightTopRadius * 2, rightTopRadius * 2);
                gp.AddArc(righttop_rect, 270, 90);
            }
            else
            {
                gp.AddLine(new PointF(rectf.Right, rectf.Y), new PointF(rectf.Right, rightBottomRadius > 0 ? rectf.Bottom - rightTopRadius * 2 : rectf.Bottom));
            }
            if (rightBottomRadius > 0)
            {
                RectangleF rightbottom_rect = new RectangleF(rectf.Right - rightTopRadius * 2, rectf.Bottom - rightTopRadius * 2, rightBottomRadius * 2, rightBottomRadius * 2);
                gp.AddArc(rightbottom_rect, 0, 90);
            }
            else
            {
                gp.AddLine(new PointF(rectf.Right, rectf.Bottom), new PointF(leftBottomRadius > 0 ? leftBottomRadius * 2 : rectf.X, rectf.Bottom));
            }
            if (leftBottomRadius > 0)
            {
                RectangleF rightbottom_rect = new RectangleF(rectf.X, rectf.Bottom - leftBottomRadius * 2, leftBottomRadius * 2, leftBottomRadius * 2);
                gp.AddArc(rightbottom_rect, 90, 90);
            }
            else
            {
                gp.AddLine(new PointF(rectf.X, rectf.Bottom), new PointF(rectf.X, leftTopRadius > 0 ? rectf.X + leftTopRadius * 2 : rectf.X));
            }
            gp.CloseAllFigures();
            return gp;
        }
    }

    #endregion 创建绘制路径
}