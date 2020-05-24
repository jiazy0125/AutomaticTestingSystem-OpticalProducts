using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Model
{
    public class ContextMenuModel :PropertyChangedModel
    {

        private RelayCommand _command;
        private string _event="Click";
        private bool _enable = false;

        private object _parameter = null;

        public MenuKey Name { get; }
        public RelayCommand Command 
        { 
            get=>_command;
            private set => NotifyPropertyChanged(ref _command, value);
        } 
        public PackIconKind IconKind { get; }
        public string Header { get;}
        public string Event
        {
            get => _event;
            private set => NotifyPropertyChanged(ref _event, value);
        }
        public string Shortcut { get;  }
        public bool IsSeparator { get; } = false;
        public bool IsEnable
        {
            get => _enable;
            private set => NotifyPropertyChanged(ref _enable, value);
        }

        public object Parameter
        {
            get => _parameter;
            private set => NotifyPropertyChanged(ref _parameter, value);
        }

        public ContextMenuModel(MenuKey _name, string _header, PackIconKind _iconKind, bool _isSeparator = false)
        {
            Name = _name;
            Header = _header;
            IconKind = _iconKind;
            IsSeparator = _isSeparator;
            Command = new RelayCommand(_ =>{ });
        }
        public ContextMenuModel() { }

        public void Register(RelayCommand _command, string _event, bool _isEnabel, object _parameter = null)
        {
            Event = _event ?? Event;
            Command = _command ?? Command;
            IsEnable = _isEnabel;
            Parameter = _parameter;
        }
    }   
}
