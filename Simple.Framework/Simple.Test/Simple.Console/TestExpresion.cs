using System.Linq.Expressions;
using System.Reflection;

namespace Simple.Console.Test
{
    internal class TestExpresion
    {
        public void RunTest()
        {
            var t = new { id = "10", name = "Admin", age = 23, intID = 12 };
            var result = ExpressionWhere(t, _ => t.id);
            var result1 = ExpressionWhere(t, _ => t.intID);
            var result2 = ExpressionWhere(t, _ => new { t.id });
        }

        /// <summary>表达式测试</summary>
        /// <param name="t"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public (string name, object value) ExpressionWhere(object t, Expression<Func<object, object>> where)
        {
            MemberExpression body = null;
            PropertyInfo member = null;

            body = where.Body as MemberExpression;

            if (body != null)
            {
                member = body.Member as PropertyInfo;
                var name0 = member.Name;
                var value0 = member.GetValue(t, null);
                return (name0, value0);
            }

            var newBody = where.Body as NewExpression;

            if (newBody != null)
            {
                foreach (var item in newBody.Arguments)
                {
                    body = item as MemberExpression;
                    member = body.Member as PropertyInfo;

                    var name = member.Name;
                    var value = member.GetValue(t, null);
                    return (name, value);
                }
            }
            var bodyUnExpression = where.Body as UnaryExpression;

            if (bodyUnExpression != null && bodyUnExpression.Operand is MemberExpression)
            {
                body = bodyUnExpression.Operand as MemberExpression;

                member = body.Member as PropertyInfo;

                var name = member.Name;
                var value = member.GetValue(t, null);
                return (name, value);
            }
            return ("", null);
        }
    }
}