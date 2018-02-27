using System.Collections.Generic;

namespace Mail.Model
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
