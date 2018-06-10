using System;
using System.Collections.Generic;
using System.Linq;

namespace TokenCalculator.Useful
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);
        }

        public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, params T[] items)
        {
            return enumerable.Where(x => !items.Contains(x));
        }
    }
}
