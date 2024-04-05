using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.ContextMenuStrip
{
    #region ToolStrip渲染器
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// ToolStrip渲染器
    /// </summary>
    public class ToolStripProfessionRenderer : ToolStripRenderer
    {
        #region 当前项选中背景颜色
        /// <summary>
        /// 当前项选中背景颜色
        /// </summary>
        private Color _currentColor;
        #endregion

        #region 透明度
        /// <summary>
        /// 透明度
        /// </summary>
        private int _opacity = 55;
        #endregion

        #region 背景颜色
        /// <summary>
        /// 背景颜色
        /// </summary>
        private Color _backColor;
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="current"></param>
        /// <param name="opacity"></param>
        /// <param name="backColor"></param>
        public ToolStripProfessionRenderer(Color current, int opacity, Color backColor)
        {
            this._currentColor = current;
            this._opacity = opacity;
            this._backColor = backColor;
        }
        #endregion

        #region  初始化项
        /// <summary>
        /// 初始化项
        /// </summary>
        /// <param name="item"></param>
        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);

        }
        #endregion

        #region 重绘分割线
        /// <summary>
        /// 重绘分割线
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);
            Graphics g = e.Graphics;
            g.DrawLine(new Pen((_currentColor)), new Point(20, e.Item.Bounds.Height / 2), new Point(e.Item.Bounds.Width - 20, e.Item.Bounds.Height / 2));

        }
        #endregion

        #region 重绘背景
        /// <summary>
        /// 重绘背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            //if (e.AffectedBounds.Width < 200)
            //{
            Graphics g = e.Graphics;
            //填充背景
            SolidBrush solid = new SolidBrush(_backColor);
            Rectangle reactBound = new Rectangle(new Point(1, 1), new Size(e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
            g.FillRectangle(solid, reactBound);

            //画制背景边框
            // Pen pen = new Pen(brush);
            Pen pen = new Pen(_currentColor);
            Rectangle reactBorder = new Rectangle(new Point(0, 0), new Size(e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
            g.DrawRectangle(pen, reactBorder);

            //}

        }
        #endregion

        #region 重绘项背景
        /// <summary>
        /// 重绘项背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);
            if (e.Item.Selected)
            {
                Graphics g = e.Graphics;
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(_opacity, _currentColor));
                g.FillRectangle(solidBrush, new Rectangle(2, 2, e.Item.Width - 3, e.Item.Height - 3));

            }

        }
        #endregion

        #region 创建绘制路径
        /// <summary>
        /// 创建绘制路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cornerRadius"></param>
        /// <returns></returns>
        public GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
        #endregion

    }
    #endregion
}
