using Microsoft.AspNetCore.Mvc;
using Simple.AdminApplication;
using Simple.AdminApplication.Application;
using Simple.AdminApplication.Entitys;
using Simple.AspNetCore.Controllers;

namespace Simple.AdminController.Controllers
{
    /// <summary>角色控制器</summary>
    [Route("api/[controller]"), PermissionGroup("角色Curd")]
    public class RoleController : AppCurdController<SysRole>
    {
        private SysMainApplication mainApplication;

        public RoleController(AdminDbContext db, SysMainApplication mainApplication) : base(db)
        {
            this.mainApplication = mainApplication;
        }

        /// <summary>角色授权菜单 有则删除，无则添加</summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpPost("grantmenu/{roleId:int}/{menuId:INT}")]
        public ApiResult GrantRoleMenu(int roleId, int menuId)
        {
            return ApiResult.Operation(mainApplication.GrantRoleMenu(roleId, menuId));
        }

        /// <summary>角色授权权限 addIds 添加的集合 delIds 删除的集合</summary>
        /// <param name="roleId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost("grantfunction/{roleId:int}")]
        public ApiResult GrantRoleFunction(int roleId, [FromBody] ParameterModel mode)
        {
            var addIdsReq = mode["addIds"].ToString();
            var delIdsReq = mode["delIds"].ToString();
            return ApiResult.Operation(mainApplication.GrantRoleMenuFuntions(roleId, addIdsReq.SplitToInt(), delIdsReq.SplitToInt()));
        }
    }
}