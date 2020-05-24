using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.PNManagment
{
    [Database("PartNumberConfig")]
    public class PartNumberConfigModel : PropertyChangedModel
    {
        public PartNumberConfigModel() { }

        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                NotifyPropertyChanged(ref _isChanged, value);
            }
        }

        [Column("GUID", 0)]
        public string Guid { get; set; }
        private string _pn = "";
        [Column("PartNumber", 1)]
        public string PartNumber
        {
            get => _pn;
            set
            {
                NotifyPropertyChanged(ref _pn, value);
            }
        }

        private string _productGuid = "";
        [Column("ProductType", 2)]
        public string ProductType
        {
            get => _productGuid;
            set
            {
                NotifyPropertyChanged(ref _productGuid, value);
            }
        }
        private string _processGuid = "";
        [Column("ProcessGuid", 3)]
        public string ProcessGuid
        {
            get => _processGuid;
            set
            {

               
                NotifyPropertyChanged(ref _processGuid, value);
                try
                {
                    ProductType = SystemSettings.ProductModels.First(s => s.Guid == (SystemSettings.ProcessItems.First(t => t.GUID == value)?.ProductGuid))?.Name;
                }
                catch { }
            }
        }

        private string _description = "";
        [Column("Description", 4)]
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
            }
        }

        public object Object1 
        { get;
            set; 
        }

    }
}
