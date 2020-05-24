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
using AutomaticTestingSystem.InstrumentDriver;
using AutomaticTestingSystem.UserControls.ProductModelManage;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.UserControls.ModuleTables
{
    /// <summary>
    /// TablesView.xaml 的交互逻辑
    /// </summary>
    public partial class TablesView : UserControl, IData
    {
        public ObservableCollection<TableInfoModel> ItemList = new ObservableCollection<TableInfoModel>();
        public TablesView()
        {
            InitializeComponent();
            LoadProductModel();
            TableContentDataGrid.ItemsSource = ItemList;



        }

        private async void Unlock_Click(object sender, RoutedEventArgs e)
        {
            if (UnlockPwTextBox.Text.Trim() == "")
            {
                await this.MsgBox("Unlock password can no be empty");
                return;          
            }
            try
            {
                var pwd = this.ConvertHexStringToBytes(UnlockPwTextBox.Text.Trim());


                Unlock.BadgeBackground = new SolidColorBrush(Colors.Green);

            }
            catch (Exception exp)
            {
                Unlock.BadgeBackground = new SolidColorBrush(Colors.Red);
                await this.MsgBox(exp.Message);
            }

        }

        private async void Write_Click(object sender, RoutedEventArgs e)
        {
            if (EVBComboBox.SelectedItem == null || PdTypeComboBox.SelectedItem == null || TableSelectComboBox.SelectedItem == null)
            {
                await this.MsgBox("Please check EVB\\Product\\Table selected item.");
                return;
            }

            var instr = (InstrumentModel)EVBComboBox.SelectedItem;
            var tt = new EVB1_QsfpDD(instr);
            tt.Open();
            //切表
            tt.Write(0xA0, 0x7F, this.ConvertHexStringToBytes(((TableDefineModel)TableSelectComboBox.SelectedItem).TableIndex));
            var first = true;
            foreach (var item in ItemList)
            {
                byte addr = 0x00;
                var data = new List<byte>();
                //如果是连续变更数据,可以一次性写入
                //遇到非连续性数据,则把之前更改过的数据写入
                if (item.IsChanged)
                {
                    if (first)
                        addr = Convert.ToByte(item.Address, 16);
                    first = false;
                    data.Add(Convert.ToByte(item.Value, 16));
                }
                if (!item.IsChanged && data.Count > 0)
                {
                    first = true;
                    try
                    {
                        tt.Write(0xA0, addr, data.ToArray());
                    }
                    catch (Exception exp)
                    {
                        await this.MsgBox(exp.Message);
                    }
                
                }
            }
            tt.Close();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Read_Click(object sender, RoutedEventArgs e)
        {
            if (EVBComboBox.SelectedItem == null || PdTypeComboBox.SelectedItem == null || TableSelectComboBox.SelectedItem == null)
            {
                await this.MsgBox("Please check EVB\\Product\\Table selected item.");
                return;
            }

            try
            {
                var instr = (InstrumentModel)EVBComboBox.SelectedItem;

                var tt = new EVB1_QsfpDD(instr);

                tt.Open();
                //切表
                tt.Write(0xA0, 0x7F, this.ConvertHexStringToBytes(((TableDefineModel)TableSelectComboBox.SelectedItem).TableIndex));
                //读取数据
                var data = tt.Read(0xa0, Convert.ToByte(((TableDefineModel)TableSelectComboBox.SelectedItem).StartAddress, 16), 128);
                for (var i = 0; i < data.Length; i++)
                {
                    ItemList[i].Value = data[i].ToString("X2");
                    ItemList[i].CacheOldData(ItemList[i]);
                }

                tt.Close();


            }
            catch (Exception exp)
            {
                await this.MsgBox(exp.Message);
            }
        }

        private void LoadProductModel()
        {
            var sql = "SELECT * FROM ProductModel";
            var res = this.AccessData<ProductModel>(sql);
            PdTypeComboBox.ItemsSource = new ObservableCollection<ProductModel>(res.Data);


        }

        private void PdType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ComboBox)sender;
            var res = this.AccessData(null, new[]
            {
                new ConditionExperssion<TableDefineModel>().Eq(5, item.SelectedValue)
            });

            SystemSettings.TablesDetails.Clear();
            foreach(var t in res.Data)
            {
                SystemSettings.TablesDetails.Add(t);
            }

        }

        private void TableSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemList.Clear();
            var start = Convert.ToByte(((TableDefineModel)TableSelectComboBox.SelectedItem).StartAddress, 16);
            for (var i = 0; i < 128; i++)
            {

                ItemList.Add(new TableInfoModel() { Address = $"0x{(i + start).ToString("X2")}" });
            }
        }
    }
}
