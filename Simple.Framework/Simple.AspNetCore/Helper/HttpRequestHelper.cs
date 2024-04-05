using Microsoft.AspNetCore.Http;
using Simple.Utils;
using System.Text;

namespace Simple.AspNetCore
{
    /// <summary>HttpRequest请求帮助类</summary>
    public static class HttpRequestHelper
    {
        /// <summary>获取请求体</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">请求上下文</param>
        /// <param name="allowMulitRead">是否允许重复读取</param>
        /// <returns></returns>
        public static T? GetBodyModel<T>(this HttpRequest request, bool allowMulitRead = true) where T : class, new()
        {
            var data = request.GetBody(allowMulitRead);
            return JsonHelper.FromJson<T>(data);
        }

        /// <summary>获取请求体</summary>
        /// <param name="request">请求上下文</param>
        /// <param name="allowMulitRead">是否允许重复读取</param>
        /// <returns></returns>
        public static string GetBody(this HttpRequest request, bool allowMulitRead = true)
        {
            var length = request.ContentLength.Value.ToInt();
            using (var writer = new StreamReader(request.Body, Encoding.UTF8, true, length, true))
            {
                var data = writer.ReadToEnd();
                if (allowMulitRead)
                    request.Body.Position = 0;//以后可以重复读取
                return data;
            }
        }
    }
}