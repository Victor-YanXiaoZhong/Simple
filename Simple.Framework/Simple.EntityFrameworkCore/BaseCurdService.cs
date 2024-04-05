using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Simple.Utils;
using Simple.Utils.Attributes;
using Simple.Utils.Models;
using Simple.Utils.Models.Entity;

namespace Simple.EntityFrameworkCore
{
    public class BaseCurdService<T> : ICurdService<T> where T : class
    {
        protected readonly AppDbContext appDb;
        protected DbSet<T> table;

        public BaseCurdService(AppDbContext appDb)
        {
            this.appDb = appDb;
            table = appDb.Set<T>();
        }

        /// <summary>获取变量</summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> GetWhere(List<Filter> filters)
        {
            return FiterExpressHelper.GetWhere<T>(filters);
        }

        public virtual bool Add(T model)
        {
            appDb.Add(model);
            return appDb.SaveChanges() == 1;
        }

        public virtual bool Delete(object id)
        {
            var entity = appDb.Find<T>(id);
            if (entity is null)
                throw new CustomException("根据Id 查询不到数据");

            appDb.Remove(entity);
            return appDb.SaveChanges() == 1;
        }

        public virtual bool Edit(object id, ParameterModel model)
        {
            var entity = appDb.Find<T>(id);
            if (entity is null)
                throw new CustomException("根据Id 查询不到数据");

            entity.TryFromDict(model);
            appDb.Update(entity);
            return appDb.SaveChanges() == 1;
        }

        public virtual T? Get(object id)
        {
            var entity = appDb.Find<T>(id);

            if (entity is null)
                throw new CustomException("根据Id 查询不到数据");

            return entity;
        }

        public virtual T? Get(Expression<Func<T, bool>> where)
        {
            var entity = table.Where(where).FirstOrDefault();

            if (entity is null)
                throw new CustomException("根据Id 查询不到数据");

            return entity;
        }

        public virtual T? Get(List<Filter> filters)
        {
            var model = table.Where(GetWhere(filters)).FirstOrDefault();

            return model;
        }

        public virtual ApiListResult<T> List(List<Filter> filters)
        {
            var list = table.Where(GetWhere(filters)).ToList();
            return ApiListResult<T>.Success(list);
        }

        public virtual List<T> List(Expression<Func<T, bool>> where)
        {
            var list = table.Where(where).ToList();
            return list;
        }

        public virtual ApiPageResult Page(PageRequest pageRequest)
        {
            var list = table.Where(GetWhere(pageRequest.Filters));
            var page = list.Skip((pageRequest.Page - 1) * pageRequest.Size)
                        .Take(pageRequest.Size)
                        .ToList();
            return ApiPageResult.Success(page, list.Count());
        }

        public virtual bool SetFlag(object id, ParameterModel model)
        {
            var entity = appDb.Find<T>(id);
            if (entity is null)
                throw new CustomException("根据Id 查询不到数据"); ;

            var update = appDb.Attach(entity);

            foreach (var item in model)
            {
                try
                {
                    var field = update.Property(item.Key);
                    if (field is null)
                        throw new FatalException($"模型{typeof(T)}中不存在属性 {item.Key}");

                    field.CurrentValue = item.Value;
                    field.IsModified = true;
                }
                catch (Exception ex)
                {
                    throw new FatalException($"模型{typeof(T)}中 属性 {item.Key} 赋值没有成功 {ex.Message}", ex);
                }
            }
            return appDb.SaveChanges() == 1;
        }
    }
}