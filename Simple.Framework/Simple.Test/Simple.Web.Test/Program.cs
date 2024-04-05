using Simple.Utils.Helper;
using Simple.Web.Test.Service;
using Simple.Wechat;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.TenpayV3;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigHelper.Init(builder.Configuration);
builder.Services.AddControllers();
var wechatApiClientOptions = ConfigHelper.GetSection("WechatPay").Get<WechatApiClientOptions>();
builder.Services.AddSingleton(wechatApiClientOptions);
builder.Services.AddHttpClient();
builder.Services.AddSingleton<WechatApiClient>(provider =>
{
    return new WechatApiClient(wechatApiClientOptions);
});
builder.Services.AddSingleton<WechatApi>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ITestService, TestService>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();