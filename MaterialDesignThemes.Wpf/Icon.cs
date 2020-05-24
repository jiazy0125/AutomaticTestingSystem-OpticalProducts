using System.Windows;
using System.Windows.Controls;

namespace MaterialDesignThemes.Wpf
{

    public class Icon : Control
    {
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type), typeof (IconType), typeof (Icon), new PropertyMetadata(default(IconType)));

        /// <summary>
        /// Gets or sets the name of icon being displayed.
        /// </summary>
        public IconType Type
        {
            get { return (IconType) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
    }
}
