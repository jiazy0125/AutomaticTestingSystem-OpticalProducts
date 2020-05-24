using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class UdpCfgModel : PropertyChangedModel, IConfiguration
    {
        public UdpCfgModel()
        {
            _localIp = this.GetLocalIP();
        }

        private string _remoteIp = "127.0.0.1";
        public string RemoteIP
        {
            get => _remoteIp;
            set => NotifyPropertyChanged(ref _remoteIp, value);
        }

        private int _remotePort = 3030;
        public int RemotePort
        {
            get => _remotePort;
            set => NotifyPropertyChanged(ref _remotePort, value);
        }

        private string _localIp;
        public string LocalIP
        {
            get => _localIp;
            set => NotifyPropertyChanged(ref _localIp, value);
        }

        private int _localPort = 8080;
        public int LocalPort
        {
            get => _localPort;
            set => NotifyPropertyChanged(ref _localPort, value);
        }


    }
}
