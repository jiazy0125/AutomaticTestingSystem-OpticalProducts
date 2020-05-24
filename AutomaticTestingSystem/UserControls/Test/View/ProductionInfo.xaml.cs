using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Interfaces;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.PNManagment;
using AutomaticTestingSystem.Framework.Database;
using System.Linq;

namespace AutomaticTestingSystem.UserControls.Test
{
    /// <summary>
    /// ProductionInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionInfo : UserControl, IController, IData
    {
        public ProductionInfo()
        {
            InitializeComponent();
        }

        public PropertyChangedModel Model => null;



        private async void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Trim() != "")
            {

                var res = this.AccessData(new int[0], new[]
                {
                    new ConditionExperssion<PartNumberConfigModel>().Eq(1,tb.Text.Trim())

                });
                if (res.Status && res.Data != null && res.Data?.Count > 0)
                {
                    this.LoadAllItems(res.Data[0].ProcessGuid, SystemSettings.GroupItems, false);
                    SystemSettings.VariantDic.Clear();
                    return;
                }

                await this.MsgBox("Can not find any configuration for current PN");

            }

        }
    }
}
