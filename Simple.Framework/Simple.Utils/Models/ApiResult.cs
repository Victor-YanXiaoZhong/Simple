using Newtonsoft.Json;

namespace Simple.Utils.Models
{
    /// <summary>API调用结果基类</summary>
    public abstract class ApiResultBase
    {
        private int code = 500;

        /// <summary>是否为成功</summary>
        protected bool isSuccess;

        /// <summary>应答码，默认500（约定：200-成功、500-失败或出错、其它-根据需要约定）</summary>
        [JsonProperty("code")]
        public virtual int Code
        {
            get => code;
            set
            {
                code = value;
                isSuccess = value == 200;
            }
        }

        /// <summary>应答消息</summary>
        [JsonProperty("message")]
        public virtual string Message { get; set; } = string.Empty;

        /// <summary>是否为成功</summary>
        /// <returns>bool 值</returns>
        [JsonProperty("success")]
        public virtual bool IsSuccess
        { get => isSuccess; set { isSuccess = value; } }
    }

    /// <summary>API调用结果</summary>
    public class ApiResult : ApiResultBase
    {
        /// <summary>参数有误</summary>
        public static readonly ApiResult ParamError = new ApiResult() { Code = 500, Message = "参数有误" };

        /// <summary>操作成功</summary>
        public static readonly ApiResult OperationSuccessful = new ApiResult() { Code = 200, Message = "操作成功" };

        /// <summary>操作失败</summary>
        public static readonly ApiResult OperationFailed = new ApiResult() { Code = 500, Message = "操作失败" };

        public static ApiResult Default
        { get { return new ApiResult(); } }

        /// <summary>应答数据</summary>
        [JsonProperty("data")]
        public Object Data { get; set; }

        /// <summary>生成失败时的应答实例</summary>
        /// <param name="message">应答消息</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiResult Fail(string message)
        {
            return new ApiResult { Code = 500, Message = message };
        }

        /// <summary>生成成功时的应答实例</summary>
        /// <returns>ApiResult 实例</returns>
        public static ApiResult Success()
        {
            return new ApiResult { Code = 200, Message = "success" };
        }

        /// <summary>生成成功时的应答实例</summary>
        /// <param name="data">应答数据</param>
        /// <param name="message">应答消息</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiResult Success(Object data, string message = "success")
        {
            return new ApiResult { Code = 200, Data = data, Message = message };
        }

        /// <summary>生成应答实例</summary>
        /// <param name="flag">为true则Code等于200，否则Code等于500</param>
        /// <param name="trueMessage">flag为true时返回的消息</param>
        /// <param name="falseMessage">flag为false时返回的消息</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiResult Return(bool flag, string trueMessage, string falseMessage)
        {
            return new ApiResult
            {
                Code = flag ? 200 : 500,
                Data = null,
                Message = flag ? trueMessage : falseMessage
            };
        }

        /// <summary>操作结果</summary>
        /// <param name="isSuccess">是否为操作成功</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiResult Operation(bool isSuccess)
        {
            return isSuccess ? OperationSuccessful : OperationFailed;
        }

        /// <summary>设置成功时的应答</summary>
        /// <param name="message">应答消息</param>
        /// <param name="data">应答数据</param>
        public void SetSuccess(string message, Object data = null)
        {
            Code = 200;
            Message = message;
            Data = data;
        }

        /// <summary>设置出错或失败时的应答</summary>
        /// <param name="message">应答消息</param>
        public void SetFail(string message)
        {
            Code = 500;
            Message = message;
        }

        /// <summary>设置应答</summary>
        /// <param name="isSuccess">是否为成功</param>
        /// <param name="trueMessage">成功时的应答消息</param>
        /// <param name="falseMessage">失败或出错时的应答消息</param>
        public void Set(bool isSuccess, string trueMessage = "操作成功", string falseMessage = "操作失败")
        {
            Code = isSuccess ? 200 : 500;
            Data = null;
            Message = isSuccess ? trueMessage : falseMessage;
        }
    }

    /// <summary>API分页调用结果</summary>
    public class ApiPageResult : ApiResultBase
    {
        /// <summary>应答数据</summary>
        [JsonProperty("data")]
        public Object Data { get; set; }

        /// <summary>条数</summary>
        [JsonProperty("total")]
        public Int32 Total { get; set; }

        /// <summary>生成成功时的应答实例</summary>
        /// <param name="data">应答数据</param>
        /// <param name="total"></param>
        /// <param name="message">应答消息</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiPageResult Success(Object data, int total, string message = "success")
        {
            return new ApiPageResult { Code = 200, Data = data, Total = total, Message = message };
        }

        /// <summary>生成失败时的应答实例</summary>
        /// <param name="message">应答消息</param>
        /// <returns>ApiResult 实例</returns>
        public static ApiPageResult Fail(string message = "fail")
        {
            return new ApiPageResult { Code = 500, Data = new int[0], Total = 0, Message = message };
        }

        /// <summary>设置成功时的应答</summary>
        /// <param name="message">应答消息</param>
        /// <param name="data">应答数据</param>
        /// <param name="total"></param>
        public void SetSuccess(string message, Object data = null, Int32 total = 0)
        {
            Code = 200;
            Message = message;
            Data = data;
            Total = total;
        }
    }

    /// <summary>API调用结果（泛型）</summary>
    public class ApiResult<T> : ApiResultBase
    {
        /// <summary>参数有误</summary>
        public static readonly ApiResult<T> ParamError =
            new ApiResult<T>() { Code = 500, Message = "参数有误" };

        /// <summary>应答数据，非null</summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>生成成功时的应答实例</summary>
        /// <param name="message">应答消息</param>
        /// <param name="data">应答数据</param>
        /// <returns>ApiResult&lt;T&gt; 实例</returns>
        public static ApiResult<T> Success(T data, string message = "success")
        {
            return new ApiResult<T> { Code = 200, Data = data, Message = message };
        }

        /// <summary>生成失败时的应答实例</summary>
        /// <param name="message"></param>
        /// <returns>ApiResult&lt;T&gt; 实例</returns>
        public static ApiResult<T> Fail(string message = "fail")
        {
            return new ApiResult<T> { Code = 500, Message = message };
        }
    }

    /// <summary>API调用结果（泛型集合）</summary>
    public class ApiListResult<T> : ApiResultBase
    {
        private IList<T> data;

        /// <summary>参数有误</summary>
        public static readonly ApiListResult<T> ParamError =
            new ApiListResult<T>() { Code = 500, Message = "参数有误" };

        [JsonProperty("total")]
        public int Total = 0;

        /// <summary>构造函数</summary>
        public ApiListResult()
        { Code = 200; }

        /// <summary>构造函数</summary>
        /// <param name="code">应答码（约定：200-成功、500-失败或出错、其它-按需约定）</param>
        public ApiListResult(int code)
        { Code = code; }

        /// <summary>应答数据，非null</summary>
        [JsonProperty("data")]
        public virtual IList<T> Data
        {
            get
            {
                if (data == null) { data = new List<T>(); }
                return data;
            }
            set { data = value; }
        }

        /// <summary>生成成功时的应答实例</summary>
        /// <param name="message">应答消息</param>
        /// <param name="data">应答数据</param>
        /// <returns>ApiListResult&lt;T&gt; 实例</returns>
        public static ApiListResult<T> Success(IList<T> data, int total = 0, string message = "success")
        {
            return new ApiListResult<T> { Code = 200, Data = data, Total = total == 0 ? data.Count : total, Message = message };
        }

        /// <summary>生成失败时的应答实例</summary>
        /// <param name="message"></param>
        /// <returns>ApiListResult&lt;T&gt; 实例</returns>
        public static ApiListResult<T> Fail(string message = "fail")
        {
            return new ApiListResult<T> { Code = 500, Message = message };
        }
    }

    /// <summary>分页实体</summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class PagedModel<T> : ApiListResult<T>
    {
        /// <summary>条数</summary>
        [JsonProperty("total")]
        public new Int32 Total { get; set; }

        /// <summary>生成成功时的应答实例</summary>
        /// <param name="Total">数据总数</param>
        /// <param name="message">应答消息</param>
        /// <param name="data">应答数据</param>
        /// <returns>ApiListResult&lt;T&gt; 实例</returns>
        public static PagedModel<T> PageResult(Int32 Total, IList<T> data, string message = "success")
        {
            return new PagedModel<T> { Total = Total, Code = 200, Data = data, Message = message };
        }
    }
}