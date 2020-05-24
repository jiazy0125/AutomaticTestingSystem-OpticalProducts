using System;
using System.Linq;
using AutomaticTestingSystem.UserControls.InputDialog;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Model;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Interfaces;
using AutomaticTestingSystem.Framework.Common;
using System.Collections.ObjectModel;
using AutomaticTestingSystem.UserControls.ProductModelManage;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class ProcessItemsController : Controller, IModelService<ProcessItemsModel>, IData
    {

        public RelayCommand Create { get; }
        public RelayCommand Modify { get; }
        public RelayCommand Delete { get; }

        //public RelayCommand SelectiondChanged { get; set; } = new RelayCommand(s => { MessageBox.Show("Not define any code in this event"); });


        public ProcessItemsController(PropertyChangedModel model) : base(model)
        {

            foreach (ProcessListModel plm in this.AccessData<ProcessListModel>( new int[0]).Data) 
            {
                this.Model().Items.Add(plm);
            }           
            Create = new RelayCommand(new Action<object>(ItemCreate));
            Modify = new RelayCommand(new Action<object>(ItemModify));
            Delete = new RelayCommand(new Action<object>(ItemDelete));
        }
        #region Popup Menu Option
        private async void ItemCreate(object obj)
        {
            var dialog3 = new InputDialog3();
            dialog3.Model().WarningMsg1 = "Input not available!";
            dialog3.Model().Label3 = "Product Type";
            dialog3.Model().ItemsSource = new ObservableCollection<ProductModel>(SystemSettings.ProductModels);
            dialog3.Model().Input1Validation = (
            s =>
            {
                return !this.Model().Items.Any(t => t.Name.ToUpper() == s.ToUpper());
            });
            var result = await DialogHost.Show(dialog3, "RootDialog", new DialogClosingEventHandler((s, t) => { }));
            //判断输入是否确认，并添加新增项至列表
            if ((bool)result)
            {
                var item = new ProcessListModel() {
                    GUID = Guid.NewGuid().ToString("N").ToUpper(),
                    Creator = SystemSettings.Operator.Name,
                    CreateTime = DateTime.Now.ToString(),
                    Name = dialog3.Model().Input1,
                    Description = dialog3.Model().Input2,
                    ProductGuid= ((ProductModel)dialog3.Model().Input3).Guid
                };
                var res = this.SaveData(item, null);
                if (res.Status)
                    this.Model().AddProcessItem(item);
                else
                    await this.MsgBox(res.Message);
            }
            this.LoadTableDefine(((ProductModel)dialog3.Model().Input3).Guid);
        }
        private async void ItemModify(object obj)
        {
            var dialog3 = new InputDialog3();
            dialog3.Model().WarningMsg1 = "Input not available!";
            dialog3.Model().Input1 = this.Model().SelectedItem.Name;
            dialog3.Model().Input2 = this.Model().SelectedItem.Description;
            dialog3.Model().Input3 = SystemSettings.ProductModels.First(t => t.Guid == this.Model().SelectedItem.ProductGuid);
            dialog3.Model().Label3 = "Product Type";
            dialog3.Model().ItemsSource = new ObservableCollection<ProductModel>(SystemSettings.ProductModels);
            dialog3.Model().Input1Validation = (
                s=>
                {
                    if (s == this.Model().SelectedItem.Name) return true;
                    return !this.Model().Items.Any(t => t.Name.ToUpper() == s.ToUpper());
                });

            var result = await DialogHost.Show(dialog3, "RootDialog", new DialogClosingEventHandler((s, t) => { }));

            if((bool)result)
            {
                var name = dialog3.Model().Input1;
                var description = dialog3.Model().Input2;
                this.Model().SelectedItem.Name = dialog3.Model().Input1;
                this.Model().SelectedItem.Description = dialog3.Model().Input2;
                this.Model().SelectedItem.ProductGuid = ((ProductModel)dialog3.Model().Input3).Guid;
                var res = this.UpdateData<ProcessListModel>(new[] { 1, 2 ,5}, new[] { name, description },
                    new[] { new ConditionExperssion<ProcessListModel>().Eq(0, this.Model().SelectedItem.GUID) });
                if (res.Status)
                {
                    this.Model().UpdateSelectedItem(name, description);
                    this.LoadTableDefine(this.Model().SelectedItem.ProductGuid);
                }
                else
                    await this.MsgBox(res.Message);
            }
        }
        private async void ItemDelete(object obj)
        {
            var message = "All information related to the selected item will be permanently deleted.\r\n" +
                                          "Please click 'DELETE' to confirm.";
            var result = await this.MsgBox(message, "DELETE", "CANCEL");
            
            if((bool)result)
            {
                var conditions = new ConditionExperssion<ProcessListModel>[] {

                    new ConditionExperssion<ProcessListModel>().Eq(0, this.Model().SelectedItem.GUID)

                };
                if (this.DeleteData(conditions).Status)  
                    this.Model().RemoveSelectedItem();
                else
                {
                    //添加错误信息

                }

            }
        }
        #endregion

    }
}
