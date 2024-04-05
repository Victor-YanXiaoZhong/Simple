using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.AdminApplication.Application;

namespace Simple.AdminController.Controllers
{
    [Route("api/[controller]")]
    [ApiController, PermissionGroup("系统管理"), Authorize]
    public class AuthController : ControllerBase
    {
        private readonly SysMainApplication sysMainApplication;
        private readonly AuthService authService;

        public AuthController(SysMainApplication sysMainApplication, AuthService authService)
        {
            this.sysMainApplication = sysMainApplication;
            this.authService = authService;
        }

        /// <summary>登录</summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login"), AllowAnonymous]
        public ApiResult Login(LoginRequest request)
        {
            return authService.Login(request.Account, request.Password);
        }

        /// <summary>刷新Token</summary>
        /// <returns></returns>
        [HttpGet("refreshtoken")]
        public ApiResult RefreshAuthentication()
        {
            return authService.RefreshAuthentication();
        }

        /// <summary>登录效期心跳</summary>
        /// <returns></returns>
        [HttpGet("health")]
        public ApiResult Health()
        {
            return authService.Health();
        }

        /// <summary>用户信息</summary>
        /// <returns></returns>
        [HttpGet("userinfo")]
        public ApiResult UserInfo()
        {
            return authService.UserInfo();
        }

        /// <summary>返回用户菜单和权限</summary>
        /// <returns></returns>
        [HttpGet("usermenus")]
        public ApiResult UserMenus()
        {
            return authService.UserMenus();
        }

        /// <summary>测试Api</summary>
        /// <returns></returns>
        [HttpGet("test"), AllowAnonymous]
        public ApiResult TestTs()
        {
            return sysMainApplication.TcTest();
        }

        public class LoginRequest
        {
            public string Account { get; set; }
            public string Password { get; set; }
        }
    }
}