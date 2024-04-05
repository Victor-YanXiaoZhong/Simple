using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Utils
{
    /// <summary>操作枚举</summary>
    public enum OpEnum
    {
        /// <summary>!= 不等于</summary>
        notequal,

        /// <summary>= 等于</summary>
        equal,

        /// <summary>包含 like</summary>
        contains,

        /// <summary>大于 &gt;</summary>
        greater,

        /// <summary>大于等于 &gt;=</summary>
        greaterorequal,

        /// <summary>/* 小于 */</summary>
        less,

        /// <summary>/* 小于等于 */</summary>
        lessorequal,

        /// <summary>在 、、之中</summary>
        In,

        /// <summary>在 、、之间</summary>
        between
    }

    internal class FiterExpress : Filter
    {
        public Expression Expression { get; set; }
    }

    /// <summary>条件查询表达式的扩展</summary>
    internal class QueryExpression<T>
    {
        private ParameterExpression parameter;

        public QueryExpression()
        {
            parameter = Expression.Parameter(typeof(T));
        }

        private Expression ParseExpressionBody(List<Filter> conditions)
        {
            if (conditions == null || conditions.Count() == 0)
            {
                return Expression.Constant(true, typeof(bool));
            }

            if (conditions.Count() == 1)
            {
                return ParseCondition(conditions.First());
            }

            var expresses = new List<FiterExpress>();
            conditions.ForEach(condition =>
            {
                var filter = new FiterExpress
                {
                    field = condition.field,
                    isAnd = condition.isAnd,
                    op = condition.op,
                    value = condition.value
                };
                var express = ParseCondition(filter);
                filter.Expression = express;
                expresses.Add(filter);
            });

            Expression lasestExpress = null;
            for (int i = 1; i < expresses.Count; i++)
            {
                if (lasestExpress is null)
                    lasestExpress = expresses[i - 1].Expression;

                var now = expresses[i];
                lasestExpress = now.isAnd ? Expression.And(lasestExpress, now.Expression)
                    : Expression.Or(lasestExpress, now.Expression);
            }
            return lasestExpress;
        }

        private Expression ParseCondition(Filter condition)
        {
            ParameterExpression p = parameter;
            Expression key = null;

            try
            {
                key = Expression.Property(p, condition.field);
            }
            catch (Exception ex)
            {
                throw new FatalException($"模型{p.Type}中不存在字段{condition.field}", ex);
            }

            var opEnum = ParaseOp(condition.op);

            object convertValue = condition.value;
            if (opEnum != OpEnum.In || opEnum != OpEnum.between)
            {
                if (key.Type.Name.ToUpper() == "DATETIME")
                {
                    convertValue = Convert.ToDateTime(condition.value);
                }
                else if (key.Type.Name.ToUpper() == "INT32")
                {
                    convertValue = Convert.ToInt32(condition.value);
                }
                else if (key.Type.Name.ToUpper() == "INT64")
                {
                    convertValue = Convert.ToInt64(condition.value);
                }
                else if (key.Type.Name.ToUpper() == "LONG")
                {
                    convertValue = Convert.ToInt64(condition.value);
                }
                else if (key.Type.Name.ToUpper() == "DOUBLE")
                {
                    convertValue = Convert.ToDouble(condition.value);
                }
                else if (key.Type.Name.ToUpper() == "BOOLEAN")
                {
                    convertValue = Convert.ToBoolean(condition.value);
                }
            }
            Expression value = Expression.Constant(convertValue);

            switch (opEnum)
            {
                case OpEnum.contains:
                    return Expression.Call(key, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), value);

                case OpEnum.equal:
                    return Expression.Equal(key, Expression.Convert(value, key.Type));

                case OpEnum.greater:
                    return Expression.GreaterThan(key, Expression.Convert(value, key.Type));

                case OpEnum.greaterorequal:
                    return Expression.GreaterThanOrEqual(key, Expression.Convert(value, key.Type));

                case OpEnum.less:
                    return Expression.LessThan(key, Expression.Convert(value, key.Type));

                case OpEnum.lessorequal:
                    return Expression.LessThanOrEqual(key, Expression.Convert(value, key.Type));

                case OpEnum.notequal:
                    return Expression.NotEqual(key, Expression.Convert(value, key.Type));

                case OpEnum.In:
                    return ParaseIn(condition);

                case OpEnum.between:
                    return ParaseBetween(condition);

                default:
                    throw new NotImplementedException("不支持此操作");
            }
        }

        /// <summary>Between 转换</summary>
        /// <param name="parameter"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private Expression ParaseBetween(Filter conditions)
        {
            ParameterExpression p = parameter;
            var value = conditions.value.ToString();
            Expression key = Expression.Property(p, conditions.field);
            var valueArr = value.Split(',');
            if (valueArr.Length != 2)
            {
                throw new NotImplementedException("ParaseBetween参数错误");
            }
            try
            {
                int.Parse(valueArr[0]);
                int.Parse(valueArr[1]);
            }
            catch
            {
                throw new NotImplementedException("ParaseBetween参数只能为数字");
            }

            //开始位置
            Expression startvalue = Expression.Constant(int.Parse(valueArr[0]));
            Expression start = Expression.GreaterThanOrEqual(key, Expression.Convert(startvalue, key.Type));

            Expression endvalue = Expression.Constant(int.Parse(valueArr[1]));
            Expression end = Expression.LessThanOrEqual(key, Expression.Convert(endvalue, key.Type));
            return Expression.AndAlso(start, end);
        }

        /// <summary>In 转换 多个值之间 ， 间隔</summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private Expression ParaseIn(Filter conditions)
        {
            ParameterExpression p = parameter;
            var fieldvalue = conditions.value.ToString();
            Expression key = Expression.Property(p, conditions.field);
            var valueArr = fieldvalue.Split(',');
            Expression expression = Expression.Constant(true, typeof(bool));
            foreach (var itemVal in valueArr)
            {
                Expression value = Expression.Constant(itemVal);
                Expression right = Expression.Equal(key, Expression.Convert(value, key.Type));
                expression = Expression.Or(expression, right);
            }
            return expression;
        }

        /// <summary>转换操作符</summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="FatalException"></exception>
        private OpEnum ParaseOp(string op)
        {
            switch (op.ToLower())
            {
                case "=":
                    return OpEnum.equal;

                case "like":
                    return OpEnum.contains;

                case ">":
                    return OpEnum.greater;

                case ">=":
                    return OpEnum.greaterorequal;

                case "!=":
                    return OpEnum.notequal;

                case "<":
                    return OpEnum.less;

                case "<=":
                    return OpEnum.lessorequal;

                case "in":
                    return OpEnum.In;

                case "<between":
                    return OpEnum.between;

                default:
                    throw new FatalException("操作类型 Op 未定义");
            }
        }

        /// <summary>根据Filter获取查询表达式</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters"></param>
        /// <returns></returns>
        public Expression<Func<T, bool>> ParserWhere(List<Filter> filters)
        {
            //将条件转化成表达是的Body
            var query = ParseExpressionBody(filters);
            return Expression.Lambda<Func<T, bool>>(query, parameter);
        }
    }

    public static class FiterExpressHelper
    {
        /// <summary>获取条件表达式</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetWhere<T>(List<Filter> filters)
        {
            var param = new QueryExpression<T>();
            return param.ParserWhere(filters);
        }
    }

    /// <summary>查询条件 And Or 查询 不支持嵌套</summary>
    public class Filter : IEqualityComparer<Filter>
    {
        /// <summary>And 连接 默认</summary>
        public bool isAnd { get; set; } = true;

        /// <summary>字段名称</summary>
        public string field { get; set; }

        /// <summary> 操作符 like 、 = 、！= 、> 、< 、>= 、<= 、in 、 between </summary>
        public string op { get; set; } = "=";

        /// <summary>值</summary>
        public string value { get; set; }

        public static List<Filter> Filters(params Filter[] filter)
        {
            var filers = new List<Filter>(filter);
            return filers.Distinct(new Filter()).ToList();
        }

        public bool Equals(Filter me, Filter other)
        {
            var result = me.field == other.field
                && me.value.ToString() == other.value.ToString()
                && me.op == other.op
                && me.isAnd == other.isAnd;
            return result;
        }

        public int GetHashCode(Filter me)
        {
            return me.ToString().GetHashCode();
        }
    }

    /// <summary>排序</summary>
    public class Sort
    {
        /// <summary>字段</summary>
        public string field { get; set; }

        /// <summary>排序类型</summary>
        public string sort { get; set; } = "ASC";
    }
}