using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Simple.Utils;
using Simple.Utils.Helper;
using Simple.Utils.Models;
using System.Text;

namespace Simple.AspNetCore.Filters
{
    /// <summary>鉴权过滤器</summary>
    public class AuthenticationFilter : IAuthorizationFilter
    {
        private readonly IDistributedCache cache;

        public AuthenticationFilter(IDistributedCache _cache)
        {
            cache = _cache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(p => p is IAllowAnonymousFilter)) return;
            //if (context.Filters.Any(p => p is AuthorizeAttribute)) return;
            var actDes = context.ActionDescriptor as ControllerActionDescriptor;
            var tokenKey = ConfigHelper.GetValue("TokenHeadKey");
            if (tokenKey.IsNullOrEmpty())
                tokenKey = "Authorization";
            var token = context.HttpContext.Request.Headers[tokenKey].ToString();

            if (token.IsNullOrEmpty())
            {
                Set401Result(context);
                return;
            }

            var cacheValue = Encoding.UTF8.GetString(cache.Get(token));
            if (cacheValue.IsNullOrEmpty())
                Set401Result(context, "权限已过期");
        }

        /// <summary>设置401 没有权限的返回</summary>
        /// <param name="context"></param>
        public void Set401Result(AuthorizationFilterContext context, string message = "没有获取到权限信息")
        {
            var result = new ApiResult
            {
                Code = 401,
                IsSuccess = false,
                Message = message
            };

            context.HttpContext.Response.StatusCode = 200;
            context.Result = new JsonResult(result);
            return;
        }
    }
}