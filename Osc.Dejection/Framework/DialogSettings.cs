using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Osc.Dejection.Framework
{
    public class DialogSettings : IDialogSettings
    {
        public bool AllowsTransparency { get; set; }

        public bool PreventClosing { get; set; }

        public ResizeMode ResizeMode { get; set; }

        public WindowStartupLocation WindowStartupLocation { get; set; }

        public WindowState WindowState { get; set; }

        public WindowStyle WindowStyle { get; set; }

        public SizeToContent SizeToContent { get; set; }
    }
}
