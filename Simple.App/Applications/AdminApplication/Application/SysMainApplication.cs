using Microsoft.EntityFrameworkCore;
using Simple.AdminApplication.Entitys;
using Simple.AdminApplication.Model;
using Simple.EntityFrameworkCore;

using Simple.Utils.Models.BO;

namespace Simple.AdminApplication.Application
{
    [Transient]
    public class SysMainApplication
    {
        private AdminDbContext adminDb;

        public SysMainApplication(AdminDbContext adminDb)
        {
            this.adminDb = adminDb;
            roleService = new BaseCurdService<SysRole>(this.adminDb);
            functionService = new BaseCurdService<SysFunction>(this.adminDb);
            menuService = new BaseCurdService<SysMenu>(this.adminDb);
            roleMenuService = new BaseCurdService<SysRoleMenu>(this.adminDb);
            roleFunctionService = new BaseCurdService<SysRoleFunction>(this.adminDb);
            SysDicService = new BaseCurdService<SysDicType>(this.adminDb);
        }

        /// <summary>角色服务</summary>
        public ICurdService<SysRole> roleService { get; }

        /// <summary>功能服务</summary>
        public ICurdService<SysFunction> functionService { get; }

        /// <summary>菜单服务</summary>
        public ICurdService<SysMenu> menuService { get; }

        /// <summary>角色菜单服务</summary>
        public ICurdService<SysRoleMenu> roleMenuService { get; }

        /// <summary>角色权限服务</summary>
        public ICurdService<SysRoleFunction> roleFunctionService { get; }

        /// <summary>字典服务</summary>
        public ICurdService<SysDicType> SysDicService { get; }

        public ApiResult TcTest()
        {
            return ApiResult.Success(
                adminDb.RunWithTran(trans =>
                {
                    functionService.Add(new SysFunction { FunGroup = "系统管理", FunSign = "ADMIN,TEST", Name = "haha " });
                    functionService.Edit(2, new ParameterModel
                    {
                        { "Name",$"{DateTime.Now}"},{ "EnAbled",false}
                    }
                   );
                })
            );
        }

        /// <summary>角色授权菜单</summary>
        /// <param name="isAdd"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool GrantRoleMenu(int roleId, int menuId)
        {
            var menu = menuService.Get(menuId);
            if (menu is null) throw new CustomException("没有找到菜单信息");

            var role = roleService.Get(roleId);
            if (role is null) throw new CustomException("没有找到角色信息");

            var roleMenu = roleMenuService.Get(x => x.SysRoleId == roleId && x.SysMenuId == menuId);
            if (roleMenu is null)
            {
                return roleMenuService.Add(new SysRoleMenu
                {
                    SysRoleId = roleId,
                    SysMenuId = menuId
                });
            }
            else
            {
                return roleMenuService.Delete(roleMenu);
            }
        }

        /// <summary>角色授权菜单权限</summary>
        /// <param name="addIds"></param>
        /// <param name="delIds"></param>
        /// <returns></returns>
        public bool GrantRoleMenuFuntions(int roleId, int[] addIds, int[] delIds)
        {
            var role = roleService.Get(roleId);
            if (role is null) throw new CustomException("没有找到角色信息");

            bool issuccess = true;
            foreach (var functionId in addIds)
            {
                if (!issuccess)
                    throw new FatalException("添加角色权限异常");
                issuccess = roleFunctionService.Add(new SysRoleFunction
                {
                    SysRoleId = roleId,
                    SysFunctionId = functionId
                });
            }

            foreach (var functionId in delIds)
            {
                if (!issuccess)
                    throw new FatalException("添加角色权限异常");
                var roleFuction = roleFunctionService.Get(x => x.SysRoleId == roleId && x.SysFunctionId == functionId);

                if (roleFuction != null)
                {
                    issuccess = roleFunctionService.Delete(roleFuction.Id);
                }
            }

            return issuccess;
        }

        /// <summary>获取授权用的菜单树 包含菜单和权限</summary>
        /// <returns></returns>
        public List<MenuTree> GetMenuTrees(LoginUserBO loginUser)
        {
            if (loginUser.SupperAdmin)
            {
                var menus = adminDb.SysMenu.Include(x => x.SysMenuFunctions).ToList();
                return MenuTree.GetMenuTrees(menus, 0);
            }
            var roleId = loginUser.RoleId.ToInt();
            var menuData = adminDb.SysRoleMenu.Where(p => p.SysRoleId == roleId)
                .Include(x => x.SysMenu).ThenInclude(x => x.SysMenuFunctions)
                .Where(x => !x.SysMenu.Deleted)
                .Select(x => x.SysMenu).ToList();
            return MenuTree.GetMenuTrees(menuData, 0);
        }
    }
}