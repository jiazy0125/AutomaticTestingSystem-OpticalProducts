using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ModuleTables
{
    public class TableInfoModel:PropertyChangedModel
    {

        public TableInfoModel() 
        {
            CreateChangesFlag(this);
        }


        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set => NotifyPropertyChanged(ref _isChanged, value);
        }

        private string _addr ;
        public string Address
        {
            get => _addr;
            set
            {
                NotifyPropertyChanged(ref _addr, value);
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
            }
        }
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                NotifyPropertyChanged(ref _value, value);
                ValueStr = Encoding.UTF8.GetString(this.ConvertHexStringToBytes(value));
                IsChanged = UpdateChangeFlag(value);

            }
        }
        private string _valueStr;
        public string ValueStr
        {
            get => _valueStr;
            set
            {
                NotifyPropertyChanged(ref _valueStr, value);
            }
        }











    }
}
