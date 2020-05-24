using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace AutomaticTestingSystem.Framework.Converters
{
    class TreeViewItemSelectedConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return Visibility.Collapsed;
            if (!(values[1] is TreeView view)) return Visibility.Collapsed;
            var item = (TreeViewItem)view.ItemContainerGenerator.ContainerFromItem(values[0]);
            if (item != null)
            {
                return item.IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
