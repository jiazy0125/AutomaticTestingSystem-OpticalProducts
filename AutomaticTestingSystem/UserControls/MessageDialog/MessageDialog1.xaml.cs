using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.MessageDialog
{
    /// <summary>
    /// MessageDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialog1 : UserControl, IModelService<MessagDialogModel>
    {
        public MessageDialog1()
        {
            DataContext = new MessageDialogViewModel();
            InitializeComponent();
        }
    }
}
