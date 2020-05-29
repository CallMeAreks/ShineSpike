using System.Collections.Generic;
using System.Linq;

namespace ShineSpike.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageSize, int page)
        {
            return source.Skip(pageSize * page).Take(pageSize);
        }
    }
}
