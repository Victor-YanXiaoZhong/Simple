using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Simple.Utils
{
    /// <summary>linq 扩展方法</summary>
    public static class QueryableExtensions
    {
        /// <summary>Linq If 条件判断语句 IfWhere(条件,p=&gt;xxx)</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable 源数据</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">条件表达式</param>
        /// <returns>返回加了条件的 IQueryable数据源</returns>
        public static IQueryable<T> IfWhere<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>Linq If 条件判断语句 IfWhere(条件,p=&gt;xxx) EF 禁止这么使用</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IEnumerable 源数据</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">条件表达式</param>
        /// <returns>返回加了条件的 IEnumerable数据源</returns>
        public static IEnumerable<T> IfWhere<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>排序 Id desc</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sorts">排序对象</param>
        /// <returns></returns>
        public static IQueryable<T> @Sort<T>(this IQueryable<T> query, IEnumerable<Sort> sorts)
        {
            foreach (var orderinfo in sorts)
            {
                var t = typeof(T);
                var propertyInfo = t.GetProperty(orderinfo.field);
                var parameter = Expression.Parameter(t);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                var orderby = Expression.Lambda<Func<T, object>>(propertySelector, parameter);
                if (orderinfo.sort.ToUpper() == "DESC")
                    query = query.OrderByDescending(orderby);
                else
                    query = query.OrderBy(orderby);
            }
            return query;
        }

        /// <summary>排序 Id desc</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sort">Id desc 对象数组</param>
        /// <returns></returns>
        public static IQueryable<T> @Sort<T>(this IQueryable<T> query, string sort)
        {
            var sorts = JsonConvert.DeserializeObject<List<Sort>>(sort);
            foreach (var orderinfo in sorts)
            {
                var t = typeof(T);
                var propertyInfo = t.GetProperty(orderinfo.field);
                var parameter = Expression.Parameter(t);
                Expression propertySelector = Expression.Property(parameter, propertyInfo);

                var orderby = Expression.Lambda<Func<T, object>>(propertySelector, parameter);
                if (orderinfo.sort.ToUpper() == "DESC")
                    query = query.OrderByDescending(orderby);
                else
                    query = query.OrderBy(orderby);
            }
            return query;
        }
    }
}