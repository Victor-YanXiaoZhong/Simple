using System.Text;

namespace Simple.Utils.Helper
{
    /// <summary>用于在通信中对数据进行处理的工具类</summary>
    public static class VCTHelper
    {
        #region 字节数据操作

        internal static byte[] HexStringToByteArray(ushort data)
        {
            byte[] buffer = new byte[2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(data & 0x00ff);
                data >>= 8;
            }

            return buffer;
        }

        /// <summary>返回一个byte[]从第index个元素后长度为length的byte[]</summary>
        /// <param name="by"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubByteArray(byte[] by, int index, int length)
        {
            byte[] byr = new byte[length];
            for (int i = index; i < index + length; i++)
            {
                byr[i - index] = by[i];
            }
            return byr;
        }

        /// <summary>返回一个byte[]从第index个元素后的byte[]</summary>
        /// <param name="by"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubByteArray(byte[] by, int index)
        {
            return SubByteArray(by, index, by.Length - index);
        }

        /// <summary>"aabbcc"变为"ccbbaa"</summary>
        /// <param name="str">长度必需是偶数</param>
        /// <returns></returns>
        public static string StrTOStrV(string str)
        {
            string retn = "";
            int length = str.Length;
            for (int i = 0; i <= length - 2; i += 2)
            {
                retn = str.Substring(i, 2) + retn;
            }
            return retn;
        }

        /// <summary>把一个字节数组转化成十六进制的字符串形式，以空格隔开。</summary>
        public static string ByteToHexStr(byte[] da)
        {
            string s = "";
            for (int i = 0; i < da.Length; i++)
            {
                s += Convert.ToString(da[i], 16).PadLeft(2, '0') + " ";
            }
            return s;
        }

        /// <summary>把一个字节数组转化成十六进制的字符串形式</summary>
        public static string ByteToHexStr2(byte[] da)
        {
            string s = "";
            for (int i = 0; i < da.Length; i++)
            {
                s += Convert.ToString(da[i], 16).PadLeft(2, '0');
            }
            return s;
        }

        /// <summary>把诸如：23 fe e3 的字符串转化成byte[]</summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(this string da)
        {
            string sends = da;
            sends = sends.Replace(" ", "");//去掉空格
            sends = sends.Replace("\n", "");
            sends = sends.Replace("\r", "");
            int length = sends.Length / 2;
            byte[] sendb = new byte[length];

            for (int i = 0; i < length; i++)
            {
                sendb[i] = Convert.ToByte(sends.Substring(i * 2, 2), 16);
            }

            return (sendb);
        }

        /// <summary>把字符串转化成byte[]</summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static byte[] StrToByte(this string da)
        {
            return Encoding.Default.GetBytes(da);
        }

        /// <summary>把int转化成byte[]</summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static byte[] IntToByte(this int da)
        {
            return BitConverter.GetBytes(da);
        }

        /// <summary>清理hex字符串数据 把诸如：23 fe e3 的字符串转化成 23FEE3</summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string HexClear(string da)
        {
            string sends = da;
            sends = sends.Replace(" ", "");//去掉空格
            sends = sends.Replace("\n", "");
            sends = sends.Replace("\r", "");
            return sends.ToUpper();
        }

        /// <summary>判断是否数字</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDigit(string str)//判断是否数字
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!(str[i] >= '0' && str[i] <= '9'))
                    return false;
            }
            return true;
        }

        /// <summary>把8位字符byt的从第begin字符到end字符转化为二进制字符串</summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="byt"></param>
        /// <returns></returns>
        public static string BitDivision(int begin, int end, byte byt)
        {
            string str1 = "1";
            str1 = str1.PadRight(end - begin + 1, '1');
            str1 = str1.PadLeft(end, '0');
            str1 = str1.PadRight(8, '0');
            int a = Convert.ToInt32(str1, 2);
            int b = byt;
            int c = a & b;
            c = c >> (8 - end);
            return Convert.ToString(c, 2).PadLeft(end - begin + 1, '0');
        }

        /// <summary>把二进制字符串转化为十六进制字符串.</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSix(string str)
        {
            long l = Convert.ToInt64(str, 2);
            return Convert.ToString(l, 16).PadLeft(2, '0');
        }

        /// <summary>把二进制字符串转化为十六进制字符串. 0X 前缀4位字符</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To0XSix(string str)
        {
            long l = Convert.ToInt64(str, 2);
            return "0x" + Convert.ToString(l, 16).PadLeft(2, '0');
        }

        /// <summary>把二进制字符串转化为十进制字符串</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTen(string str)
        {
            long l = Convert.ToInt64(str, 2);
            return Convert.ToString(l, 10);
        }

        /// <summary>"ef"-----"11101111"</summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string HexStrToBinStr(string hex)
        {
            int l = hex.Length * 4;
            return Convert.ToString(Convert.ToInt32(hex, 16), 2).PadLeft(l, '0');
        }

        /// <summary>
        ///16进制字符串 （高位在前）转整数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int UppHexToInt(this string hex)
        {
            int i = 0;
            try
            {
                i = Convert.ToInt32(hex, 16);
            }
            catch
            {
                return 0;
            }
            return i;
        }

        /// <summary>16进制字符串转 双精度浮点数</summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static double HexToDouble(this string hex)
        {
            try
            {
                var longdd = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                return BitConverter.Int64BitsToDouble(longdd);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///16进制字符串 （低位在前）转浮点数,瞬时量
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float HexToFloat(this string s)
        {
            try
            {
                uint num = uint.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] data = VCTHelper.StrToHexByte(s);
                float f = BitConverter.ToSingle(BitConverter.GetBytes(num), 0);
                return f;
            }
            catch
            {
                return 0f;
            }
        }

        /// <summary>5字节（低位在前）16进制字符串转日期 年-月-日-时-分 19 09 30 09 38</summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime HexToTime(this string s)
        {
            string t = string.Empty;
            try
            {
                int y = Convert.ToInt32(s.Substring(0, 2)) + 2000;
                var m = s.Substring(2, 2);
                var d = s.Substring(4, 2);
                var h = s.Substring(6, 2);
                var mm = s.Substring(8, 2);
                var ss = "00";
                if (s.Length > 10)
                {
                    ss = s.Substring(10, 2);
                }
                t = $"{y}-{m}-{d} {h}:{mm}:{ss}";
            }
            catch
            {
            }
            return DateTime.Parse(t);
        }

        /// <summary>
        ///16进制字符串 （低位在前）转整数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int LowHexToInt(this string hex)
        {
            int i = 0;
            try
            {
                hex = hex.PadRight(8, '0');
                byte[] data = VCTHelper.StrToHexByte(hex);
                i = BitConverter.ToInt32(data, 0);
            }
            catch
            {
                return 0;
            }
            return i;
        }

        public static string HexToUTF8String(this string hex)
        {
            try
            {
                var bytes = hex.StrToHexByte();
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>"1110010"--------"1,1,1,0,0,1,0"</summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string BinDotBin(string bin)
        {
            string rtn = "";
            for (int i = 0; i < bin.Length; i++)
            {
                rtn += bin.Substring(i, 1) + ",";
            }
            return rtn.TrimEnd(new char[] { ',' });
        }

        #endregion 字节数据操作

        #region 字节数组操作

        /// <summary>字节数组连接</summary>
        /// <param name="byte1">子串1</param>
        /// <param name="byte2">子串2</param>
        /// <returns>连接后的字节数组</returns>
        public static byte[] ByteAdd(byte[] byte1, byte[] byte2)
        {
            byte[] data = new byte[byte1.Length + byte2.Length];
            byte1.CopyTo(data, 0);
            byte2.CopyTo(data, byte1.Length);
            return data;
        }

        /// <summary>连接新字节数组</summary>
        /// <param name="left"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] ByteContact(this byte[] left, byte[] bytes)
        {
            return left.Concat(bytes).ToArray();
        }

        /// <summary>字节数组连接</summary>
        /// <param name="byte1">子串1</param>
        /// <param name="byte2">子串2</param>
        /// <param name="byte3">子串3</param>
        /// <returns>连接后的字节数组</returns>
        public static byte[] ByteAdd(byte[] byte1, byte[] byte2, byte[] byte3)
        {
            byte[] bytetemp = ByteAdd(byte1, byte2);
            return ByteAdd(bytetemp, byte3);
        }

        /// <summary>字节数组连接</summary>
        /// <param name="byte1">子串1</param>
        /// <param name="byte2">子串2</param>
        /// <param name="byte3">子串3</param>
        /// <param name="byte4">子串4</param>
        /// <returns>连接后的字节数组</returns>
        public static byte[] ByteAdd(byte[] byte1, byte[] byte2, byte[] byte3, byte[] byte4)
        {
            byte[] temp1 = ByteAdd(byte1, byte2);
            byte[] temp2 = ByteAdd(byte3, byte4);
            return ByteAdd(temp1, temp2);
        }

        /// <summary>从字数组中移除子串</summary>
        /// <param name="data">要移除子串的数组</param>
        /// <param name="pos">要移除的起始位置</param>
        /// <param name="length">移除的长度</param>
        /// <returns>移除子串后的数组</returns>
        public static byte[] ByteRemove(byte[] data, int pos, int length)
        {
            try
            {
                if (length <= 0)
                {
                    throw new Exception("移除的长度必须大于0");
                }
                if (pos < 0 || pos >= data.Length)
                {
                    throw new Exception("起始位置参数必须大于等于0且小于数组长度");
                }
                if (data.Length <= pos + length - 1)
                {
                    throw new Exception("参数错误！");
                }
                byte[] temp = new byte[data.Length - length];

                int j = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (i == pos)
                    {
                        i += length - 1;
                        continue;
                    }
                    temp[j++] = data[i];
                }
                return temp;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        /// <summary>从字节数组中取子串</summary>
        /// <param name="data">要取子字节数组的数据</param>
        /// <param name="pos">起始位置</param>
        /// <returns></returns>
        public static Byte[] ByteSubByte(byte[] data, int pos)
        {
            try
            {
                if (data.Length <= pos || pos < 0)
                {
                    throw new Exception("起始位置必须大于等于0且小于数组长度");
                }

                byte[] temp = new byte[data.Length - pos];

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = data[i + pos];
                }
                return temp;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        /// <summary>从字节数组中取子串</summary>
        /// <param name="data">要取子字节数组的数据</param>
        /// <param name="pos">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>取出的数组</returns>
        public static byte[] ByteSubByte(byte[] data, int pos, int length)
        {
            try
            {
                if (pos >= data.Length || pos < 0)
                {
                    throw new Exception("起始位置必须大于等于0且小于数组长度");
                }
                if (length == 0 || length > data.Length)
                {
                    throw new Exception("子串长度必须大于0且小于数组长度");
                }
                if (data.Length <= pos + length - 1)
                {
                    throw new Exception("参数不正确");
                }
                byte[] temp = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    temp[i] = data[i + pos];
                }
                return temp;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        #endregion 字节数组操作

        #region 字符串操作

        /// <summary>字符串转hex</summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetHexFromString(string s)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";
            }

            byte[] bytes = UTF8Encoding.UTF8.GetBytes(s);

            return ByteToHexStr2(bytes);
        }

        /// <summary>hex字符串 按照2位分割后高低位翻转 123456789000 输出为 00 90 78 56 34 12</summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string HexHLCV(string hex)
        {
            if (hex.Length % 2 != 0) throw new Exception("不是有效的hex字符串");
            var sb = new StringBuilder(hex.Length);
            for (int i = hex.Length - 2; i >= 0; i = i - 2)
            {
                sb.Append(hex.Substring(i, 2));
            }
            return sb.ToString();
        }

        #endregion 字符串操作
    }
}