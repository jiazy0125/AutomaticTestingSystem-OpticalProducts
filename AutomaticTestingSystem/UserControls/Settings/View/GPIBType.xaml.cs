using System.Windows;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;

namespace AutomaticTestingSystem.UserControls.Settings
{
    /// <summary>
    /// GPIBType.xaml 的交互逻辑
    /// </summary>
    public partial class GPIBType : UserControl
    {
        public GPIBType()
        {
            InitializeComponent();
            GPIBPortComboBox.ItemsSource = NIInstrument.FindAddresses(PortType.GPIB);
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            GPIBPortComboBox.ItemsSource = NIInstrument.FindAddresses(PortType.GPIB);
            SystemSettings.SnackbarMessageQueue.Enqueue("Refresh Complete.");
        }
    }
}
