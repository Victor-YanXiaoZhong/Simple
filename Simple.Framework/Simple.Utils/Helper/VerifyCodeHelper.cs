using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Simple.Utils.Helper
{
    /// <summary>验证码助手</summary>
    public class VerifyCodeHelper
    {
        private static readonly Random random;

        static VerifyCodeHelper()
        {
            random = new Random();
        }

        /// <summary>生成随机浅颜色</summary>
        /// <returns></returns>
        private static Color GetRandomLightColor()
        {
            int nRed, nGreen, nBlue;    //越大颜色越浅
            int low = 180;           //色彩的下限
            int high = 255;          //色彩的上限
            nRed = random.Next(high) % (high - low) + low;
            nGreen = random.Next(high) % (high - low) + low;
            nBlue = random.Next(high) % (high - low) + low;
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }

        /// <summary>生成随机深颜色</summary>
        /// <returns></returns>
        private static Color GetRandomDeepColor()
        {
            int nRed, nGreen, nBlue;    // nBlue,nRed  nGreen 相差大一点 nGreen 小一些
                                        //int high = 255;
            int redLow = 120;
            int greenLow = 0;
            int blueLow = 120;
            nRed = random.Next(redLow);
            nGreen = random.Next(greenLow);
            nBlue = random.Next(blueLow);
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }

        /// <summary>生成验证码</summary>
        /// <param name="codeLength">默认的Code数据长度 默认为4</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static (string code, Bitmap img) CreateVerifyCode(int codeLength = 4)
        {
            string precode;
            //建立Bitmap对象，绘图
            Bitmap bitmap = new Bitmap(codeLength * 40, 60);
            Graphics graph = Graphics.FromImage(bitmap);
            graph.FillRectangle(new SolidBrush(GetRandomLightColor()), 0, 0, codeLength * 40, 60);
            Font font = new Font(FontFamily.GenericSerif, 48, FontStyle.Bold, GraphicsUnit.Pixel);
            Random r = new Random();
            string letters = "ABCDEFGHIJKLMNPRSTUVWXY23456789";

            StringBuilder sb = new StringBuilder();

            //添加随机的N个字母
            for (int x = 0; x < codeLength; x++)
            {
                string letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
                sb.Append(letter);
                graph.DrawString(letter, font, new SolidBrush(GetRandomDeepColor()), x * 38, r.Next(0, 15));
            }
            precode = sb.ToString();

            //混淆背景
            for (int x = 0; x < 6; x++)
            {
                Pen linePen = new Pen(new SolidBrush(GetRandomLightColor()), 2);
                graph.DrawLine(linePen, new Point(r.Next(0, codeLength * 50), r.Next(0, 59)), new Point(r.Next(0, codeLength * 50), r.Next(0, 59)));
            }
            return (precode, bitmap);
        }
    }
}