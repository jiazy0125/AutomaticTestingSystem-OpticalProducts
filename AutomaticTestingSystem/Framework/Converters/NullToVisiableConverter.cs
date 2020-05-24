using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AutomaticTestingSystem.Framework.Converters
{
    class NullToVisiableConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            if ((string)value == "") return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
