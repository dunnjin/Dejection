using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> DistinctBy<T, U>(this IEnumerable<T> source, Func<T, U> key)
        {
            HashSet<U> keys = new HashSet<U>();

            foreach (T element in source)
            {
                if (keys.Add(key(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
