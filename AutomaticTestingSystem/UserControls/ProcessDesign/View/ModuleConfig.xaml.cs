using System.Windows;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Interfaces;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    /// <summary>
    /// ModuleConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ModuleConfig : UserControl, IData
    {
        public ModuleConfig()
        {
            DataContextChanged += ModuleConfig_DataContextChanged;
            InitializeComponent();
        }

        private void ModuleConfig_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
             var aa = "string";
        }

        public string XmlExpression
        {
            get { return (string)GetValue(XmlExpressionProperty); }
            set
            {
                SetValue(XmlExpressionProperty, value);
            }
        }
        public static readonly DependencyProperty XmlExpressionProperty = DependencyProperty.Register("XmlExpression", typeof(string), typeof(ModuleConfig),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));



        private static void OnValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ((ModuleConfig)target).XmlExpression = (string)e.NewValue;
        }



    }
}
