using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.Panel
{
    #region

    public class UPanel : System.Windows.Forms.Panel
    {
        #region 边框颜色

        /// <summary>边框颜色</summary>
        private Color _borderColor = Color.Silver;

        /// <summary>边框宽度</summary>
        private int _borderWidth = 1;

        private int _radius = 0;

        /// <summary>构造</summary>
        public UPanel() : base()
        {
            //首先开启双缓冲，防止闪烁
            //双缓冲的设置 具体参数含义参照msdn的ControlStyles枚举值
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        [Browsable(true)]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        #endregion 边框颜色

        #region 边框宽度
        #endregion 边框宽度

        [Browsable(false)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return BorderStyle.None;
            }
            set
            {
                BorderStyle = BorderStyle.None;
            }
        }

        #region 边框角度

        /// <summary>边框角度</summary>
        [Browsable(true)]
        [Description("边框角度")]
        public int Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                this.Invalidate();
            }
        }

        #endregion 边框角度

        #region 构造
        #endregion 构造

        #region 绘制

        /// <summary>绘制</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen pen = new Pen(_borderColor, _borderWidth);
            SolidBrush solidBrush = new SolidBrush(BackColor);
            Rectangle rectangle = this.ClientRectangle;
            rectangle.X += 0;
            rectangle.Y += 0;

            if (_radius != 0)
            {
                GraphicsPath graphicsPathRegion = GraphicsPathManager.CreatePath(rectangle, _radius, RoundStyle.All, false);
                using (GraphicsPath graphicsPath = GraphicsPathManager.CreatePath(rectangle, _radius, RoundStyle.All, true))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                    e.Graphics.FillPath(solidBrush, graphicsPath);
                    e.Graphics.DrawPath(pen, graphicsPath);
                    e.Dispose();
                    this.Region = new Region(graphicsPathRegion);
                }
            }
            else
            {
                rectangle.Width -= _borderWidth * 2;
                rectangle.Height -= _borderWidth * 2;
                e.Graphics.FillRectangle(solidBrush, rectangle);
                e.Graphics.DrawRectangle(pen, rectangle);
                e.Dispose();
            }
            //    Rectangle rectangle = this.ClientRectangle;
            //rectangle.X += 0;
            //rectangle.Y += 0;
            //rectangle.Width -= _borderWidth*2;
            //rectangle.Height -= _borderWidth*2;
            //e.Graphics.DrawRectangle(pen, rectangle);
            //e.Dispose();
        }

        #endregion 绘制
    }

    #endregion
}