using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.AdminApplication.Application;
using Simple.AdminApplication;
using Simple.AdminApplication.Entitys;
using Simple.AspNetCore.Controllers;
using Simple.EntityFrameworkCore;

namespace Simple.AdminController.Controllers
{
    /// <summary>菜单控制器</summary>
    [Route("api/[controller]"), PermissionGroup("字典Curd")]
    public class SysDicController : AppCurdController<SysDicType>
    {
        private ICurdService<SysDicValue> sysDicValueService;

        public SysDicController(AdminDbContext db) : base(db)
        {
            this.sysDicValueService = new BaseCurdService<SysDicValue>(db);
        }
    }
}