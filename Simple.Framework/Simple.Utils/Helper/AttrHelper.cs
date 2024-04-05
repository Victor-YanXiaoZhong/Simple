using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils.Helper
{
    /// <summary>
    /// 特性帮助类
    /// </summary>
    public static class AttrHelper
    {
        static BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 获取类型的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T? GetMemberAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttribute<T>(false);
        }

        /// <summary>
        /// 获取类型的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T? GetMemberAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttribute<T>(false);
        }
    }
}
