using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osc.Dejection.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum obj)
        {
            if (obj.IsNull())
                return string.Empty;

            return obj.GetType()
                .GetField(obj.ToString())
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                ?.FirstOrDefault() ?? obj.ToString();
        }

        public static string GetDisplay(this Enum obj)
        {
            if (obj.IsNull())
                return string.Empty;

            return obj.GetType()
            .GetField(obj.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .Cast<DisplayAttribute>()
            .Select(x => x.Name)
            ?.FirstOrDefault() ?? obj.ToString();
        }
    }
}
