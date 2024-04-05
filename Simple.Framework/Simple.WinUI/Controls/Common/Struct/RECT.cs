using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls
{
    #region 系统矩形
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 系统矩形
    /// </summary>
    public struct RECT
    {
        #region 位置
        /// <summary>
        /// 位置
        /// </summary>
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="rect"></param>
        public RECT(Rectangle rect)
        {
            Left = rect.Left;
            Top = rect.Top;
            Right = rect.Right;
            Bottom = rect.Bottom;
        }
        #endregion

        #region 返回矩形
        /// <summary>
        /// 返回矩形
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return new Rectangle(
                    Left,
                    Top,
                    Right - Left,
                    Bottom - Top);
            }
        }
        #endregion

        #region 返回尺寸
        /// <summary>
        /// 返回尺寸
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(Right - Left, Bottom - Top);
            }
        }
        #endregion

        #region 根据坐标和宽高生成矩形
        /// <summary>
        /// 根据坐标和宽高生成矩形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static RECT FromXYWH(
            int x, int y, int width, int height)
        {
            return new RECT(x,
                            y,
                            x + width,
                            y + height);
        }
        #endregion

        #region 根据Rectangle生成矩形
        /// <summary>
        /// 根据Rectangle生成矩形
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static RECT FromRectangle(Rectangle rect)
        {
            return new RECT(rect.Left,
                             rect.Top,
                             rect.Right,
                             rect.Bottom);
        }
        #endregion
    }
    #endregion
}
