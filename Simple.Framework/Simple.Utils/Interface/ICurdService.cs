using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Simple.Utils.Models;

namespace Simple.Utils
{
    /// <summary>
    /// 实现增删改查的接口 用于快速搭建基于领域的api
    /// </summary>
    public interface ICurdService<T> where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"> </param>
        /// <returns> </returns>
        bool Add(T model);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"> </param>
        /// <returns> </returns>
        bool Edit(object id, ParameterModel model);

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="model"> </param>
        /// <returns> </returns>
        bool Delete(object id);

        /// <summary>
        /// 设置flag属性
        /// </summary>
        /// <param name="model"> </param>
        /// <returns> </returns>
        bool SetFlag(object id, ParameterModel model);

        #region 查询类

        T? Get(object id);
        T? Get(Expression<Func<T, bool>> where);
        T? Get(List<Filter> filters);

        /// <summary>
        /// 条件分页
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        ApiPageResult Page(PageRequest pageRequest);

        /// <summary>
        ///条件查询
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ApiListResult<T> List(List<Filter> filters);

        List<T> List(Expression<Func<T, bool>> where);
        #endregion 查询类
    }
}