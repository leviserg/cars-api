using cars_api.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace cars_api.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyCarSorting<T>(
                this IQueryable<T> source,
                string sortBy,
                string sortOrder) where T : Car
        {

            if (source == null) throw new ArgumentNullException(nameof(source));
            
            if (!CarSortMappings.TryGetValue(sortBy ?? string.Empty, out var sortExpression))
            {
                sortExpression = CarSortMappings["default"];
            }

            return (IQueryable<T>)(sortOrder?.ToLower() == "desc"
                ? source.OrderByDescending(sortExpression)
                : source.OrderBy(sortExpression));
        }

        private static Dictionary<string, Expression<Func<Car, object>>> CarSortMappings = new Dictionary<string, Expression<Func<Car, object>>>
            {
                { "name", c => c.Name },
                { "year", c => c.Year },
                { "brand", c => c.Brand.Name },
                { "default", c => c.Name }
            };
    }
}
