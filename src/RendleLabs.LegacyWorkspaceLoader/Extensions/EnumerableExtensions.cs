using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
        {
            foreach (var item in source)
            {
                if (!(item is null))
                {
                    yield return item!;
                }
            }
        }

        public static IEnumerable<string> WhereNotNullOrWhitespace(this IEnumerable<string?> source)
        {
            foreach (var item in source)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    yield return item!;
                }
            }
        }
    }
}