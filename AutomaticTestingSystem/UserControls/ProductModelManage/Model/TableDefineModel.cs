using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProductModelManage
{
    [Database("TablesDefine")]
    public class TableDefineModel : PropertyChangedModel
    {
        public TableDefineModel() { }


        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set => NotifyPropertyChanged(ref _isChanged, value);
        }

        private bool _isChecked = false;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                NotifyPropertyChanged(ref _isChecked, value);
            }
        }
        [Column("GUID", 0)]
        public string Guid { get; set; }
        private string _name = "";
        [Column("Name",1)]
        public string Name
        {
            get => _name;
            set
            {
                NotifyPropertyChanged(ref _name, value);
            }
        }
        private string _tableIndex = "00";
        [Column("TableIndex", 2)]
        public string TableIndex
        {
            get => _tableIndex;
            set
            {
                NotifyPropertyChanged(ref _tableIndex, value);
            }
        }
        private string _description = "";
        [Column("Description", 3)]
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
            }
        }
        private string _startAddress = "00";
        [Column("StartAddress", 4)]
        public string StartAddress
        {
            get => _startAddress;
            set
            {
                NotifyPropertyChanged(ref _startAddress, value);
                //IsChanged = UpdateChangeFlag(value);
            }
        }
        private string _parent = "";
        [Column("Parent", 5)]
        public string Parent
        {
            get => _parent;
            set
            {
                NotifyPropertyChanged(ref _parent, value);
                //IsChanged = UpdateChangeFlag(value);
            }
        }

    }
}
