using Microsoft.EntityFrameworkCore;

using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infrastructure.Linq
{
    /// <summary>
    /// Linq扩展类
    /// </summary>
    public static class LinqQuery
    {
        /// <summary>
        /// 构建筛选表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exps"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildFilterLambda<T>(string exps) where T : class
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

        /// <summary>
        /// 根据条件执行Include方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="source">IQueryable对象</param>
        /// <param name="condition">条件</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> expression)
            where T : class => 
            condition
                ? source.Include(expression)
                : source;

        /// <summary>
        /// 根据条件执行Where方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">IQueryable对象</param>
        /// <param name="condition">条件</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> expression) => 
            condition
                ? source.Where(expression)
                : source;

    }
}
