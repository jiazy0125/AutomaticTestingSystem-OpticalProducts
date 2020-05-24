using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace AutomaticTestingSystem.Framework.Converters
{
    class EmptyTextToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;

            var temp = (string)value;
            if (temp.Length > 0) return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class InvertStringToVisibility : IValueConverter
    {
        public Visibility EmptyValue { get; set; } = Visibility.Collapsed;
        public Visibility NotEmptyValue { get; set; } = Visibility.Visible;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return EmptyValue;

            var temp = (string)value;
            if (temp.Length > 0) return NotEmptyValue;

            return EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BoolStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            if (((string)value).ToLower() == "false") return false;
            if (((string)value).ToLower() == "true") return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "false";
            if ((bool)value) return "true";
            else return "false";
        }
    }
}
