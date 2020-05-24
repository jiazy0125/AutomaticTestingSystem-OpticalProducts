using System.Windows;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;

namespace AutomaticTestingSystem.UserControls.Settings
{
    /// <summary>
    /// USBType.xaml 的交互逻辑
    /// </summary>
    public partial class USBType : UserControl
    {
        public USBType()
        {
            InitializeComponent();
            UsbPortComboBox.ItemsSource = NIInstrument.FindAddresses(PortType.USB);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            UsbPortComboBox.ItemsSource = NIInstrument.FindAddresses(PortType.USB);
            SystemSettings.SnackbarMessageQueue.Enqueue("Refresh Complete."); 
        }
    }
}
