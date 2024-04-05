using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simple.AspNetCore
{
    /// <summary>web框架模块标识，会自动注册模块</summary>
    public interface ISimpleModule
    {
        /// <summary>配置服务</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration);

        /// <summary>配置管道</summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        void ConfigApp(IApplicationBuilder app, IConfiguration configuration);

        /// <summary>模块第一次初始化，初始化数据库，数据种子之类的事情</summary>
        void AppFirstInit();
    }

    /// <summary>web框架模块标识，会自动注册模块</summary>
    public abstract class SimpleModule : ISimpleModule
    {
        /// <summary>配置管道</summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public virtual void ConfigApp(IApplicationBuilder app, IConfiguration configuration)
        {
        }

        /// <summary>配置服务</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        public virtual void ConfigServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
        }

        /// <summary>模块第一次初始化，初始化数据库，数据种子之类的事情</summary>
        public virtual void AppFirstInit()
        {
        }
    }
}