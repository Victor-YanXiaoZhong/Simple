using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils.Helper
{
    /// <summary>
    /// 泛型映射工具类
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <typeparam name="F">目标类型</typeparam>
    public static class MapperHelper<T, F>
    {
        /// <summary>
        /// 私有静态字段
        /// </summary>
        private static readonly Func<T, F> map = MapProvider();

        /// <summary>
        /// 私有方法
        /// </summary>
        /// <returns></returns>
        private static Func<T, F> MapProvider()
        {
            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var memberBindingList = new List<MemberBinding>();
            foreach (var item in typeof(F).GetProperties())
            {
                if (!item.CanWrite)
                    continue;
                var property = Expression.Property(parameterExpression, typeof(T).GetProperty(item.Name));
                var memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }
            var memberInitExpression = Expression.MemberInit(Expression.New(typeof(F)), memberBindingList.ToArray());
            var lambda = Expression.Lambda<Func<T, F>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
            return lambda.Compile();
        }

        /// <summary>
        /// 映射方法
        /// </summary>
        /// <param name="entity">待映射的对象</param>
        /// <returns>目标类型对象</returns>
        public static F MapTo(T entity)
        {
            return map(entity);
        }
    }
}
