using AutomaticTestingSystem.Common;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;
using System.Collections.ObjectModel;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    [Database("TopItemInfo")]
    public class TopItemModel:PropertyChangedModel, ITreeItem
    {
        public TopItemModel()
        {
            SubItems = new ObservableCollection<SubItemModel>();
            CreateChangesFlag(this);
        }
        public TopItemModel(string guid)
        {
            Guid = guid;
            Name = guid;
            SubItems = new ObservableCollection<SubItemModel>();
            CreateChangesFlag(this);
            CacheOldData(this);
        }

        private string _name;
        private string _description = "";
        private bool _isIndependent = false;


        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set => NotifyPropertyChanged(ref _isChanged, value);
        }

        public ObservableCollection<SubItemModel> SubItems { get; }

        [Column("GUID", 0)]
        public string Guid { get; set; }

        [Column("Name", 1)]
        public string Name
        {
            get => _name;
            set
            {
                NotifyPropertyChanged(ref _name, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
        [Column("Description", 2)]
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
        public LevelType Level { get; } = LevelType.Group;
        /// <summary>
        /// 排序序号,代表该操作执行的顺序
        /// </summary>
        private int _sequence = 0;
        [Column("Sequence", 3)]
        public int Sequence 
        {
            get => _sequence;
            set
            {
                _sequence = value;
                IsChanged = UpdateChangeFlag(value);
            }
        }
        [Column("Parent", 4)]
        public string Parent { get; set; } = "";
      
        [Column("Independent", 5)]
        public bool Independent
        {
            get
            {
                return _isIndependent;
            }
            set
            {
                NotifyPropertyChanged(ref _isIndependent, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
       


    }

}
