using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Interfaces;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.UserControls.InputDialog;
using MaterialDesignThemes.Wpf;

namespace AutomaticTestingSystem.UserControls.ProductModelManage
{
    /// <summary>
    /// ProductManageView.xaml 的交互逻辑
    /// </summary>
    public partial class ProductManageView : UserControl,IData
    {
        public ProductManageView()
        {
            InitializeComponent();
        }

        private async void DeleteBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            var message = "All information related to the selected item will be permanently deleted.\r\n" +
                                           "Please click 'DELETE' to confirm.";
            var result = await this.MsgBox(message, "DELETE", "CANCEL");
            if ((bool)result)
            {
                var res = this.DeleteData(new[]
                    {
                         new ConditionExperssion<ProductModel>().Eq(0,((ProductModel)ProductModelListBox.SelectedItem).Guid)
                    });
                if (res.Status)  SystemSettings.ProductModels.Remove((ProductModel)ProductModelListBox.SelectedItem);
            }
        }

        private async void NewBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            var dialog2 = new InputDialog2();
            dialog2.Model().WarningMsg1 = "Input not available!";
            dialog2.Model().Input1Validation = (
            s =>
            {
                return !SystemSettings.ProductModels.Any(t => t.Name.ToUpper() == s.ToUpper());
            });
            var result = await DialogHost.Show(dialog2, "RootDialog", new DialogClosingEventHandler((s, t) => { }));
            //判断输入是否确认，并添加新增项至列表
            if ((bool)result)
            {
                var item = new ProductModel() {
                    Guid = Guid.NewGuid().ToString("N").ToUpper(),
                    Name = dialog2.Model().Input1,
                    Description = dialog2.Model().Input2
                };
                var res = this.SaveData(item, null);
                if (res.Status)
                    SystemSettings.ProductModels.Add(item);
                else
                    await this.MsgBox(res.Message);
            }
        }


        //private void LoadProductModelList()
        //{
        //    var sql = "SELECT * FROM ProductModel";
        //    var res=  this.AccessData<ProductModel>(sql);
        //    itemsource = new ObservableCollection<ProductModel>(res.Data);
        //    ProductModel.ItemsSource = itemsource;
        //}

        private void TableAdd_Click(object sender, RoutedEventArgs e)
        {

            var item = new TableDefineModel() 
            {
                Guid = Guid.NewGuid().ToString("N").ToUpper(),
                TableIndex = "00",
                Name = "A0",
                Parent = ((ProductModel)ProductModelListBox.SelectedItem).Guid,
                StartAddress = "00"
            };

            var res= this.SaveData(item, null);
            if (res.Status)
            {
                SystemSettings.TablesDetails.Add(item);
            }


        }

        private async void TableDel_Click(object sender, RoutedEventArgs e)
        {
            var message = "All selected item will be permanently deleted.\r\n" +
                                 "Please click 'DELETE' to confirm.";
            var result = await this.MsgBox(message, "DELETE", "CANCEL");
            if ((bool)result)
            {
                var deleted = new List<TableDefineModel>();
                foreach (TableDefineModel item in TablesDataGrid.Items)
                {
                    if (item.IsChecked) deleted.Add(item);
                }
                foreach (var del in deleted)
                {

                        var res = this.DeleteData(new[]
                        {
                            new ConditionExperssion<TableDefineModel>().Eq(0,del.Guid)
                        });
                    if (res.Status) SystemSettings.TablesDetails.Remove(del);
                    
                }
            }

        }


        private void TableSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (TableDefineModel item in TablesDataGrid.Items)
            {
                this.UpdateData(new[] { 1, 2, 3, 4 }, item, new[] 
                {
                new ConditionExperssion<TableDefineModel>().Eq(0, item.Guid)
                });                               
            }
        }

        private void ProductModelListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (this.VisualUpwardSearch<ListBoxItem>((DependencyObject)e.OriginalSource) is ListBoxItem listBoxItem)
            {
                listBoxItem.IsSelected = true;
                e.Handled = true;

                var product = (ProductModel)listBoxItem.DataContext;

                var res = this.AccessData(null, new[]
                {
                        new ConditionExperssion<TableDefineModel>().Eq(5, product.Guid)
                    });
                SystemSettings.TablesDetails.Clear();
                foreach (var item in res.Data)
                {
                    SystemSettings.TablesDetails.Add(item);
                }


            }





        }
    }
}
