using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{

    public static class GenericExtensions
    {
        public static bool In<T>(this T source, params T[] target)
            where T : class
        {
            source.ThrowIfNull(nameof(source));

            return target.Contains(source);
        }

        public static bool Between<T>(this T source, T lower, T upper)
            where T : IComparable<T>
        {
            return source.CompareTo(lower) > 0 && source.CompareTo(upper) < 0;
        }

        public static bool Intersects<T>(this T source, T lower, T upper)
            where T : IComparable<T>
        {
            return source.CompareTo(lower) >= 0 && source.CompareTo(upper) <= 0;
        }

        public static void ThrowIfNull<T>(this T source, string name)
            where T : class
        {
            if (source.IsNull())
                throw new ArgumentNullException(name);
        }       

        public static bool LessThan<T>(this T source, T value)
            where T : IComparable<T> 
        {
            return source.CompareTo(value) < 0;
        }

        public static bool LessThanOrEqual<T>(this T source, T value)
            where T : IComparable<T>
        {
            return source.CompareTo(value) <= 0;
        }

        public static bool GreatherThan<T>(this T source, T value)
            where T : IComparable<T>
        {
            return source.CompareTo(value) > 0;
        }

        public static bool GreatherThanOrEqual<T>(this T source, T value)
            where T : IComparable<T>
        {
            return source.CompareTo(value) >= 0;
        }

        public static T Clamp<T>(this T source, T min, T max)
            where T : IComparable<T>
        {
            if (source.LessThan(min))
                return min;

            if (source.GreatherThan(max))
                return max;

            return source;
        }

        public static bool IsNull<TSource>(this TSource source)
        {
            return source == null;
        }

        public static bool IsNull<TSource>(this TSource source, Func<TSource, object> obj)
        {
            if (source.IsNull())
                return true;

            if (obj.IsNull())
                return true;

            return obj(source).IsNull();
        }

        public static T ToType<T>(this object source)
        {
            return source is T ? (T)source : default(T);
        }

        public static IDictionary<string, object> ToDictionary(this object source)
        {
            source.ThrowIfNull(nameof(source));

            return source.GetType()
                .GetProperties()
                .ToDictionary(property => property.Name, property => property.GetValue(source, null));
        }

        public static T ToType<T>(this IDictionary<string, object> source)
           where T : class, new()
        {
            source.ThrowIfNull(nameof(source));

            T instance = new T();

            Type type = source.GetType();

            source.ToList().ForEach(keyValue => type.GetProperty(keyValue.Key)
                .SetValue(instance, keyValue.Value, null));

            return instance;
        }

        public static T SelectRandom<T>(this Random source, params T[] target)
        {
            source.ThrowIfNull(nameof(source));

            target.ThrowIfNull(nameof(target));

            return target[source.Next(target.Length)];
        }

        public static T SelectRandom<T>(this Random source, IEnumerable<T> target)
        {
            source.ThrowIfNull(nameof(source));

            target.ThrowIfNull(nameof(target));

            int index = source.Next(0, target.Count());

            return target.ElementAt(index);
        }
    }
}
