using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class GPIBCfgModel: PropertyChangedModel, IConfiguration
    {
        public GPIBCfgModel()
        {

        }

        private string _gpibAddress;
        public string GPIBAddress
        {
            get => _gpibAddress;
            set => NotifyPropertyChanged(ref _gpibAddress, value);
        }

    }
}
