using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Osc.Dejection.Sample.Extensions
{
    public static class WindowExtensions
    {
        public static void RegisterSettings(this Window window)
        {
            window.WindowState = WindowState.Maximized;
        }
    }
}
