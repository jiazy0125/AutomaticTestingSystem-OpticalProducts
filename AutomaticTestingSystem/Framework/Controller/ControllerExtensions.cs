using System.Linq;
using System.Reflection;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.MessageDialog;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Framework.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AutomaticTestingSystem.UserControls.ProcessDesign;
using System;

namespace AutomaticTestingSystem.Framework.Controller
{
    public static class ControllerExtensions
    {


        //public static async Task<object> MsgBox(this IController ctrl, string message, string btn1Name = "OK", string btn2Name = "")
        //{
        //    var msgDialog = new MessageDialog1();
        //    msgDialog.Model().Message = message;

        //    msgDialog.Model().Btn1Label = btn1Name;

        //    msgDialog.Model().Btn2Label = btn2Name;
        //    return await DialogHost.Show(msgDialog, "RootDialog", new DialogClosingEventHandler((s, t) => { msgDialog = null; }));
        //}


        public static void LoadAllItems(this IController ctrl, string process, ObservableCollection<TopItemModel> items, bool needInitialize = true)
        {
            items.Clear();
            if (process != null && process.Length > 0) 
            {
                var topItems = DataExtension.AccessData(null, null, //获取所有TopItem
                    new[] //条件语句
                    {
                        new ConditionExperssion<TopItemModel>().Eq(4, process),
                        new ConditionExperssion<TopItemModel>().OrderBy(new[]{ 3 }, Sequence.Asc)
                    });

                var subItems = DataExtension.AccessData(null, null, //获取所有SubItem
                    new[]
                    {
                        new ConditionExperssion<SubItemModel>().In(4, new SqlExpression().Select<TopItemModel>(new[]{ 0 }).Where(
                            new[] {new ConditionExperssion<TopItemModel>().Eq(4, process) })),
                        new ConditionExperssion<SubItemModel>().OrderBy(new[]{ 3 }, Sequence.Asc)
                    });
                foreach (var top in topItems.Data)
                {
                    items.Add(top);
                    if (needInitialize)
                        top.CacheOldData(top);
                }

                SystemSettings.Variants.Clear();
                foreach (var sub in subItems.Data)
                {
                    items.First(t => t.Guid == sub.Parent).SubItems.Add(sub);
                    if (sub.AsVariable)
                    {
                        SystemSettings.Variants.Add(new ComboBoxItemModel(sub.VariantName, sub.Description));
                    }
                    if (needInitialize)
                        sub.CacheOldData(sub);
                }

            }
        }

        

    }
}
