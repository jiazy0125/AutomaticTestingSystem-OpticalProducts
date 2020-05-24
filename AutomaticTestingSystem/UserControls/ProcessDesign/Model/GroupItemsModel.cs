using System.Collections.ObjectModel;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class GroupItemsModel : PropertyChangedModel
    {
        private object _selectedItem;
        private LevelType _currentItemLevel;

        public ObservableCollection<TopItemModel> TopItems { get; }

        public ObservableCollection<ContextMenuModel> ContextMenu { get; } 


        public object SelectedItem 
        {
            get => _selectedItem;
            set => NotifyPropertyChanged(ref _selectedItem, value); 
        }

        public LevelType CurrentItemLevel
        {
            get => _currentItemLevel;
            set => NotifyPropertyChanged(ref _currentItemLevel, value);
        
        }     

        public GroupItemsModel()
        {
            TopItems = new ObservableCollection<TopItemModel>();
            ContextMenu = new ObservableCollection<ContextMenuModel>();
        }


        

    }
}
