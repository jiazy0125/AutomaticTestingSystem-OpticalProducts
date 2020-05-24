using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    /// <summary>
    /// GroupItems.xaml 的交互逻辑
    /// </summary>
    public partial class GroupItems : UserControl
    {

        public GroupItems()
        {
            DataContext = new GroupItemsViewModel();

            InitializeComponent();
        }
    }
}
