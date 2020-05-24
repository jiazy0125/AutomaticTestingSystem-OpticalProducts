using System.Collections.ObjectModel;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    class ProcessItemsModel:PropertyChangedModel
    {
        private ProcessListModel _selectedItem;
        private string _currentProcess;
        public ProcessItemsModel() 
        {
            Items = new ObservableCollection<ProcessListModel>();
        }
        public ObservableCollection<ProcessListModel> Items { get; }
        public ProcessListModel SelectedItem 
        {
            get {return _selectedItem; }
            set
            {
                CurrentProcess = value?.GUID;
                NotifyPropertyChanged(ref _selectedItem, value);

            }
        }

        public string CurrentProcess
        {
            get => _currentProcess;
            set => NotifyPropertyChanged(ref _currentProcess, value);
        
        }

        public void RemoveSelectedItem()
        {

            Items.Remove(SelectedItem);
            SelectedItem = null;
        }

        public void AddProcessItem(ProcessListModel model)
        {
            Items.Add(model);
            SelectedItem = model;
        
        }
        public void UpdateSelectedItem(string name, string description)
        {
            SelectedItem.Name = name;
            SelectedItem.Description = description;
        
        }
    }
}
 