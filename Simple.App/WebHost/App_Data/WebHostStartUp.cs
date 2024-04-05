using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Simple.AspNetCore.Helper;
using Simple.AspNetCore.Models;
using Simple.Job;
using System.Text;

namespace Simple.WebHost
{
    /// <summary>主机通用配置</summary>
    public static class WebHostStartUp
    {
        private static async void Jobmanager_JobEnd(FluentScheduler.JobEndInfo obj)
        {
            if (obj.Name.IsNullOrEmpty()) return;
            var jobInfo = new AdminApplication.Entitys.SysJobLog
            {
                Name = obj.Name,
                StartTime = obj.StartTime,
                NextRun = obj.NextRun,
                Duration = $"{obj.Duration.Hours}时{obj.Duration.Minutes}分{obj.Duration.Seconds}秒"
            };
            await AppDomainEventDispatcher.PublishEvent(new AppDomainEvent<AdminApplication.Entitys.SysJobLog>(jobInfo));
        }

        /// <summary>添加JWT格式token</summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtAuth(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;

                var jwtSetting = ConfigHelper.GetValue<JwtSetting>("Jwt");
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.SecurityKey)),
                    ValidIssuer = jwtSetting.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                };
                option.Events = new JwtBearerEvents
                {
                    OnMessageReceived = e =>
                    {
                        if (string.IsNullOrEmpty(e.Token)) return Task.CompletedTask;
                        var user = JWTHelper.GetPayload(e.Token);
                        var cache = e.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
                        var cacheToken = cache.GetString("jwt:" + user.UserAccount);
                        LogHelper.Debug("收到请求的jwttoken：" + e.Token);
                        if (string.IsNullOrEmpty(cacheToken))
                        {
                            return Task.CompletedTask;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        /// <summary>添加到管道尾部，以便能注入到全部的服务</summary>
        /// <param name="app"></param>
        public static void AddJobScheduler(this IApplicationBuilder app)
        {
            try
            {
                var jobmanager = new JobSchedule();
                jobmanager.JobEnd += Jobmanager_JobEnd;
                jobmanager.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}