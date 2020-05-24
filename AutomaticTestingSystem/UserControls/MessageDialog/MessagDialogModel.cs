using AutomaticTestingSystem.Framework.Model;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace AutomaticTestingSystem.UserControls.MessageDialog
{
    public class MessagDialogModel:PropertyChangedModel
    {

        private string _btn1Label = "ACCEPT";
        private string _btn2Label = "CANCEL";

        private Visibility _btn1Visiable = Visibility.Visible;
        private Visibility _btn2Visiable = Visibility.Visible;

        private string _message;

        private PackIconKind _icon = PackIconKind.Warning;

        public MessagDialogModel()
        { 
        }

        public string Btn1Label
        {
            get => _btn1Label;
            set
            {
                NotifyPropertyChanged(ref _btn1Label, value);
                if (_btn1Label.Length <= 0) Btn1Visiable = Visibility.Collapsed;
            }
        }

        public string Btn2Label
        {
            get => _btn2Label;
            set
            {
                NotifyPropertyChanged(ref _btn2Label, value);
                if (_btn2Label.Length <= 0) Btn2Visiable = Visibility.Collapsed;
            }
        }
        public Visibility Btn1Visiable
        {
            get => _btn1Visiable;
            set => NotifyPropertyChanged(ref _btn1Visiable, value);
        }
        public Visibility Btn2Visiable
        {
            get => _btn2Visiable;
            set => NotifyPropertyChanged(ref _btn2Visiable, value);
        }
        public string Message
        {
            get => _message;
            set => NotifyPropertyChanged(ref _message, value);
        }
        public PackIconKind Icon
        {
            get => _icon;
            set => NotifyPropertyChanged(ref _icon, value);
        }



    }
}
