using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime source)
        {
            var age = DateTime.Now.Year - source.Year;

            if (DateTime.Now < source.AddYears(age))
                age--;

            return age;
        }

        public static bool WorkingDay(this DateTime source)
        {
            return source.DayOfWeek != DayOfWeek.Saturday && source.DayOfWeek != DayOfWeek.Sunday;
        }

        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday || source.DayOfWeek == DayOfWeek.Sunday;
        }

        public static DateTime Next(this DateTime source, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - source.DayOfWeek;

            if (offsetDays <= 0)
                offsetDays += 7;
            
            DateTime result = source.AddDays(offsetDays);

            return result;
        }
    }
}
