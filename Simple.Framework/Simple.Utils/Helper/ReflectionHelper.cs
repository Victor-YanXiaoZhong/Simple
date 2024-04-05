using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils.Helper
{
    /// <summary>反射帮助类</summary>
    public static class ReflectionHelper
    {
        private static readonly Type stringDictType = typeof(IDictionary<String, String>);

        /// <summary>实体类转字典</summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体类对象</param>
        /// <exception cref="Exception">转换异常</exception>
        /// <returns>IDictionary&lt;String, Object&lt; 实例</returns>
        public static IDictionary<String, Object> GetDictionary<T>(T entity) where T : class, new()
        {
            var result = new Dictionary<String, Object>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                String name = property.Name;
                result[name] = property.GetValue(entity, null);
            }
            return result;
        }

        /// <summary>
        /// 对象转字典
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="Exception">转换异常</exception>
        /// <returns>IDictionary&lt;String, Object&lt; 实例</returns>
        public static IDictionary<string, object> GetDictionary(Object obj)
        {
            if (!(obj is IDictionary<String, Object> result))
            {
                result = new Dictionary<String, Object>();
                Type objType = obj.GetType();
                if (objType == stringDictType && (obj is IDictionary<String, String> dict))
                {
                    foreach (var item in dict)
                    {
                        result[item.Key] = item.Value;
                    }

                }
                else
                {
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        String name = property.Name;
                        result[name] = property.GetValue(obj, null);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 设置模型类指定字段的值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEntity SetEntityValue<TEntity>(this TEntity entity, string name, object value) where TEntity : class
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            var property = properties.FirstOrDefault(p => p.Name == name);
            if (property is null)
            {
                throw new Exception($"实体中不存在属性：{name}");
            }
            property.SetValue(entity, value);
            return entity;
        }

        /// <summary>
        /// 获取模型类指定字段的值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object? GetEntityValue<TEntity>(this TEntity entity, string name) where TEntity : class
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();

            var property = properties.FirstOrDefault(p => p.Name == name);
            if (property is null)
            {
                throw new Exception($"实体中不存在属性：{name}");
            }
            return property.GetValue(entity);
        }
    }
}
