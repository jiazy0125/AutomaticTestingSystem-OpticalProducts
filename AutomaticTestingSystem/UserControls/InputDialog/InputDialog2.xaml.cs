using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem.UserControls.InputDialog
{
    /// <summary>
    /// InputDialog2.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog2 : UserControl, IModelService<InputDialogModel>
    {
        private InputDialogViewModel viewModel;
        public InputDialog2()
        {
            viewModel = new InputDialogViewModel();
            DataContext = viewModel;
            InitializeComponent();

            
        }
    }
}
