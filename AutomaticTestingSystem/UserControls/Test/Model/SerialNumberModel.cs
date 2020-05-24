using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.Test
{
    public class SerialNumberModel : PropertyChangedModel
    {

        private string _label;
        private string _sn;
        public SerialNumberModel(string label, string sn)
        {
            Label = label;
            SN = sn;
        }
        public string Label
        {
            get
            {
                return _label;
            }
            set
            {
                NotifyPropertyChanged(ref _label, value);
            }
        }
        public string SN
        {
            get
            {
                return _sn;
            }
            set
            {
                NotifyPropertyChanged(ref _sn, value);
            }
        }

    }


}
