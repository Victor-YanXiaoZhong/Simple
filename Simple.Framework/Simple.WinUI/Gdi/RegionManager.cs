using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI
{
    #region 为指定控件设置区域风格
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 为指定控件设置区域风格
    /// </summary>
    public class RegionManager
    {
        #region 为指定控件设置区域风格
        /// <summary>
        /// 为指定控件设置区域风格
        /// </summary>
        /// <param name="control"></param>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        /// <param name="roundStyle"></param>
        public static void CreateRegion(Control control, Rectangle bounds, int radius, RoundStyle roundStyle)
        {
            using (GraphicsPath path = GraphicsPathManager.CreatePath(bounds, radius, roundStyle, true))
            {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                if (control.Region != null)
                {
                    control.Region.Dispose();
                }
                control.Region = region;
            }
        }
        #endregion

        #region 为指定控件设置区域
        /// <summary>
        /// 为指定控件设置区域
        /// </summary>
        /// <param name="control"></param>
        /// <param name="bounds"></param>
        public static void CreateRegion(Control control, Rectangle bounds)
        {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }
        #endregion
    }
    #endregion
}
