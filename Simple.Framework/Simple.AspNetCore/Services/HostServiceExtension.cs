using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Simple.AspNetCore.Filters;
using Simple.Utils;

using Simple.Utils.Attributes;
using Simple.Utils.Helper;
using Simple.Utils.Models.BO;
using StackExchange.Redis;
using System.Reflection;
using System.Runtime.Loader;

namespace Simple.AspNetCore
{
    /// <summary>主机服务扩展 模块加载及配置</summary>
    public static class HostServiceExtension
    {
        //模块列表
        private static readonly List<ISimpleModule> appModules = new List<ISimpleModule>();

        //等待注册的服务列表
        private static readonly List<Type> serviceList = new List<Type>();

        private static IApplicationBuilder app;
        private static IServiceCollection serviceCollection;
        private static IServiceProvider serviceProvider;
        private static IConfiguration configuration;
        private static IHttpContextAccessor httpContextAccessor;
        private static List<Assembly> assemblies = new List<Assembly>();

        //全部控制器的权限列表
        public static List<PermissionBO> permissions = new List<PermissionBO>();

        public static AppProfileDataDb appProfileData;

        static HostServiceExtension()
        {
            appProfileData = AppProfileDataDb.GetInstance();
            GetAppModules();
            GetAppPermission();
        }

        /// <summary>获取配置项</summary>
        public static IConfiguration Configuration
        { get { return configuration; } }

        /// <summary>获取服务提供者</summary>
        /// <returns></returns>
        public static IServiceProvider ServiceProvider
        { get { return serviceProvider; } }

        /// <summary>获取当前访问的上下文 waring 需要注入httpContextAccessor</summary>
        public static HttpContext? httpContext
        { get { return httpContextAccessor.HttpContext; } }

        /// <summary>获取全部注释Xml文档</summary>
        public static List<string> XmlCommentsFilePath
        {
            get
            {
                var basePath = AppContext.BaseDirectory;
                DirectoryInfo d = new DirectoryInfo(basePath);
                FileInfo[] files = d.GetFiles("*.xml");
                var xmls = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
                return xmls;
            }
        }

        /// <summary>注册领域内都使用的管道服务</summary>
        private static void RegistDomainServices()
        {
            //MediatR
            serviceCollection.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(GetAssemblyList().ToArray());
            });
            ConsoleHelper.Debug("已注册 MediatR 服务通道，可以使用领域服务");
        }

        /// <summary>注册运行模块第一次初始化工作</summary>
        private static void RegistModulesFirstInit()
        {
            ConsoleHelper.Debug("检测到配置模块需要注册初始化事件，将开始注册各模块初始化事件 AppFirstInit ");
            foreach (var appmodule in appModules)
            {
                var moduleName = appmodule.GetType().Name;
                ConsoleHelper.Info($"开始注册模块 {moduleName} 初始化事件 AppFirstInit ");
                var hasInit = appProfileData.TempData.Get<bool>(moduleName + "_HasInit");
                if (hasInit)
                {
                    ConsoleHelper.Debug($"模块 {moduleName} 初始化已经操作，跳过");
                    return;
                }
                try
                {
                    appmodule.AppFirstInit();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.Err($"模块 {moduleName} 初始化异常：ex:{ex.Message}");
                }
                appProfileData.TempData.Set(moduleName + "_HasInit", true);
            }
        }

        /// <summary>注册模块服务配置</summary>
        private static void RegistSimpleModulesServices()
        {
            foreach (var appmodule in appModules)
            {
                appmodule.ConfigServices(serviceCollection, Configuration);
            }
            //注册所有需要自动注册的服务
            foreach (var service in serviceList)
            {
                var attr = service.GetCustomAttributes();

                var impInterFace = service.GetInterfaces();

                if (attr.Any(p => p.GetType() == typeof(TransientAttribute)))
                {
                    if (impInterFace.Length == 0)
                    {
                        serviceCollection.AddTransient(service);
                        continue;
                    }
                    foreach (var interfaceImpl in impInterFace)
                    {
                        if (interfaceImpl.IsGenericType)
                        {
                            var constructedInterface = GetGeneric(interfaceImpl, service);
                            serviceCollection.AddTransient(constructedInterface, service);
                        }
                        else
                        {
                            serviceCollection.AddTransient(interfaceImpl, service);
                        }
                    }
                }
                if (attr.Any(p => p.GetType() == typeof(ScopedAttribute)))
                {
                    if (impInterFace.Length == 0)
                    {
                        serviceCollection.AddScoped(service);
                        continue;
                    }

                    foreach (var interfaceImpl in impInterFace)
                    {
                        if (interfaceImpl.IsGenericType)
                        {
                            var constructedInterface = GetGeneric(interfaceImpl, service);
                            serviceCollection.AddScoped(constructedInterface, service);
                        }
                        else
                        {
                            serviceCollection.AddScoped(interfaceImpl, service);
                        }
                    }
                }
                if (attr.Any(p => p.GetType() == typeof(SingletonAttribute)))
                {
                    if (impInterFace.Length == 0)
                    {
                        serviceCollection.AddSingleton(service);
                        continue;
                    }
                    foreach (var interfaceImpl in impInterFace)
                    {
                        if (interfaceImpl.IsGenericType)
                        {
                            var constructedInterface = GetGeneric(interfaceImpl, service);
                            serviceCollection.AddSingleton(constructedInterface, service);
                        }
                        else
                        {
                            serviceCollection.AddSingleton(interfaceImpl, service);
                        }
                    }
                }
            }

            //构建泛型类型实例
            Type GetGeneric(Type interfaceDin, Type interfaceImpl)
            {
                // 获取泛型类型参数
                Type genericTypeArgument = interfaceDin.GetGenericArguments()[0];
                // 构建泛型类型实例
                Type constructedRepositoryType = interfaceImpl.MakeGenericType(genericTypeArgument);
                return constructedRepositoryType;
            }
        }

        /// <summary>注册模块服务配app</summary>
        private static void RegistSimpleModulesApp()
        {
            foreach (var appmodule in appModules)
            {
                ConsoleHelper.Info($"正在配置 Module {appmodule}");
                appmodule.ConfigApp(app, Configuration);
            }
        }

        //获取模块的程序集，构建模块列表，待注入列表
        private static void GetAppModules()
        {
            var assemblies = GetAssemblyList();

            foreach (var item in assemblies)
            {
                var appModule = item.ExportedTypes
                    .Where(w => w.BaseType == typeof(SimpleModule)).FirstOrDefault();
                if (appModule != null)
                {
                    var module = Activator.CreateInstance(appModule, true) as ISimpleModule;
                    appModules.Add(module);
                }

                var service = item.ExportedTypes
                    .Where(t => t.CustomAttributes
                        .Any(attr =>
                                attr.AttributeType == typeof(TransientAttribute)
                                || attr.AttributeType == typeof(ScopedAttribute)
                                || attr.AttributeType == typeof(SingletonAttribute)
                            )
                      );
                serviceList.AddRange(service);
            }
        }

        //获取全部控制器中标识的模块权限集合
        private static void GetAppPermission()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => !type.IsAbstract)
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type)
                    || typeof(Controller).IsAssignableFrom(type)).ToList();

                types.ForEach(type =>
                {
                    var permissionGroup = type.GetCustomAttribute<PermissionGroupAttribute>();
                    foreach (var methodInfo in type.GetMethods())
                    {
                        var permissionAttr = methodInfo.GetCustomAttributes<PermissionAttribute>().FirstOrDefault();

                        if (permissionAttr is not null)
                        {
                            var permission = new PermissionBO
                            {
                                Group = permissionAttr.Group,
                                Name = permissionAttr.Name,
                                Description = permissionAttr.Description,
                                Sign = $"{type.Name}.{methodInfo.Name}".ToUpper().Replace("CONTROLLER", "")
                            };
                            if (permission.Group.IsNullOrEmpty())
                            {
                                if (permissionGroup is null)
                                {
                                    permissionGroup = new PermissionGroupAttribute(type.Name.Replace("Controller", ""));
                                };
                                permission.Group = permissionGroup.Name;
                            }
                            permissions.Add(permission);
                        }
                    }
                });
            }
            RedisHelper.StringSet("permissions", permissions);
        }

        /// <summary>获取所有的 程序集</summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAssemblyList(Func<Assembly, bool> where = null)
        {
            #region 查找手动引用的程序集

            if (assemblies.Count == 0)
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null) return assemblies;
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                assemblies = new List<Assembly> { entryAssembly }.Union(referencedAssemblies).ToList();

                #region 将所有 dll 文件 重新载入 防止有未扫描到的 程序集

                var paths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                    .Where(w => w.EndsWith(".dll") && !w.Contains("Microsoft"))
                    .Select(w => w)
                 ;
                foreach (var item in paths)
                {
                    if (File.Exists(item))
                    {
                        try
                        {
                            Assembly.Load(AssemblyLoadContext.GetAssemblyName(item));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                assemblies = AssemblyLoadContext.Default.Assemblies.Union(assemblies)
                    .Where(x => !x.FullName.Contains("Microsoft") && !x.FullName.Contains("System")).ToList();

                #endregion 将所有 dll 文件 重新载入 防止有未扫描到的 程序集
            }

            #endregion 查找手动引用的程序集

            return @where == null ? assemblies : assemblies.Where(@where);
        }

        /// <summary>注册主机服务，同时注册所有模块的Service，配置于服务最后</summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void BuildHostService(IServiceCollection _serviceCollection, IConfiguration _configuration)
        {
            serviceCollection = _serviceCollection;
            configuration = _configuration;
            RegistSimpleModulesServices();

            RegistDomainServices();

            serviceProvider = serviceCollection.BuildServiceProvider();
            httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        }

        /// <summary>构建主机，同时配置所有模块的服务配置</summary>
        /// <param name="_app"></param>
        public static void BuildHostApp(IApplicationBuilder _app)
        {
            app = _app;
            RegistSimpleModulesApp();
            var needInit = !ConfigHelper.GetValue("AppHasInit", true);
            if (needInit) RegistModulesFirstInit();
        }

        /// <summary>注册服务</summary>
        /// <param name="regist"></param>
        public static void RegistService(Action<IServiceCollection, IConfiguration> regist)
        {
            regist(serviceCollection, configuration);
        }

        /// <summary>创建服务域</summary>
        /// <returns></returns>
        public static IServiceScope CreateScope() => serviceProvider.CreateScope();

        /// <summary>创建服务,参数若已经注入，则自动获取</summary>
        /// <returns></returns>
        public static T CreateInstance<T>() => ActivatorUtilities.CreateInstance<T>(serviceProvider);

        /// <summary>创建服务,参数若已经注入，则自动获取</summary>
        /// <returns></returns>
        public static object CreateInstance(Type type) => ActivatorUtilities.CreateInstance(serviceProvider, type);

        /// <summary>添加控制器配置文件，包括全局异常配置、json序列化配置</summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddControllerConfig(this IServiceCollection services)
        {
            services.AddControllers(option =>
            {
                //关闭不可为空引用类型的属性 例如String
                option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                option.Filters.Add<GlobalExceptionFilter>();
                //option.Filters.Add<AuthenticationFilter>();
                option.ValueProviderFactories.Add(new JQueryQueryStringValueProviderFactory());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //序列化时key为驼峰样式
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//忽略循环引用
            });
            return services;
        }

        /// <summary>添加基于Redis的Session</summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCacheAndSession(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            //将Redis分布式缓存服务添加到服务中
            services.AddStackExchangeRedisCache(options =>
            {
                //Redis实例名
                options.InstanceName = "DistributedCache";
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    ConnectTimeout = 2000,
                    DefaultDatabase = ConfigHelper.GetValue<int>("Redis:Database"),
                    //Password = "xxxxxx",
                    AllowAdmin = true,
                    AbortOnConnectFail = false,//当为true时，当没有可用的服务器时则不会创建一个连接
                };
                options.ConfigurationOptions.EndPoints.Add(ConfigHelper.GetValue("Redis:Hosts"));
            });
            services.AddSession(op =>
            {
                op.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            return services;
        }

        /// <summary>添加自定义跨域</summary>
        /// <param name="services"></param>
        public static void AddCustomerCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    var urls = ConfigHelper.GetValue("CorsUrl");
                    builder.WithOrigins(urls.Split(',')) // 允许部分站点跨域请求
                    .AllowAnyMethod() // 允许所有请求方法
                    .AllowAnyHeader() // 允许所有请求头
                    .AllowCredentials(); // 允许Cookie信息
                });
            });
        }

        /// <summary>配置跨域管道</summary>
        /// <param name="app"></param>
        public static void UseCustomerCors(this IApplicationBuilder app)
        {
            app.UseCors("AllowSpecificOrigin");
        }
    }
}