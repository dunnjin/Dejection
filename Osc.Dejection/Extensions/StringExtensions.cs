using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    public static class StringExtensions
    {
        public static bool Match(this string source, string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(source);
        }

        public static T ToEnum<T>(this string source)
        {
            return ToEnum<T>(source, false);
        }

        public static T ToEnum<T>(this string source, bool ignoreCase)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentNullException(nameof(source));

            MemberInfo[] members = typeof(T).GetFields();

            foreach (MemberInfo info in members)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == source)
                    return (T)Enum.Parse(typeof(T), info.Name);
                
                return (T)Enum.Parse(typeof(T), source, ignoreCase);
            }

            return default(T);
        }

        public static string TruncateAtWord(this string source, int length)
        {
            if (source == null || source.Length < length)
                return source;

            int iNextSpace = source.LastIndexOf(" ", length);

            return string.Format("{0}...", source.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        public static string RemoveLastCharacter(this string source)
        {
            source.ThrowIfNull(nameof(source));

            return source.Substring(0, source.Length - 1);
        }

        public static string RemoveLast(this string source, int number)
        {
            source.ThrowIfNull(nameof(source));

            return source.Substring(0, source.Length - number);
        }

        public static string RemoveFirstCharacter(this string source)
        {
            source.ThrowIfNull(nameof(source));

            return source.Substring(1);
        }

        public static string RemoveFirst(this string source, int number)
        {
            source.ThrowIfNull(nameof(source));

            return source.Substring(number);
        }

        public static short? ToShort(this string source)
        {
            short value;

            return short.TryParse(source, out value) ? (short?)value : null;
        }

        public static int? ToInt(this string source)
        {
            int value;

            return int.TryParse(source, out value) ? (int?)value : null;
        }

        public static long? ToLong(this string source)
        {
            long value;

            return long.TryParse(source, out value) ? (long?)value : null;
        }

        public static double? ToDouble(this string source)
        {
            double value;

            return double.TryParse(source, out value) ? (double?)value : null;
        }

        public static decimal? ToDecimal(this string source)
        {
            decimal value;

            return decimal.TryParse(source, out value) ? (decimal?)value : null;
        }
    }
}
