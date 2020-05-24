using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Controls;

namespace AutomaticTestingSystem.UserControls.Settings
{
    /// <summary>
    /// PowerSupply.xaml 的交互逻辑
    /// </summary>
    public partial class SerialType : UserControl
    {
        //DataContext的类型为串口配置modle:SerialPortCfgModel
        public SerialType()
        {
            InitializeComponent();
            PortComboBox.ItemsSource = SerialPort.GetPortNames();
        }


        private void Refresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PortComboBox.ItemsSource = SerialPort.GetPortNames();
            SystemSettings.SnackbarMessageQueue.Enqueue("Serial port list refreshed complete.");
        }
    }
}
