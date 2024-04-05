using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Simple.AdminApplication;
using Simple.AdminApplication.Application;
using Simple.AdminApplication.Entitys;
using Simple.AspNetCore.Controllers;
using Simple.EntityFrameworkCore;

namespace Simple.AdminController.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("api/[controller]"), PermissionGroup("用户Curd")]
    public class UserController : AppCurdController<SysUser>
    {
        public UserController(AdminDbContext db) : base(db)
        {
        }
    }
}
