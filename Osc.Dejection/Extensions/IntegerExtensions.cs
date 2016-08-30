using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    /// <summary>
    /// Represents extension methods for integer objects
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns a TimeSpan representing the specified number of milliseconds
        /// </summary>
        /// <param name="source">milliseconds</param>
        /// <returns>TimeSpan in milliseconds</returns>
        public static TimeSpan Milliseconds(this int source)
        {
            return TimeSpan.FromMilliseconds(source);
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of seconds
        /// </summary>
        /// <param name="source">seconds</param>
        /// <returns>TimeSpan in seconds</returns>
        public static TimeSpan Seconds(this int source)
        {
            return TimeSpan.FromSeconds(source);
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of minutes
        /// </summary>
        /// <param name="source">minutes</param>
        /// <returns>TimeSpan in minutes</returns>
        public static TimeSpan Minutes(this int source)
        {
            return TimeSpan.FromMinutes(source);
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of hours
        /// </summary>
        /// <param name="source">hours</param>
        /// <returns>TimeSpan in hours</returns>
        public static TimeSpan Hours(this int source)
        {
            return TimeSpan.FromHours(source);
        }

        /// <summary>
        /// Returns a TimeSpan representing the specified number of days
        /// </summary>
        /// <param name="source">days</param>
        /// <returns>TimeSpan in days</returns>
        public static TimeSpan Days(this int source)
        {
            return TimeSpan.FromDays(source);
        }
         
    }
}
