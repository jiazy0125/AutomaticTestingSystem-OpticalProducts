using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class SerialPortCfgModel : PropertyChangedModel, IConfiguration
    {
        public SerialPortCfgModel() 
        {
            //AllPorts = new ObservableCollection<string>();
        }

        //public ObservableCollection<string> AllPorts { get; }

        private string _portName = "";
        public string PortName 
        {
            get => _portName;
            set => NotifyPropertyChanged(ref _portName, value);
        }

        private int _baudRate = 9600;
        public int BaudRate 
        {
            get => _baudRate;
            set => NotifyPropertyChanged(ref _baudRate, value);
        }
        private int _dataBits = 8;
        public int DataBits 
        {
            get => _dataBits;
            set => NotifyPropertyChanged(ref _dataBits, value); 
        }

        private Parity _parity = Parity.None;
        public Parity Parity 
        {
            get => _parity;
            set => NotifyPropertyChanged(ref _parity, value);
        }

        private StopBits _stopBits = StopBits.One;
        public StopBits StopBits 
        {
            get => _stopBits;
            set => NotifyPropertyChanged(ref _stopBits, value);
        }

        private bool _isAsync = false;
        public bool IsAsync 
        {
            get => _isAsync;
            set => NotifyPropertyChanged(ref _isAsync, value);
        }
    }
}
