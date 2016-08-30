using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Osc.Dejection.Themes.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(ImageButton), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(ImageButton), new PropertyMetadata(double.NaN));

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }            

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
    }
}
