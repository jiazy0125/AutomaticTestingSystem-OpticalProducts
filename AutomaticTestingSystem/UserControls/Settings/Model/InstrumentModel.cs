using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Settings
{
    public class InstrumentModel:PropertyChangedModel
    {

        public InstrumentModel() { }


        private string _name;
        public string Name
        {
            get => _name;
            set => NotifyPropertyChanged(ref _name, value);
        }

        private string _description ;
        public string Description
        {
            get => _description;
            set => NotifyPropertyChanged(ref _description, value);
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get => _image;
            set => NotifyPropertyChanged(ref _image, value);
        }

        private CommunicationType _communicationType;
        public CommunicationType CommnunicationType
        {
            get => _communicationType;
            set => NotifyPropertyChanged(ref _communicationType, value);
        }

        private object _config;
        public object Config
        {
            get => _config;
            set => NotifyPropertyChanged(ref _config, value);
        }

        public ICommunication CommReference { get; set; }

    }
}
