using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class TcpIpCfgModel : PropertyChangedModel, IConfiguration
    {
        public TcpIpCfgModel() { }



        private string _ip = "192.168.0.178";
        public string IP
        {
            get => _ip;
            set => NotifyPropertyChanged(ref _ip, value);
        }

        private int _port = 3003;
        public int Port
        {
            get => _port;
            set => NotifyPropertyChanged(ref _port, value);
        }
    }
}
