using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{

    [Database("ProcessInfo")]
    public class ProcessListModel : PropertyChangedModel
    {

        public ProcessListModel() { }
        private string _guid;
        private string _name;
        private string _description;

        [Column("GUID", 0)]
        public string GUID
        {
            get => _guid;
            set => NotifyPropertyChanged(ref _guid, value);
        }

        [Column("Name", 1)]
        public string Name
        {
            get => _name;
            set => NotifyPropertyChanged(ref _name, value);
        }

        [Column("Description", 2)]
        public string Description
        {
            get => _description;
            set => NotifyPropertyChanged(ref _description, value);
        }

        [Column("Creator", 3)]
        public string Creator { get; set; }

        [Column("CreateTime", 4)]
        public string CreateTime { get; set; }

        private string _productGuid;
        [Column("ProductGuid", 5)]
        public string ProductGuid 
        {
            get => _productGuid;
            set
            {
                NotifyPropertyChanged(ref _productGuid, value);
            }
        }

    }
}
