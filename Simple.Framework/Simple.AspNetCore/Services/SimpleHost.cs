using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple.Utils;
using Simple.Utils.Helper;

namespace Simple.AspNetCore.Services
{
    /// <summary>一键启动Web主机 包含默认配置</summary>
    public class SimpleHost
    {
        /// <summary>一键启动Web简单主机 已配置 config、 ControllerConfig、RedisCacheAndSession、跨域、静态服务器、配置启用授权鉴权策略</summary>
        /// <param name="args"></param>
        /// <param name="configBuilder">添加额外的管道</param>
        /// <param name="configApp">配置额外的管道</param>
        public static void SimpleRunWeb(string[] args,
            Action<WebApplicationBuilder>? configBuilder = null,
            Action<WebApplication, IConfiguration>? configApp = null)
        {
            ConsoleHelper.Debug($"开始构建WebHost主机");
            var builder = WebApplication.CreateBuilder(args);
            if (builder.Configuration["ConfigEnv"] is null)
            {
                ConsoleHelper.Err("环境配置文件 ConfigEnv 配置不存在！");
                return;
            }
            if (builder.Configuration["ConfigEnv"].ToString().ToUpper() == "DEV")
            {
                ConsoleHelper.Debug($"检测到当前是开发环境，使用配置文件：Config/app_dev.json");
                builder.Configuration.AddJsonFile("Config/app_dev.json", true, true);
            }
            else
            {
                ConsoleHelper.Debug($"检测到当前为正式环境，使用配置文件：Config/app.json");
                builder.Configuration.AddJsonFile("Config/app.json", true, true);
            }

            ConfigHelper.Init(builder.Configuration);
            builder.Services.AddControllerConfig();

            //配置Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRedisCacheAndSession();
            builder.Services.AddCustomerCors();
            ///添加自定义管道
            configBuilder?.Invoke(builder);
            HostServiceExtension.BuildHostService(builder.Services, builder.Configuration);
            ConsoleHelper.Debug($"主机服务配置完成，开始配置管道");

            //配置启动的管道服务
            var app = builder.Build();
            app.UseSession();
            HostServiceExtension.BuildHostApp(app);
            configApp?.Invoke(app, builder.Configuration);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCustomerCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            ConsoleHelper.Debug($"配置主机管道完成，准备启动");
            app.Run();
        }

        /// <summary>一键创建控制台主机</summary>
        /// <param name="args"></param>
        /// <param name="configServices">配置服务</param>
        /// <param name="afterBuilder">主机构建完成后的操作</param>
        public static async Task SimpleRunConsoleAsync(string[] args,
            Action<HostBuilderContext, IServiceCollection>? configServices = null,
            Action? afterBuilder = null)
        {
            ConsoleHelper.Debug($"开始构建Console主机");

            var createHost = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                ConfigHelper.Init(hostContext.Configuration);
                // 注册服务及配置
                if (hostContext.Configuration["ConfigEnv"].ToString().ToUpper() == "DEV")
                {
                    ConsoleHelper.Debug($"检测到当前是开发环境，使用配置文件：Config/app_dev.json");
                    ConfigHelper.Init(new string[] { "appsetting.json", "Config/app_dev.json" }, true, true);
                }
                else
                {
                    ConsoleHelper.Debug($"检测到当前为正式环境，使用配置文件：Config/app.json");
                    ConfigHelper.Init(new string[] { "appsetting.json", "Config/app.json" }, true, true);
                }
                HostServiceExtension.BuildHostService(services, ConfigHelper.configuration);
                configServices?.Invoke(hostContext, services);
                ConsoleHelper.Debug($"主机服务配置完成，Console主机跳过配置管道");
            })
            .UseConsoleLifetime();

            var build = createHost.Build();
            afterBuilder?.Invoke();
            await build.RunAsync();
        }
    }
}