using System.Linq;
using System.Linq.Expressions;
using System.Web.Helpers;

namespace SalesStatisticsSystem.DataAccessLayer.Support.Adapter
{
    public static class QueryableSupport
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> items, string propertyName, SortDirection direction)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetProperty(propertyName)?.PropertyType;
            var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var orderByMethod = (direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending");
            var expression = Expression.Call(typeof(Queryable), orderByMethod, new[] { typeOfT, propertyType },
                items.Expression, Expression.Quote(orderExpression));
            return items.Provider.CreateQuery<T>(expression);
        }
    }
}