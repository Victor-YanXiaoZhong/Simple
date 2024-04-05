using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils
{
    /// <summary>
    /// 时间扩展
    /// </summary>
    public static class DateExtension
    {
        /// <summary>
        /// 获取unix时间戳
        /// </summary>
        /// <returns></returns>
        public static int ToUnix(this DateTime date)
        {
            return (int)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }
        /// <summary>
        /// 时间戳转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixToTime(this long timeStamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddMilliseconds(timeStamp).AddHours(8);
        }
        /// <summary>
        /// 时间戳字符串转换为日期（时间戳单位秒）
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixToTime(this string timeStamp)
        {
            return Convert.ToInt64(timeStamp).UnixToTime();
        }
    }
}
