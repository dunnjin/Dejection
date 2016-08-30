using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Osc.Dejection.Framework
{
    public interface IDialogSettings
    {
        bool PreventClosing { get; set; }

        bool AllowsTransparency { get; set; }

        WindowStyle WindowStyle { get; set; }

        ResizeMode ResizeMode { get; set; }        

        WindowStartupLocation WindowStartupLocation { get; set; }

        WindowState WindowState { get; set; }

        SizeToContent SizeToContent { get; set; }
    }
}
