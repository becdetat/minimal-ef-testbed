using System.Linq;
using System.Reflection;

namespace MinimalEF
{
    public static class QueryableExtensions
    {
        public static string GetSql<T>(IQueryable<T> query)
        {
            var property = typeof (System.Data.Entity.Infrastructure.DbQuery<T>)
                .GetProperty("InternalQuery", (BindingFlags) int.MaxValue);

            return property.GetValue(query).ToString();
        }
    }
}