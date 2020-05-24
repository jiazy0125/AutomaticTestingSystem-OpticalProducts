using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    /// <summary>
    /// ProcessItems.xaml 的交互逻辑
    /// </summary>
    public partial class ItemsListBox : UserControl
    {
        private ProcessItemsViewModel pivm;
        public ItemsListBox()
        {

            pivm = new ProcessItemsViewModel() { };
            DataContext = pivm;
            
            InitializeComponent();
        }

        private void ProcessListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ProcessListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            //定义传递参数
            var args = new RoutedEventArgs(PreviewMLBtnDownEvent, e);
            //引用自定义路由事件
            RaiseEvent(args);

        }


        /// <summary>
        /// 声明路由事件
        /// 参数:要注册的路由事件名称，路由事件的路由策略，事件处理程序的委托类型(可自定义)，路由事件的所有者类类型
        /// </summary>
        public static readonly RoutedEvent PreviewMLBtnDownEvent = EventManager.RegisterRoutedEvent("PreviewMLBtnDown", 
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<object>), typeof(ItemsListBox));
        /// <summary>
        /// 处理各种路由事件的方法 
        /// </summary>
        public event RoutedEventHandler PreviewMLBtnDown
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(PreviewMLBtnDownEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(PreviewMLBtnDownEvent, value); }
        }



    }
}
