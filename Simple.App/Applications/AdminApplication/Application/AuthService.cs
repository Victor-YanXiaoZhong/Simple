using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Simple.AdminApplication.Entitys;
using Simple.AdminApplication.Model;
using Simple.AspNetCore.Helper;
using Simple.RedisClient;
using Simple.Utils.Models.BO;

namespace Simple.AdminApplication.Application
{
    /// <summary>授权鉴权服务</summary>
    [Transient]
    public class AuthService
    {
        private readonly AdminDbContext db;
        private readonly IDistributedCache cache;

        public AuthService(AdminDbContext dbContext)
        {
            this.db = dbContext;
            cache = HostServiceExtension.ServiceProvider.GetService<IDistributedCache>();
        }

        /// <summary>登录信息</summary>
        private LoginUserBO loginUser
        {
            get
            {
                var auth = JWTHelper.GetPayload(HostServiceExtension.httpContext.User.Claims);
                if (auth.UserID.IsNullOrEmpty())
                    throw new CustomException("登录信息不存在");

                var userId = auth.UserID.ToInt();
                var user = db.SysUser.Where(p => p.Id == userId)
                    .Select(p => new { p.Account, p.Id, p.Email, p.Phone, p.Name, p.SupperAdmin, p.SysRoleId, p.SysOrgnizationId }).FirstOrDefault();
                if (user is null)
                    throw new CustomException("账户信息不存在");
                return new LoginUserBO
                {
                    UserID = userId,
                    UserName = user.Name,
                    UserAccount = user.Account,
                    RoleId = user.SysRoleId?.ToString(),
                    OrgnizationId = user.SysOrgnizationId.ToString(),
                    SupperAdmin = user.SupperAdmin
                };
            }
        }

        private void SetUserFunctions(string key, List<SysFunction> functions)
        {
            var permissions = functions.Select(x => x.FunSign).ToArray();
            RedisHelper.StringSet(key, permissions);
        }

        /// <summary>登录</summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ApiResult Login(string account, string password)
        {
            if (account.IsNullOrEmpty() || password.IsNullOrEmpty())
                return ApiResult.Fail("账号或密码不能为空");

            var user = db.SysUser.FirstOrDefault(p => p.Account == account);
            if (user == null)
                return ApiResult.Fail("账号对应的信息不存在");

            if (user.Password != password)
                return ApiResult.Fail("用户密码错误");

            if (user.Deleted)
                return ApiResult.Fail("用户已被禁用");

            var userOrg = db.SysOrgnization.FirstOrDefault(x => x.Id == user.SysOrgnizationId);

            var token = JWTHelper.CreateToken(user.Account, user.Name, user.SysRoleId?.ToString(), user.Id.ToString(), user.SysOrgnizationId.ToString(), user.SupperAdmin, userOrg == null ? false : userOrg.IsAdmin);
            cache.SetString(account, token);

            UserPermissions(user);

            return ApiResult.Success(new
            {
                user.Name,
                user.Account,
                user.Id,
                user.Avatar,
                user.Phone,
                user.SupperAdmin,
                user.SysOrgnizationId,
                AdminOrg = userOrg == null ? false : userOrg.IsAdmin,
                Token = token,
                LogTime = DateTime.Now
            });
        }

        /// <summary>刷新token</summary>
        /// <returns></returns>
        public ApiResult RefreshAuthentication()
        {
            var oldToken = HostServiceExtension.httpContext.Request.Headers[ConfigHelper.GetValue("TokenHeadKey")];
            if (string.IsNullOrEmpty(oldToken))
            {
                return ApiResult.Success("无法获取请求的Token");
            }

            var user = JWTHelper.GetPayload(HostServiceExtension.httpContext.User.Claims);
            var newToken = JWTHelper.RefreshToken(oldToken);
            cache.SetString(user.UserAccount, newToken);
            return ApiResult.Success(new { authrization = newToken });
        }

        /// <summary>心跳检查 并返回登录后的信息</summary>
        /// <returns></returns>
        public ApiResult Health()
        {
            var user = JWTHelper.GetPayload(HostServiceExtension.httpContext.User.Claims);
            return ApiResult.Success(new { user });
        }

        public ApiResult UserInfo()
        {
            var auth = JWTHelper.GetPayload(HostServiceExtension.httpContext.User.Claims);
            if (auth.UserID.IsNullOrEmpty())
                return ApiResult.Fail("登录信息不存在");
            return ApiResult.Success(auth);
        }

        public ApiResult UserMenus()
        {
            if (loginUser.SupperAdmin)
            {
                var menus = db.SysMenu.Include(x => x.SysMenuFunctions).ToList();
                var functions = db.SysFunction.ToList();
                SetUserFunctions("UserPermission:" + loginUser.UserAccount, functions);
                return ApiResult.Success(new { menus, functons = functions, menuTree = MenuTree.GetMenuTrees(menus, 0) });
            }
            var roleId = loginUser.RoleId.ToInt();
            var menuData = db.SysRoleMenu.Where(p => p.SysRoleId == roleId)
                .Include(x => x.SysMenu)
                .Where(x => !x.SysMenu.Deleted)
                .Select(x => x.SysMenu).ToList();
            var functionData = db.SysRoleFunction.Where(p => p.SysRoleId == roleId)
                .Include(x => x.SysFunction)
                .Where(x => !x.SysFunction.Deleted)
                .Select(x => x.SysFunction).ToList();
            SetUserFunctions("UserPermission:" + loginUser.UserAccount, functionData);
            return ApiResult.Success(new { menus = menuData, functons = functionData, menuTree = MenuTree.GetMenuTrees(menuData, 0) });
        }

        public List<SysFunction> UserPermissions(SysUser user)
        {
            if (user.SupperAdmin)
            {
                var functions = db.SysFunction.ToList();
                SetUserFunctions("UserPermission:" + user.Account, functions);
                return functions;
            }
            var roleId = user.SysRoleId.Value;
            var functionData = db.SysRoleFunction.Where(p => p.SysRoleId == roleId)
                .Include(x => x.SysFunction)
                .Where(x => !x.SysFunction.Deleted)
                .Select(x => x.SysFunction).ToList();
            SetUserFunctions("UserPermission:" + user.Account, functionData);
            return functionData;
        }
    }
}