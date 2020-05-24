using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Interfaces;

namespace AutomaticTestingSystem.UserControls.PNManagment
{
    /// <summary>
    /// PartNumber.xaml 的交互逻辑
    /// </summary>
    public partial class PartNumber : UserControl,IData
    {
        private ObservableCollection<PartNumberConfigModel> pnList = new ObservableCollection<PartNumberConfigModel>();
        public PartNumber()
        {
            InitializeComponent();
            //加载所有PN信息

            foreach (var item in this.AccessData<PartNumberConfigModel>(new int[0]).Data)
            {
                pnList.Add(item);
            }

            PNDataGrid.ItemsSource = pnList;

        }

        private void PNAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = new PartNumberConfigModel() {
                Guid = Guid.NewGuid().ToString("N").ToUpper(),
                PartNumber="00000000000000"
            };

            if (this.SaveData(item, null).Status)
                pnList.Add(item);
        }

        private async void PNDel_Click(object sender, RoutedEventArgs e)
        {
            var message = "All selected item will be permanently deleted.\r\n" +
                                 "Please click 'DELETE' to confirm.";
            var result = await this.MsgBox(message, "DELETE", "CANCEL");
            if ((bool)result)
            {
                var deleted = new List<PartNumberConfigModel>();
                foreach (PartNumberConfigModel item in pnList)
                {
                    if (item.IsChanged) deleted.Add(item);
                }
                foreach (var del in deleted)
                {

                    var res = this.DeleteData(new[]
                    {
                            new ConditionExperssion<PartNumberConfigModel>().Eq(0,del.Guid)
                        });
                    if (res.Status) pnList.Remove(del);

                }
            }

            foreach (var item in pnList)
            { 
            
            
            }
        }

        private void PNSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (PartNumberConfigModel item in pnList)
            {
                this.UpdateData(new[] { 1, 2, 3, 4 }, item, new[]
                {
                new ConditionExperssion<PartNumberConfigModel>().Eq(0, item.Guid)
                });
            }
        }

        private void PNQuery_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
