using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.AdminApplication.Entitys;
using System.Text;

namespace Simple.AdminApplication
{
    public class AdminModule : SimpleModule
    {
        public override void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<AdminDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("AdminDb"));
            });
        }

        public override void AppFirstInit()
        {
            var msg = new StringBuilder();
            var adminDb = HostServiceExtension.ServiceProvider.GetService<AdminDbContext>();
            try
            {
                adminDb.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err("AdminModule 创建数据库异常：" + ex.Message);
            }

            try
            {
                var databaseCreator = adminDb.GetService<IRelationalDatabaseCreator>();
                databaseCreator.CreateTables();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err("AdminModule 创建数据库表异常：" + ex.Message);
            }

            try
            {
                var permissions = new List<SysFunction>();
                //初始化权限功能点数据
                foreach (var permission in HostServiceExtension.permissions)
                {
                    if (adminDb.SysFunction.Any(p => p.Name == permission.Name && p.FunSign == permission.Sign && p.FunGroup == permission.Group)) continue;

                    permissions.Add(new SysFunction
                    {
                        Name = permission.Name,
                        FunGroup = permission.Group,
                        FunSign = permission.Sign
                    });
                }
                if (permissions.Count > 0)
                {
                    adminDb.SysFunction.AddRange(permissions);
                    adminDb.SaveChanges();
                }
                ConsoleHelper.Debug($"AdminModule创建应用功能点完成，新增{permissions.Count}条");

                if (adminDb.SysUser.Count() > 0) return;
                var adminOrg = new SysOrgnization
                {
                    Name = "管理平台",
                    IsAdmin = true,
                    Remark = "这是管理平台的组织"
                };
                adminDb.Add(adminOrg);

                var adminUser = new SysUser
                {
                    Account = "Admin",
                    Name = "超级管理员",
                    Password = "123456",
                    SupperAdmin = true,
                    SysOrgnization = adminOrg,
                };

                adminDb.Add(adminUser);
                adminDb.SaveChanges();

                ConsoleHelper.Debug($"AdminModule添加超级管理员账号和组织完成，账号：Amdin 密码：123456");
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err("AdminModule创建初始化数据异常：" + ex.Message);
            }

            Console.WriteLine(msg);
        }
    }
}