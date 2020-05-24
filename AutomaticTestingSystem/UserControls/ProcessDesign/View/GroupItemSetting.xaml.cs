using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    /// <summary>
    /// SubItemSetting.xaml 的交互逻辑
    /// </summary>
    public partial class GroupItemSetting : UserControl, INotifyPropertyChanged
    {
        private bool _enable = false;

        public GroupItemSetting()
        {
            InitializeComponent();
        }

        public bool Enable
        { 
            get=> _enable;
            set 
            {
                if (EqualityComparer<bool>.Default.Equals(_enable, value)) return;
                _enable = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Enable"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public LevelType CurrentProcessLevel
        {
            get { return (LevelType)GetValue(CurrentProcessLevelProperty); }
            set
            {
                SetValue(CurrentProcessLevelProperty, value);
                Enable = value == LevelType.Item;
            }
        }
        public static readonly DependencyProperty CurrentProcessLevelProperty = DependencyProperty.Register("CurrentProcessLevel", typeof(LevelType), typeof(GroupItemSetting),
            new FrameworkPropertyMetadata(LevelType.Group, new PropertyChangedCallback(OnValueChanged)));



        private static void OnValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ((GroupItemSetting)target).CurrentProcessLevel = (LevelType)e.NewValue;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text == "") return;
            if (SystemSettings.Variants.Any(t => t.Name == tb.Text))
                tb.Foreground = new SolidColorBrush(Colors.Red);

        }


    }
}
