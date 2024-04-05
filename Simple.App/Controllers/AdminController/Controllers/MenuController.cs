using Microsoft.AspNetCore.Mvc;
using Simple.AdminApplication;
using Simple.AdminApplication.Application;
using Simple.AdminApplication.Entitys;
using Simple.AspNetCore.Controllers;

namespace Simple.AdminController.Controllers
{
    /// <summary>菜单控制器</summary>
    [Route("api/[controller]"), PermissionGroup("菜单Curd")]
    public class MenuController : AppCurdController<SysMenu>
    {
        private SysMainApplication mainApplication;

        public MenuController(AdminDbContext db, SysMainApplication mainApplication) : base(db)
        {
            this.mainApplication = mainApplication;
        }

        /// <summary>获取授权树</summary>
        /// <returns></returns>
        [HttpGet("menutree"), Permission("menutree", "权限树")]
        public ApiResult GetMenuTree()
        {
            return ApiResult.Success(mainApplication.GetMenuTrees(LoginUser));
        }
    }
}