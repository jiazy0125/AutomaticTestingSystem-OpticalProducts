using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AutomaticTestingSystem.Framework.Converters
{
    class PassFailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            switch (((string)value).ToLower())
            {
                case "pass":
                    return new SolidColorBrush(Colors.Green);
                case "fail":
                    return new SolidColorBrush(Colors.Red);
                default:
                    return null;
            
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; ;
        }
    }
}
