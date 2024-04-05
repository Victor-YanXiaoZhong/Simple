using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Simple.AspNetCore.Helper;
using Simple.Utils;
using Simple.Utils.Helper;
using Simple.Utils.Models.BO;
using System.Reflection;

namespace Simple.AspNetCore.Controllers
{
    /// <summary>具有鉴权的控制器，能获取到登录的用户信息,并直接拒绝无权限的访问</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AppAuthController : Controller
    {
        private readonly IDistributedCache cache;

        private readonly ContentResult Set401Result = new ContentResult
        {
            Content = "{\"code\":401,\"message\":\"您需要登录后才能操作\"}",
            ContentType = "application/json"
        };

        /// <summary>登录用户信息</summary>
        protected LoginUserBO LoginUser;

        public AppAuthController()
        {
            cache = HostServiceExtension.ServiceProvider.GetService<IDistributedCache>();
        }

        /// <summary>鉴权</summary>
        /// <param name="context"></param>
        private void OnAuthoring(ActionExecutingContext context)
        {
            bool isLogined = false;
            var descriptor = context.ActionDescriptor;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            var endpoint = context.HttpContext.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                return;
            }

            if (descriptor.FilterDescriptors.Count(x => x.Filter is AllowAnonymousFilter) != 0)
            {
                return;
            }

            var token = string.Empty;
            var tokenKey = ConfigHelper.GetValue("TokenHeadKey");

            //未设置tokenkey 则认为session认证
            if (tokenKey.IsNullOrEmpty())
            {
                isLogined = SessionLoginAuth(context.HttpContext);
            }
            else
            {
                request.Cookies.TryGetValue(tokenKey, out token);

                if (string.IsNullOrEmpty(token))
                {
                    token = request.Headers[tokenKey];
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    isLogined = false;
                }

                //设置登录的账户
                if (!string.IsNullOrWhiteSpace(token))
                {
                    isLogined = LoginUserAuth(token);
                }
            }

            if (!isLogined)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = Set401Result;
                return;
            }
            //验证是否有权限访问控制器
            if (!HasPermissionAuth((ControllerActionDescriptor)descriptor, out string message))
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = SetNoPermissionResult(message);
                return;
            };
        }

        private ContentResult SetNoPermissionResult(string message = "")
        {
            return new ContentResult
            {
                Content = $"{{\"code\":403,\"message\":\"您没有{message}访问的权限\"}}",
                ContentType = "application/json"
            };
        }

        protected bool SessionLoginAuth(HttpContext httpContext)
        {
            LoginUser = httpContext.Session.Get<LoginUserBO>("AuthUser");
            return LoginUser != null;
        }

        /// <summary>在此鉴权，并设置登录的用户信息</summary>
        /// <param name="token">authrization token 前缀携带有Bearer加空格</param>
        /// <returns></returns>
        protected bool LoginUserAuth(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var payLoad = JWTHelper.GetPayload(token);
            LoginUser = new LoginUserBO
            {
                UserAccount = payLoad.UserAccount,
                UserID = payLoad.UserID,
                OrgnizationId = payLoad.OrgnizationId,
                RoleId = payLoad.RoleId,
                UserName = payLoad.UserName,
                SupperAdmin = payLoad.SupperAdmin,
                AdminOrg = payLoad.AdminOrg
            };
            return true;
        }

        /// <summary>根据控制器信息，判断是否有访问控制器或方法的权限</summary>
        /// <param name="descriptor"></param>
        /// <param name="message">权限异常的信息</param>
        /// <returns>True 为拥有权限</returns>
        protected bool HasPermissionAuth(ControllerActionDescriptor descriptor, out string message)
        {
            var controllerName = descriptor.ControllerName.ToLower();
            var action = descriptor.ActionName;

            var actionPermission = descriptor.MethodInfo.GetCustomAttributes<PermissionAttribute>().FirstOrDefault();
            var funSign = string.Empty;
            message = string.Empty;
            if (actionPermission == null)
            {
                return true;
            }
            funSign += controllerName.ToUpper() + "." + action.ToUpper();

            var userPermission = RedisHelper.StringGet<string[]>("UserPermission:" + LoginUser.UserAccount);

            if (!userPermission.Contains(funSign))
            {
                message = controllerName + actionPermission.Description;
                return false;
            }

            return true;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            OnAuthoring(context);
        }
    }
}