using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AutomaticTestingSystem.Framework.Converters
{
    class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if (value is Visibility vb)
                return vb == Visibility.Collapsed || vb == Visibility.Hidden;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
