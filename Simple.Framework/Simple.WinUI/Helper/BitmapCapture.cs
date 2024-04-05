using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI
{
    #region  图片裁剪
    /// <summary>
    /// 图片裁剪
    /// </summary>
    public class BitmapCapture
    {
        #region  将一张图片裁剪为多个
        /// <summary>
        /// 将一张图片裁剪为多个
        /// </summary>
        /// <param name="original">原始图片</param>
        /// <param name="x">横向分割多少份</param>
        /// <param name="y">纵向分割多少份</param>
        /// <returns></returns>
        public static Bitmap[,] Capture(Bitmap original, int x, int y)
        {
            Bitmap[,] array = new Bitmap[y, x];
            int num = original.Width / x;
            int num2 = original.Height / y;
            original.SetResolution(96f, 96f);
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Bitmap bitmap = new Bitmap(num, num2);
                    Rectangle rect = new Rectangle(j * num, i * num2, num, num2);
                    bitmap = original.Clone(rect, original.PixelFormat);
                    array[i, j] = bitmap;
                    //bitmap.Dispose();
                }
            }
            return array;
        }
        #endregion
    }
    #endregion
}
