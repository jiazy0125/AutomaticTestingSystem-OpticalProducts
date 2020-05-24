using System.Windows;
using ControlzEx;
using static System.Net.WebRequestMethods;

namespace MaterialDesignThemes.Wpf
{

    public class PackIcon : PackIconBase<PackIconKind>
    {        
        static PackIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIcon), new FrameworkPropertyMetadata(typeof(PackIcon)));
        }

        public PackIcon() : base(PackIconDataFactory.Create) { }
    }
}
