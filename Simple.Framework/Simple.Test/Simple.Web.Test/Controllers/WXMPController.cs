using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.Utils.Models;
using Simple.Wechat;

namespace Simple.Web.Test.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WXMPController : ControllerBase
    {
        private WechatApi wechatApi;

        public WXMPController(WechatApi wechatApi)
        {
            this.wechatApi = wechatApi;
        }

        [HttpGet, HttpPost]
        public string Token([FromQuery] string echostr)
        {
            return echostr;
        }

        [HttpGet]
        public ApiResult GetAccessToken()
        {
            var response = wechatApi.CgibinTokenAccess;
            return ApiResult.Success(response);
        }

        [HttpGet]
        public void Auth()
        {
            Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx1af9f6c671c2d833&redirect_uri=http%3A%2F%2F123.207.45.90%3A9999%2FWXMP%2FGetOpenId&response_type=code&scope=snsapi_base&state=123#wechat_redirect");
        }

        [HttpGet]
        public ApiResult GetOpenId(string code)
        {
            var response = wechatApi.GetOpenIdAsync(code);
            return ApiResult.Success(response);
        }

        [HttpGet]
        public async Task<ApiResult> GetJSSDKParam(string url = "123")
        {
            var response = await wechatApi.GetJssdkParam(url);
            return ApiResult.Success(response);
        }
    }
}