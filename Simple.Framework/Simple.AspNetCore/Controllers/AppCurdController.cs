using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.EntityFrameworkCore;
using Simple.Utils;
using Simple.Utils.Models;

namespace Simple.AspNetCore.Controllers
{
    /// <summary>快速实现增删改查的控制器基类</summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]"), ApiController, Authorize]
    public abstract class AppCurdController<T> : AppAuthController where T : class, new()
    {
        private readonly ICurdService<T> service;
        private readonly bool isIntKey;

        public AppCurdController(AppDbContext appDb, bool isIntKey = true)
        {
            this.service = new BaseCurdService<T>(appDb);
            this.isIntKey = isIntKey;
        }

        private List<Filter> GetFilters(string where)
        {
            var filters = new List<Filter>();
            if (!where.IsNullOrEmpty())
            {
                filters = JsonHelper.FromJson<List<Filter>>(System.Web.HttpUtility.UrlDecode(where));
            }
            return filters;
        }

        /// <summary>新增前的验证</summary>
        /// <returns></returns>
        protected virtual ApiResult VerifyAdd(T model)
        {
            return ApiResult.Success();
        }

        /// <summary>判断是否为无效主键</summary>
        /// <param name="id">主键（字符串）</param>
        /// <param name="intId">主键（整形）</param>
        /// <returns>Boolean</returns>
        [NonAction]
        protected int IsInvalidId(String id)
        {
            int intId = 0;
            Boolean result;
            if (isIntKey)
            {
                result = !Int32.TryParse(id, out intId) || intId < 0;
            }
            return intId;
        }

        /// <summary>编辑前的验证</summary>
        /// <returns></returns>
        protected virtual ApiResult VerifyEdit(T model)
        {
            return ApiResult.Success();
        }

        /// <summary>新增</summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("add"), Permission("add", "新增")]
        public virtual ApiResult Add(ParameterModel model)
        {
            if (model.Count == 0)
                return ApiResult.Fail("没有获取到对象参数 model ");

            var entity = model.Read<T>();
            var verify = VerifyAdd(entity);
            if (!verify.IsSuccess)
                return verify;
            return ApiResult.Operation(service.Add(entity));
        }

        /// <summary>编辑</summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("edit/{id}"), Permission("edit", "编辑")]
        public virtual ApiResult Edit(string id, ParameterModel model)
        {
            model.Remove("Id");
            model.Remove("CreateTime");

            if (model.Count == 0)
                return ApiResult.Fail("没有获取到对象参数 model ");

            var intId = IsInvalidId(id);

            var entity = service.Get(isIntKey ? intId : id);
            if (entity is null)
                return ApiResult.Fail("数据不存在");
            var verify = VerifyEdit(entity);
            if (!verify.IsSuccess)
                return verify;
            return ApiResult.Operation(service.Edit(isIntKey ? intId : id, model));
        }

        /// <summary>物理删除</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}"), Permission("delete", "删除")]
        public virtual ApiResult Delete(string id)
        {
            var intId = IsInvalidId(id);
            return ApiResult.Operation(service.Delete(isIntKey ? intId : id));
        }

        /// <summary>设置标量值</summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("setflag/{id}"), Permission("setflag", "设置")]
        public virtual ApiResult SetFlag(string id, ParameterModel model)
        {
            if (model.Count == 0)
                return ApiResult.Fail("没有获取到对象参数 model ");
            var intId = IsInvalidId(id);
            return ApiResult.Operation(service.SetFlag(isIntKey ? intId : id, model));
        }

        /// <summary>获取模型</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public virtual ApiResult GetById(string id)
        {
            var intId = IsInvalidId(id);

            var entity = service.Get(isIntKey ? intId : id);
            if (entity is null)
                return ApiResult.Fail("数据不存在");
            return ApiResult.Success(entity);
        }

        /// <summary>获取模型</summary>
        /// <param name="filterRequest"></param>
        /// <returns></returns>
        [HttpGet("get")]
        public virtual ApiResult Get([FromQuery] string where)
        {
            var filters = GetFilters(where);
            return ApiResult.Success(service.Get(filters));
        }

        /// <summary>获取列表</summary>
        /// <param name="where">查询条件 And Or 查询 不支持嵌套</param>
        /// <returns></returns>
        [HttpGet("list"), Permission("list", "列表")]
        public virtual ApiListResult<T> List([FromQuery] string where)
        {
            var filters = GetFilters(where);

            return service.List(filters);
        }

        /// <summary>获取分页列表</summary>
        /// <param name="pageRequest"></param>
        /// <returns></returns>
        [HttpGet("page"), Permission("page", "分页")]
        public virtual ApiPageResult Page([FromQuery] PageRequest pageRequest)
        {
            return service.Page(pageRequest);
        }
    }
}