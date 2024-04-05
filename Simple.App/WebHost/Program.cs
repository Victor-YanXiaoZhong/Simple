using Microsoft.OpenApi.Models;
using Simple.WebHost;

using Swashbuckle.AspNetCore.Filters;

var start = DateTime.Now;
ConsoleHelper.Debug($"开始构建主机");
var builder = WebApplication.CreateBuilder(args);
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

#region 添加服务到容器

ConfigHelper.Init(builder.Configuration);

builder.Services.AddControllerConfig();

//配置Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "API接口", Version = "v1" });
    foreach (var item in HostServiceExtension.XmlCommentsFilePath)
    {
        option.IncludeXmlComments(item, true);
    }
    //option.OperationFilter<SwaggerAuthOperatFilter>();
    //给api添加token令牌证书
    option.OperationFilter<SecurityRequirementsOperationFilter>();
    //option.DocumentFilter<HiddenApiFilter>(); // 在接口类、方法标记属性 [HiddenApi]，可以阻止【Swagger文档】生成
    option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
        Name = ConfigHelper.GetValue("TokenHeadKey"),//jwt默认的参数名称
        In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
        Type = SecuritySchemeType.ApiKey
    });
});
builder.Services.AddRedisCacheAndSession();
builder.Services.AddJwtAuth();
builder.Services.AddCustomerCors();
HostServiceExtension.BuildHostService(builder.Services, builder.Configuration);
ConsoleHelper.Debug($"主机服务配置完成，开始配置管道");

#endregion 添加服务到容器

//配置启动的管道服务
var app = builder.Build();
app.UseSession();
HostServiceExtension.BuildHostApp(app);
app.AddJobScheduler();
//配置Swagger ShowSwagger = true 时显示swagger
if (ConfigHelper.GetValue<bool>("ShowSwagger"))
{
    ConsoleHelper.Waring($"Waring：主机开启了Swagger，有暴露API风险");
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCustomerCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ConsoleHelper.Debug($"配置主机管道完成，准备启动");
app.Run();