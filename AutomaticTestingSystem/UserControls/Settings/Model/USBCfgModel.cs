using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class USBCfgModel : PropertyChangedModel, IConfiguration
    {
        public USBCfgModel()
        {

        }

        private string _usbAddress ;
        public string UsbAddress
        {
            get => _usbAddress;
            set => NotifyPropertyChanged(ref _usbAddress, value);
        }

      


    }
}
