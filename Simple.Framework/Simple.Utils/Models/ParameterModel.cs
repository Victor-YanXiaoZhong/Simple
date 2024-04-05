namespace Simple.Utils.Models
{
    /// <summary>动态参数基类，参数封装在字典中,FromQuery请求需要使用ParameterModelFromQuery 参数只支持一层，不支持多层的参数</summary>
    public class ParameterModel : Dictionary<string, object>
    {
        /// <summary>判断是否存在字段</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasField(string name)
        {
            return this.ContainsKey(name);
        }

        /// <summary>返回字典中指定key 模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadKey<T>(string key, bool urlDecode = false) where T : class, new()
        {
            var model = default(T);
            try
            {
                var value = this[key];
                if (value is null)
                    throw new CustomException($"字典中 {key}不存在", $"字典中 {key}不存在");
                model = JsonHelper.FromJson<T>(urlDecode ? System.Web.HttpUtility.UrlDecode(value.ToString()) : value.ToString());
            }
            catch (Exception ex)
            {
                throw new CustomException("模型赋值异常", "模型赋值异常", ex);
            }

            return model;
        }

        /// <summary>返回字典中指定模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>() where T : class, new()
        {
            var model = Activator.CreateInstance<T>();
            try
            {
                model.TryFromDict(this);
            }
            catch (Exception ex)
            {
                throw new CustomException("模型赋值异常", "模型赋值异常", ex);
            }

            return model;
        }

        /// <summary>返回字典中指定模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>(T model)
        {
            if (model is null)
                throw new CustomException("模型赋值异常 类型不能为空", "模型赋值异常 类型不能为空");
            try
            {
                model.TryFromDict(this);
            }
            catch (Exception ex)
            {
                throw new CustomException("模型赋值异常", "模型赋值异常", ex);
            }

            return model;
        }
    }

    /// <summary>动态类型参数，FromQuery请求时使用</summary>
    public class ParameterModelFromQuery : Dictionary<string, string>
    {
        /// <summary>判断是否存在字段</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasField(string name)
        {
            return this.ContainsKey(name);
        }

        /// <summary>返回字典中指定key 模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadKey<T>(string key, bool urlDecode = false) where T : class, new()
        {
            var model = default(T);
            try
            {
                var value = this[key];
                if (value is null)
                    throw new CustomException($"字典中 {key}不存在", $"字典中 {key}不存在");
                model = JsonHelper.FromJson<T>(urlDecode ? System.Web.HttpUtility.UrlDecode(value) : value);
            }
            catch (Exception ex)
            {
                throw new CustomException("模型Json赋值异常", "模型Json赋值异常", ex);
            }

            return model;
        }

        /// <summary>返回字典中指定模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>() where T : class, new()
        {
            var model = Activator.CreateInstance<T>();
            try
            {
                model.TryFromDict(this);
            }
            catch (Exception ex)
            {
                throw new CustomException("模型赋值异常", "模型赋值异常", ex);
            }

            return model;
        }

        /// <summary>返回字典中指定模型的值</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>(T model)
        {
            if (model is null)
                throw new CustomException("模型赋值异常 类型不能为空", "模型赋值异常 类型不能为空");
            try
            {
                model.TryFromDict(this);
            }
            catch (Exception ex)
            {
                throw new CustomException("模型赋值异常", "模型赋值异常", ex);
            }

            return model;
        }
    }

    /// <summary>分页请求</summary>
    public class PageRequest
    {
        public PageRequest()
        {
            Filters = new List<Filter>();
        }

        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
        public List<Filter> Filters { get; set; }
    }
}