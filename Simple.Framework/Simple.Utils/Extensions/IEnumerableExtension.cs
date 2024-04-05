using System.Text;

namespace Simple.Utils
{
    /// <summary>IEnumerable扩展方法</summary>
    public static class IEnumerableExtension
    {
        #region 常用判断

        /// <summary>判断 ICollection&lt;T&gt; 实例是否不为空</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要判断的对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsNotEmpty<T>(this ICollection<T> source)
        {
            return source != null && source.Count > 0;
        }

        /// <summary>判断 ICollection&lt;T&gt; 实例是否为空</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要判断的对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count < 1;
        }

        /// <summary>判断 IEnumerabe 实例是否不为空</summary>
        /// <param name="source">要判断的IEnumerable对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsNotEmpty(this System.Collections.IEnumerable source)
        {
            return !IsEmpty(source);
        }

        /// <summary>判断 IEnumerabe 实例是否空</summary>
        /// &gt;
        /// <param name="source">要判断的IEnumerable对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsEmpty(this System.Collections.IEnumerable source)
        {
            if (source != null)
            {
                var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext()) { return false; }
            }
            return true;
        }

        /// <summary>判断 IEnumerable&lt;T&gt; 是否不为空</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要判断的 IEnumerable&lt;T&gt; 对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return !IsEmpty(source);
        }

        /// <summary>判断 IEnumerable&lt;T&gt; 是否为空</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要判断的 IEnumerable&lt;T&gt; 对象</param>
        /// <returns>Boolean 值</returns>
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source != null)
            {
                using (IEnumerator<T> enumerator = source.GetEnumerator())
                {
                    while (enumerator.MoveNext()) { return false; }
                }
            }
            return true;
        }

        /// <summary>确定序列是否包含指定的元素使用的默认相等比较器</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要在序列中定位的值</param>
        /// <param name="values">要在其中定位某个值的序列</param>
        /// <returns>true 如果源序列包含具有指定的值，否则为 false</returns>
        public static Boolean IsIn<T>(this T source, IEnumerable<T> values)
        {
            return source != null && values != null && values.Contains(source);
        }

        /// <summary>确定值是否在某个区间内（素使用的默认相等比较器）</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">要在区间中定位的值</param>
        /// <param name="lowerBound">区间开始值</param>
        /// <param name="upperBound">区间结束值</param>
        /// <returns>true 如果值在指定区间内，否则为 false</returns>
        public static Boolean IsInRange<T>(this T source, T lowerBound, T upperBound) where T : IComparable<T>
        {
            return source != null && source.CompareTo(lowerBound) >= 0 && source.CompareTo(upperBound) <= 0;
        }

        #endregion 常用判断

        #region 筛选、遍历

        /// <summary>集合筛选（已进行null检查）</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="predicate">筛选条件</param>
        /// <returns>筛选结果：IEnumerable&lt;T&gt; 实例</returns>
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, Boolean> predicate)
        {
            return source == null || predicate == null ? null : source.Where(predicate);
        }

        /// <summary>集合遍历</summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="action">遍历每项时要执行的方法</param>
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach (T item in source)
                {
                    action(item);
                }
            }
        }

        #endregion 筛选、遍历

        #region 串联（类似String.Join）

        private static Boolean JoinItem<T>(StringBuilder sb, T item, String joiner)
        {
            return item == null ? false : JoinItem(sb, item.ToString(), joiner);
        }

        private static Boolean JoinItem<T>(StringBuilder sb, T item, Func<T, Boolean> predicate, String joiner)
        {
            return item == null || !predicate(item) ? false : JoinItem(sb, item.ToString(), joiner);
        }

        private static Boolean JoinItem<T>(StringBuilder sb, T item, Func<T, String> func, String joiner)
        {
            return item == null ? false : JoinItem(sb, func(item), joiner);
        }

        private static Boolean JoinItem(StringBuilder sb, String value, String joiner)
        {
            Boolean falg = !String.IsNullOrWhiteSpace(value);
            if (falg) { sb.Append(joiner).Append(value); }
            return falg;
        }

        /// <summary>串联集合的成员，并在每个成员之间使用指定的分隔符（忽略满足 String.IsNullOrWhiteSpace 条件的成员）</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">包含要串联对象的集合</param>
        /// <param name="joiner">连接符</param>
        /// <returns>串联结果</returns>
        public static String Join<T>(this IEnumerable<T> source, String joiner = ",")
        {
            if (source == null) { return String.Empty; }

            using (IEnumerator<T> en = source.GetEnumerator())
            {
                if (!en.MoveNext()) { return String.Empty; }
                if (joiner == null) { joiner = String.Empty; }
                var sb = new StringBuilder();

                // 第一项不添加分隔符
                if (!JoinItem(sb, en.Current, String.Empty))
                {
                    while (en.MoveNext())
                    {
                        if (JoinItem(sb, en.Current, String.Empty)) { break; }
                    }
                }

                while (en.MoveNext()) { JoinItem(sb, en.Current, joiner); }
                return sb.ToString();
            }
        }

        /// <summary>串联集合的成员，并在每个成员之间使用指定的分隔符（忽略满足 String.IsNullOrWhiteSpace 条件的成员）</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">包含要串联对象的集合</param>
        /// <param name="predicate">判定函数委托</param>
        /// <param name="joiner">连接符</param>
        /// <returns>串联结果</returns>
        public static String Join<T>(this IEnumerable<T> source, Func<T, Boolean> predicate, String joiner = ",")
        {
            if (source == null || predicate == null) { return String.Empty; }
            using (IEnumerator<T> en = source.GetEnumerator())
            {
                if (!en.MoveNext()) { return String.Empty; }
                if (joiner == null) { joiner = String.Empty; }

                var sb = new StringBuilder();

                // 第一项不添加分隔符
                if (!JoinItem(sb, en.Current, predicate, String.Empty))
                {
                    while (en.MoveNext())
                    {
                        if (JoinItem(sb, en.Current, predicate, String.Empty))
                        {
                            break;
                        }
                    }
                }

                while (en.MoveNext()) { JoinItem(sb, en.Current, predicate, joiner); }
                return sb.ToString();
            }
        }

        /// <summary>串联集合的成员，并在每个成员之间使用指定的分隔符（忽略满足 String.IsNullOrWhiteSpace 条件的成员）</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">包含要串联对象的集合</param>
        /// <param name="func">处理函数委托</param>
        /// <param name="joiner">连接符</param>
        /// <returns>串联结果</returns>
        public static String Join<T>(this IEnumerable<T> source, Func<T, String> func, String joiner = ",")
        {
            if (source == null || func == null) { return String.Empty; }

            using (IEnumerator<T> en = source.GetEnumerator())
            {
                if (!en.MoveNext()) { return String.Empty; }
                if (joiner == null) { joiner = String.Empty; }

                var sb = new StringBuilder();
                // 第一项不添加分隔符
                if (!JoinItem(sb, en.Current, func, String.Empty))
                {
                    while (en.MoveNext())
                    {
                        if (JoinItem(sb, en.Current, func, String.Empty))
                        {
                            break;
                        }
                    }
                }

                while (en.MoveNext()) { JoinItem(sb, en.Current, func, joiner); }
                return sb.ToString();
            }
        }

        #endregion 串联（类似String.Join）

        #region URL查询参数（IEnumerable<KeyValuePair<String, String>）

        /// <summary>集合转URL查询参数key1=value1&amp;key2=value2...，不作URL编码</summary>
        /// <param name="source">集合</param>
        /// <returns>URL查询参数</returns>
        public static String ToUrlQueryParam(this IEnumerable<KeyValuePair<String, String>> source)
        {
            if (source == null) { return String.Empty; }
            using (IEnumerator<KeyValuePair<String, String>> en = source.GetEnumerator())
            {
                if (!en.MoveNext()) { return String.Empty; }
                var item = en.Current;
                var index = 0;
                var length = source.Count() * 4 - 1; // 首项三个元素，其余每项4个元素
                var values = new String[length];

                values[index++] = item.Key;
                values[index++] = "=";
                values[index++] = item.Value;

                while (en.MoveNext())
                {
                    item = en.Current;
                    values[index++] = "&";
                    values[index++] = item.Key;
                    values[index++] = "=";
                    values[index++] = item.Value;
                }
                return String.Concat(values);
            }
        }

        /// <summary>集合转URL查询参数key1=value1&amp;key2=value2...，不作URL编码， key或value满足String.IsNullOrWhiteSpace对应项将被忽略</summary>
        /// <param name="source">集合</param>
        /// <returns>String</returns>
        public static String ToUrlQueryParamIgnoreEmpty(this IEnumerable<KeyValuePair<String, String>> source)
        {
            if (source == null) { return String.Empty; }
            using (IEnumerator<KeyValuePair<String, String>> en = source.GetEnumerator())
            {
                if (!en.MoveNext()) { return String.Empty; }
                var item = en.Current;
                var sb = new StringBuilder();

                // 添加第一项（key或value无效时需向下遍历）
                if (String.IsNullOrWhiteSpace(item.Key) || String.IsNullOrWhiteSpace(item.Value))
                {
                    while (en.MoveNext())
                    {
                        item = en.Current;
                        if (!String.IsNullOrWhiteSpace(item.Key) && !String.IsNullOrWhiteSpace(item.Value))
                        {
                            sb.Append(item.Key).Append('=').Append(item.Value);
                            break;
                        }
                    }
                }
                else
                {
                    sb.Append(item.Key).Append('=').Append(item.Value);
                }

                // 添加其它项
                while (en.MoveNext())
                {
                    item = en.Current;
                    if (!String.IsNullOrWhiteSpace(item.Key) && !String.IsNullOrWhiteSpace(item.Value))
                    {
                        sb.Append('&').Append(item.Key).Append('=').Append(item.Value);
                    }
                }
                return sb.ToString();
            }
        }

        #endregion URL查询参数（IEnumerable<KeyValuePair<String, String>）

        #region Xml

        /// <summary>集合转Xml字符转</summary>
        /// <param name="source">集合</param>
        /// <returns>Xml字符串</returns>
        public static String ToXml(this IEnumerable<KeyValuePair<String, String>> source)
        {
            var sb = new StringBuilder().Append("<xml>");
            if (source != null)
            {
                using (IEnumerator<KeyValuePair<String, String>> en = source.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        var item = en.Current;
                        sb.Append("<").Append(item.Key).Append(">")
                          .Append(item.Value)
                          .Append("</").Append(item.Key).Append(">");
                    }
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion Xml

        #region List<Int32>

        /// <summary>字符串集合转整形集合</summary>
        /// <param name="source">字符串集合</param>
        /// <returns>Int32集合</returns>
        public static List<Int32> ToInt32(this IEnumerable<String> source)
        {
            List<Int32> result = new List<Int32>();
            if (source == null) { return result; }
            using (IEnumerator<String> en = source.GetEnumerator())
            {
                Int32 value;
                while (en.MoveNext())
                {
                    if (Int32.TryParse(en.Current, out value))
                    {
                        result.Add(value);
                    }
                }
            }
            return result;
        }

        #endregion List<Int32>
    }
}