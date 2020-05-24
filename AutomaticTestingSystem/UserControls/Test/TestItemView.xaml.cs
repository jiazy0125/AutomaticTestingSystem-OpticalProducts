using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.ProcessDesign;

namespace AutomaticTestingSystem.UserControls.Test
{
    /// <summary>
    /// TestItemView.xaml 的交互逻辑
    /// </summary>
    public partial class TestItemView : UserControl
    {
        public TestItemView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 双击测试项执行测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.VisualUpwardSearch<TreeViewItem>((DependencyObject)e.OriginalSource) is TreeViewItem treeViewItem)
            {
                if (treeViewItem.DataContext is SubItemModel item)
                {

                    Task.Factory.StartNew(() =>
                    {
                        //初始化测试结果
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            item.MeasValue = "NA";
                            item.Result = "Waiting . . .";
                        });
                    }).ContinueWith(t =>
                    {
                        var task = Task.Factory.StartNew(() =>
                        {
                            return this.MethodInvoke<object>(SystemSettings.Procedure, item.SelectedProcedure, item);
                        }).ContinueWith(tt =>
                        {
                            //更新测试结果
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                item.MeasValue = tt.Result?.Data?.ToString();
                                item.Result = "Pass";
                            });

                        });
                        task.Wait();
                    });
                }           
            }
        }
    }
}
