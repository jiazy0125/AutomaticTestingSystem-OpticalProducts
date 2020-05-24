using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Model;
using System.Windows;
using AutomaticTestingSystem.Framework.Common;
using System.Collections.ObjectModel;

namespace AutomaticTestingSystem.UserControls.InputDialog
{
    public class InputDialogModel : PropertyChangedModel
    {

        private string _input1;
        private string _input2;
        private bool _enable = false;
        private string _warning1;
        private string _warning2;
        private string _label1 = "Name";
        private string _label2 = "Description";
        private string _label3 = "";

        private Visibility _visibility1 = Visibility.Collapsed;
        private Visibility _visibility2 = Visibility.Collapsed;
        private Visibility _visibility3 = Visibility.Collapsed;


        public InputDialogModel() { }

        public Func<string, bool> Input1Validation { private get; set; } = s => { return true; };
        public Func<string, bool> Input2Validation { private get; set; } = s => { return true; };

        public bool Button1Enable
        {
            get => _enable;
            set => NotifyPropertyChanged(ref _enable, value);
        }

        public string Input1
        {
            get => _input1;
            set
            {
                NotifyPropertyChanged(ref _input1, value);
                var tt = Input1Validation(value);
                Visibility1 = Input1Validation(value) ? Visibility.Collapsed : Visibility.Visible;
                Button1Enable = Input1Validation(value) ? true : false;
            }
        }
        public string Input2
        {
            get => _input2;
            set
            {
                NotifyPropertyChanged(ref _input2, value);
                Visibility2 = Input2Validation(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility Visibility1
        {
            get => _visibility1;
            set => NotifyPropertyChanged(ref _visibility1, value);
        }
        public Visibility Visibility2
        {
            get => _visibility2;
            set => NotifyPropertyChanged(ref _visibility2, value);

        }

        public string WarningMsg1
        {
            get => _warning1;
            set => NotifyPropertyChanged(ref _warning1, value);
        }

        public string WarningMsg2
        {
            get => _warning2;
            set => NotifyPropertyChanged(ref _warning2, value);
        }

        public string Label1
        {
            get => _label1;
            set => NotifyPropertyChanged(ref _label1, value);
        }
        public string Label2
        {
            get => _label2;
            set => NotifyPropertyChanged(ref _label2, value);
        }

        public string Label3
        {
            get => _label3;
            set => NotifyPropertyChanged(ref _label3, value);
        }

        private object _input3;
        public object Input3
        {
            get => _input3;
            set
            {
                NotifyPropertyChanged(ref _input3, value);
            }
        }

        private object _itemsSource;
        public object ItemsSource 
        {
            get => _itemsSource;
            set
            {
                NotifyPropertyChanged(ref _itemsSource, value);
            }
        } 



    }
}
