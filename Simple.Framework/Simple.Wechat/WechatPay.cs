using Simple.Utils;
using SKIT.FlurlHttpClient.Wechat.TenpayV3;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Events;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Models;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Settings;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Wechat
{
    public class WechatPay3 : IDisposable
    {
        private Timer stateTimer;

        /// <summary>直接实例化使用</summary>
        /// <param name="clientOptions"></param>
        public WechatPay3(WechatTenpayClientOptions clientOptions)
        {
            WechatTenpayOptions = clientOptions;
            TenpayClient = new WechatTenpayClient(WechatTenpayOptions);
            //立即执行，每天执行一次刷新事件
            var time = new Timer(new TimerCallback(RefreshCertificates), null, 0, 24 * 60 * 60 * 1000);
        }

        /// <summary>DI注入使用，先注入 WechatTenpayClientOptions，再注入 WechatTenpayClient 最后注入此实现</summary>
        /// <param name="tenpayClient"></param>
        public WechatPay3(WechatTenpayClientOptions clientOptions, WechatTenpayClient tenpayClient)
        {
            WechatTenpayOptions = clientOptions;
            TenpayClient = tenpayClient;
            //立即执行，每天执行一次刷新事件
            var time = new Timer(new TimerCallback(RefreshCertificates), null, 0, 24 * 60 * 60 * 1000);
        }

        public WechatTenpayClientOptions WechatTenpayOptions { get; }

        public WechatTenpayClient TenpayClient { get; }

        private async void RefreshCertificates(object state)
        {
            var request = new QueryCertificatesRequest() { AlgorithmType = "ALGORITHM_TYPE" };
            var response = await TenpayClient.ExecuteQueryCertificatesAsync(request);
            var certResponse = CheckResponse(response);
            foreach (var certificate in certResponse.CertificateList)
            {
                TenpayClient.PlatformCertificateManager.AddEntry(CertificateEntry.Parse("ALGORITHM_TYPE", certificate));
            }
        }

        private T CheckResponse<T>(T response) where T : WechatTenpayResponse
        {
            if (response == null || !response.IsSuccessful())
            {
                throw new CustomException($"调用微信支付获取返回值{response}时异常,ErrCode<{response?.ErrorCode}>,ErrorMessage<{response?.ErrorMessage}>", "调用微信支付接口出现异常");
            }
            return response;
        }

        /// <summary>微信支付下单接口</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreatePayTransactionJsapiResponse> CreatePayTransactionJsapiRequestAsync(CreatePayTransactionJsapiRequest request)
        {
            var response = await TenpayClient.ExecuteCreatePayTransactionJsapiAsync(request);
            return CheckResponse(response);
        }

        /// <summary>微信回调通知</summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="content"></param>
        /// <param name="signature"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public TransactionResource Pay3NotifyEvent(string timestamp, string nonce, string content, string signature, string serialNumber)
        {
            var valid = TenpayClient.VerifyEventSignature(timestamp, nonce, content, signature, serialNumber);
            if (valid)
            {
                throw new CustomException($"收到微信回调消息，但是验签失败", "微信回调异常");
            }
            var callbackModel = TenpayClient.DeserializeEvent(content);
            var eventType = callbackModel.EventType?.ToUpper();
            switch (eventType)
            {
                case "TRANSACTION.SUCCESS":
                    {
                        var callbackResource = TenpayClient.DecryptEventResource<TransactionResource>(callbackModel);
                        return callbackResource;
                    }

                default:
                    return null;
            }
        }

        /// <summary>发起退款</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateRefundDomesticRefundResponse> PayRefundAsync(CreateRefundDomesticRefundRequest request)
        {
            var response = await TenpayClient.ExecuteCreateRefundDomesticRefundAsync(request);
            return CheckResponse(response);
        }

        public void Dispose()
        {
            stateTimer?.Dispose();
            stateTimer = null;
        }
    }
}