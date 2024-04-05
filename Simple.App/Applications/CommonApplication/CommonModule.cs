using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.CommonApplication.Application;

namespace Simple.CommonApplication
{
    /// <summary>
    /// 共享的一些模块
    /// </summary>
    public class CommonModule : SimpleModule
    {
        public override void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<CommonDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("AdminDb"));
            });
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void AppFirstInit()
        {
            var commdb = HostServiceExtension.ServiceProvider.GetService<CommonDbContext>();
            try
            {
                commdb.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err($"创建CommonDbContext异常 ex:{ex.Message}");
            }

            try
            {
                var databaseCreator = commdb.GetService<IRelationalDatabaseCreator>();
                databaseCreator.CreateTables();
            }
            catch (Exception ex)
            {
                ConsoleHelper.Err($"创建CommonDb表异常 ex:{ex.Message}");
            }
        }
    }
}