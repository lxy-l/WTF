using System;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Infrastructure.Linq
{
    public static class LinqQuery
    {
        public static Expression<Func<T, bool>> BuildLambda<T>(string exps)
        {
            var sourceType = typeof(T);
            var sourceParameter = Expression.Parameter(sourceType);
            var lambdaExps = DynamicExpressionParser.ParseLambda(
              new[] { sourceParameter },
              typeof(bool),
              exps
            );
            return (Expression<Func<T, bool>>)lambdaExps;
        }
    }
}
