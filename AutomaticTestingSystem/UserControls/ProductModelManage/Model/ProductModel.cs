using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProductModelManage
{
    [Database("ProductModel")]
    public class ProductModel:PropertyChangedModel
    {

        public ProductModel() { }

        [Column("GUID", 0)]
        public string Guid { get; set; }
        private string _name = "";
        [Column("Name", 1)]
        public string Name
        {
            get => _name;
            set
            {
                NotifyPropertyChanged(ref _name, value);
            }
        }
        private string _description;
        [Column("Description", 2)]
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
            }
        }








    }
}
