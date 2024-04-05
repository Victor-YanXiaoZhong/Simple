using System;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simple.Utils;
using Simple.Utils.Helper;
using Simple.Wechat.Models;
using SKIT.FlurlHttpClient;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SKIT.FlurlHttpClient.Wechat.TenpayV3;

namespace Simple.Wechat
{
    /// <summary>微信API 单列使用，自动缓存AccessToken和续期</summary>
    public class WechatApi
    {
        private static object lockobj = new object();

        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private WxAccessToken wxAccessToken = new WxAccessToken();

        /// <summary>直接实例化使用</summary>
        /// <param name="options"></param>
        public WechatApi(WechatApiClientOptions options)
        {
            WechatApiOptions = options;
            ApiClient = new WechatApiClient(WechatApiOptions);

            //立即执行，每7000秒执行一次刷新事件
            var time = new Timer(new TimerCallback(RefreshAccessTokenAsync), null, 0, 7000 * 1000);
        }

        /// <summary>用于DI 注入使用 ，先注入下面2个 wechatPayOptions)、WechatTenpayClient&gt;</summary>
        /// <param name="apiClient"></param>
        public WechatApi(WechatApiClientOptions options, WechatApiClient apiClient)
        {
            WechatApiOptions = options;
            ApiClient = apiClient;

            //立即执行，每7000秒执行一次刷新事件
            var time = new Timer(new TimerCallback(RefreshAccessTokenAsync), null, 0, 7000 * 1000);
        }

        /// <summary>微信apitoken</summary>
        public WxAccessToken CgibinTokenAccess
        {
            get
            {
                if (wxAccessToken.IsExpire)
                {
                    RefreshAccessTokenAsync(null);
                    Thread.Sleep(1000);
                }
                return wxAccessToken;
            }
        }

        public WechatApiClientOptions WechatApiOptions { get; }
        public WechatApiClient ApiClient { get; }

        private T CheckResponse<T>(T response) where T : WechatApiResponse
        {
            if (response == null || !response.IsSuccessful())
            {
                LogHelper.Error($"调用微信获取返回值{response}时异常,ErrCode<{response?.ErrorCode}>,ErrorMessage<{response?.ErrorMessage}>");
                throw new CustomException($"调用微信获取返回值{response}时异常,ErrCode<{response?.ErrorCode}>,ErrorMessage<{response?.ErrorMessage}>", "调用微信接口出现异常");
            }
            return response;
        }

        /// <summary>定时获取微信AccessToken</summary>
        private async void RefreshAccessTokenAsync(object state)
        {
            if (wxAccessToken.IsExpire)
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    if (wxAccessToken.IsExpire)
                    {
                        Console.WriteLine($"{DateTime.Now}:执行了WxAccessToken刷新");
                        var request = new CgibinTokenRequest()
                        {
                            GrantType = "client_credential",
                        };

                        var response = await ApiClient.ExecuteCgibinTokenAsync(request);
                        if (response.IsSuccessful())
                        {
                            wxAccessToken.AccessToken = response.AccessToken;
                            wxAccessToken.ExpiresIn = response.ExpiresIn;
                            wxAccessToken.TokenExpireTime = DateTime.Now.AddSeconds(response.ExpiresIn);
                        }
                    }
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
        }

        /// <summary>获取openid</summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<SnsOAuth2AccessTokenResponse> GetOpenIdAsync(string code)
        {
            var tokenRequest = new SnsOAuth2AccessTokenRequest()
            {
                Code = code
            };

            var response = await ApiClient.ExecuteSnsOAuth2AccessTokenAsync(tokenRequest);
            return CheckResponse(response);
        }

        /// <summary>获取userinfo信息</summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<CgibinUserInfoResponse> GetUserInfoAsync(string accessToken, string openId)
        {
            var userInfoRequest = new CgibinUserInfoRequest()
            {
                AccessToken = accessToken,
                OpenId = openId
            };

            var response = await ApiClient.ExecuteCgibinUserInfoAsync(userInfoRequest);
            return CheckResponse(response);
        }

        /// <summary>网页中初始化微信 JS-SDK</summary>
        /// <param name="currentUrl"></param>
        /// <returns></returns>
        public async Task<JssdkParamVO> GetJssdkParam(string currentUrl)
        {
            var request = new CgibinTicketGetTicketRequest { AccessToken = CgibinTokenAccess.AccessToken, Type = "jsapi" };
            var ticketResp = CheckResponse(await ApiClient.ExecuteCgibinTicketGetTicketAsync(request));

            string nonceStr = NonceHelp.MakeNonceString(16);
            string timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            string url = currentUrl;
            string rawString = $"jsapi_ticket={ticketResp.Ticket}&noncestr={nonceStr}&timestamp={timestamp}&url={url}";

            using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(rawString));
                string signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return new JssdkParamVO { appId = WechatApiOptions.AppId, timestamp = timestamp, nonceStr = nonceStr, signature = signature };
            }
        }

        /// <summary>删除菜单</summary>
        /// <returns></returns>
        public async Task<CgibinMenuDeleteResponse> DeleteMenu()
        {
            var request = new CgibinMenuDeleteRequest { AccessToken = CgibinTokenAccess.AccessToken };
            var response = await ApiClient.ExecuteCgibinMenuDeleteAsync(request);
            return CheckResponse(response);
        }

        /// <summary>创建菜单</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CgibinMenuCreateResponse> PublishMenu(CgibinMenuCreateRequest request)
        {
            request.AccessToken = CgibinTokenAccess.AccessToken;
            var response = await ApiClient.ExecuteCgibinMenuCreateAsync(request);
            return CheckResponse(response);
        }

        /// <summary>发送微信公众号模板消息</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CgibinMessageTemplateSendResponse> SendTemplate(CgibinMessageTemplateSendRequest request)
        {
            request.AccessToken = CgibinTokenAccess.AccessToken;
            var response = await ApiClient.ExecuteCgibinMessageTemplateSendAsync(request);
            return CheckResponse(response);
        }
    }
}