using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Interfaces;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class GroupItemsController : Controller, IModelService<GroupItemsModel>, ITreeItemsOption, IData
    {
        private string _currentProcessGuid;
        private TopItemModel parentItem = null;
        private TreeViewItem selectedItem = null;
        //选中项是展开功能使能
        private bool needExpand = false;
        //菜单使能
        private bool itemEnable = false;
        private LevelType level = LevelType.Group;

        public RelayCommand PreviewMouseRightButtonDown { get; }
        public RelayCommand SetSelectedItem { get; }
        public RelayCommand ProcessPreviewMouseLeftDown { get; }

        public GroupItemsController(PropertyChangedModel model) : base(model)
        {
            ContextMenuCreate();
            ProcessPreviewMouseLeftDown = new RelayCommand(ProcessPreviewMLBtnDownAsync);
            PreviewMouseRightButtonDown = new RelayCommand(PreviewMRButtonDown);
            SetSelectedItem = new RelayCommand(s =>
            {
                this.Model().SelectedItem = s;
                this.Model().CurrentItemLevel = this.GetValue<LevelType>(s, "Level");
            });
        }

        /// <summary>
        /// Process List鼠标左键按下事件处理函数
        /// </summary>
        /// <param name="obj"></param>
        private async void ProcessPreviewMLBtnDownAsync(object obj)
        {
            var args = (MouseButtonEventArgs)((RoutedEventArgs)obj).OriginalSource;
            if (this.VisualUpwardSearch<ListBoxItem>((DependencyObject)args.OriginalSource) is ListBoxItem listBoxItem)
            {
                var _isChanged = this.Model().TopItems.Any(t => t.IsChanged == true | t.SubItems.Any(s => s.IsChanged == true));

                if (_isChanged)
                {
                    var msg = "The current process has been changed.\n" +
                                "Click ‘Abandon’ to abandon saving.\n" +
                                "Click ‘Save’ to save data";
                    var res = await this.MsgBox(msg, "Save", "Abandon");
                    if ((bool)res)
                    {
                        args.Handled = true;
                        return;
                    }
                }
                listBoxItem.IsSelected = true;
                args.Handled = true;
                _currentProcessGuid = ((ProcessListModel)listBoxItem.DataContext).GUID;
                //LoadPagesDef();
                //加载模块表详细信息
                this.LoadTableDefine(((ProcessListModel)listBoxItem.DataContext).ProductGuid);
                this.LoadAllItems(_currentProcessGuid, this.Model().TopItems);

            }
                
        }
        /// <summary>
        /// TreeView 鼠标右键按下事件处理函数
        /// </summary>
        /// <param name="obj"></param>
        private void PreviewMRButtonDown(object obj)
        {
            var args = (MouseButtonEventArgs)obj;
            var source = this.Model().TopItems;
            var tv = (TreeView)args.Source;
            //this.Model().SelectedItem = null;
            this.Model().CurrentItemLevel = LevelType.Group;
            needExpand = false;
            itemEnable = false;
            selectedItem = null;
            parentItem = null;
            if (this.VisualUpwardSearch<TreeViewItem>((DependencyObject)args.OriginalSource) is TreeViewItem treeViewItem)
            {

                itemEnable = true;
                //选中菜单
                if (!treeViewItem.IsSelected)
                    treeViewItem.Focus();
                args.Handled = true;
                this.Model().SelectedItem = treeViewItem.DataContext;
                selectedItem = treeViewItem;

                level = this.GetValue<LevelType>(selectedItem.DataContext, "Level");
                this.Model().CurrentItemLevel = level;

                if (level == LevelType.Group)
                {
                    needExpand = true;
                }
                if (level == LevelType.Item)
                {
                    needExpand = false;
                    parentItem = (TopItemModel)this.GetParentObject<TreeViewItem>(treeViewItem).DataContext;
                }

            }
            #region ContextMenu Register
            //注册添加事件,添加Item,菜单位置index=0
            this.Model().ContextMenu[0].Register(new RelayCommand(async _ =>
            {
                var newTopItem = new TopItemModel(Guid.NewGuid().ToString("N").ToUpper()) { Parent = _currentProcessGuid };
                if (selectedItem == null)
                {
                    var res = this.SaveData(newTopItem, null);
                    if (!res.Status)
                        await this.MsgBox(res.Message);
                    else this.AddItem(source, newTopItem);
                }
                if (selectedItem?.DataContext is TopItemModel tim)
                {
                    var subitem = new SubItemModel(Guid.NewGuid().ToString("N").ToUpper()) { Parent = tim.Guid };
                    var res = this.SaveData(subitem, null);
                    if (!res.Status)
                        await this.MsgBox(res.Message);
                    else this.AddItem(tim.SubItems, subitem);
                }

                if (selectedItem?.DataContext is SubItemModel)
                {
                    var subitem = new SubItemModel(Guid.NewGuid().ToString("N").ToUpper()) { Parent = parentItem.Guid };
                    var res = this.SaveData(subitem, null);
                    if (!res.Status)
                        await this.MsgBox(res.Message);
                    else this.AddItem(parentItem.SubItems, subitem);
                }

                if (needExpand)
                    try
                    {
                        selectedItem.IsExpanded = true;
                    }
                    catch { }
            }), "Click", true);
            //注册在上方插入事件,菜单位置index=1
            this.Model().ContextMenu[1].Register(new RelayCommand(InsertItemAsync), "Click", itemEnable, Position.Above);
            //注册在下方插入事件,菜单位置index=2
            this.Model().ContextMenu[2].Register(new RelayCommand(InsertItemAsync), "Click", itemEnable, Position.Below);
            //注册Delete事件,删除选中项,菜单位置index=3
            this.Model().ContextMenu[3].Register(new RelayCommand(async _ =>
            {
                string message= "All information related to the selected item will be permanently deleted.\r\n" +
                                              "Please click 'DELETE' to confirm.";
                var result = await this.MsgBox(message, "DELETE", "CANCEL");
                if ((bool)result)
                {
                    if (selectedItem?.DataContext is TopItemModel tim)
                    {

                        var res = this.DeleteData(new[] { new ConditionExperssion<TopItemModel>().Eq(0, tim.Guid) });
                        if (res.Status)
                            this.ReMoveItem(source, tim);
                        //UpdateTopItemSequence(start);
                        else await this.MsgBox(res.Message);
                    }
                    else if (selectedItem?.DataContext is SubItemModel sim)
                    {
                        //var start = this.ReMoveItem(parentItem.SubItems, sim);
                        var res = this.DeleteData(new[] { new ConditionExperssion<SubItemModel>().Eq(0, sim.Guid) });
                        if (res.Status)
                            this.ReMoveItem(parentItem.SubItems, sim);
                        //UpdateSubItemSequence(start);
                        else await this.MsgBox(res.Message);
                    }
                }
            }), "Click", itemEnable);
            //注册Move Up事件,向上移动选中项,菜单位置index=5
            this.Model().ContextMenu[5].Register(new RelayCommand(_ =>
            {
                if (selectedItem?.DataContext is TopItemModel tim)
                {
                    var start = this.MoveUp(source, tim);
                    //if (start > 0)
                    //    UpdateTopItemSequence(start, start + 1);

                }
                else if (selectedItem?.DataContext is SubItemModel sim)
                {
                    var start = this.MoveUp(parentItem.SubItems, sim);
                    //if (start > 0)
                    //    UpdateSubItemSequence(start, start + 1);
                }
            }), "Click", itemEnable);
            //注册Move Down事件,向下移动选中项,菜单位置index=4
            this.Model().ContextMenu[6].Register(new RelayCommand(_ =>
            {
                if (selectedItem?.DataContext is TopItemModel tim)
                {
                    var start = this.MoveDown(source, tim);
                    //if (start > 0)
                    //    UpdateTopItemSequence(start - 1, start);
                }
                else if (selectedItem?.DataContext is SubItemModel sim)
                {
                    var start = this.MoveDown(parentItem.SubItems, sim);
                    //if (start > 0)
                    //    UpdateSubItemSequence(start - 1, start);
                }
            }), "Click", itemEnable);
            //注册Expand事件,菜单位置index=8
            this.Model().ContextMenu[8].Register(new RelayCommand(_ =>
            {
                if (selectedItem?.DataContext is TopItemModel)
                    selectedItem.IsExpanded = true;
            }), "Click", selectedItem?.Items.Count > 0 && itemEnable);
            //注册ExpandAll事件,菜单位置index=9
            this.Model().ContextMenu[9].Register(new RelayCommand(_ =>
            {
                foreach (var item in tv.Items)
                {
                    var tvi = (TreeViewItem)tv.ItemContainerGenerator.ContainerFromItem(item);
                    if (tvi.Items.Count > 0)
                        tvi.IsExpanded = true;
                }
            }), "Click", tv.Items.Count > 0);
            //注册Collapse事件.菜单位置index=10
            this.Model().ContextMenu[10].Register(new RelayCommand(_ =>
            {

                this.SetValue(selectedItem, "IsExpanded", false);

            }), "Click", this.GetValue<int>(selectedItem?.ItemsSource, "Count") > 0 && selectedItem?.IsExpanded == true && itemEnable);
            //注册CollapseAll事件,菜单位置index=11
            this.Model().ContextMenu[11].Register(new RelayCommand(_ =>
            {
                foreach (var item in tv.Items)
                {
                    var tvi = (TreeViewItem)tv.ItemContainerGenerator.ContainerFromItem(item);
                    tvi.IsExpanded = false;
                }
            }), "Click", tv.Items.Count > 0);
            //注册SaveChangedItem事件,菜单位置index=13
            this.Model().ContextMenu[13].Register(new RelayCommand(SaveItem), "Click", this.GetValue<bool>(selectedItem?.DataContext, "IsChanged") == true & itemEnable);
            //注册SaveAllChangedItems事件,菜单位置index=14
            this.Model().ContextMenu[14].Register(new RelayCommand(async _ =>
            {
                foreach (TopItemModel item in tv.Items)
                {
                    if (item.IsChanged)
                    {
                        var res = this.UpdateData(item.ChangedIndexs, item, new[] { new ConditionExperssion<TopItemModel>().Eq(0, item.Guid) });
                        if (!res.Status)
                        {
                            await this.MsgBox(res.Message);
                            return;
                        }
                        else
                        {
                            item.CacheOldData(item);
                            item.ReInitChangesFlags();
                            item.IsChanged = false;
                        }

                    }                    
                    foreach (var item1 in item.SubItems)
                    {
                        if (item1.IsChanged)
                        {
                           var res= this.UpdateData(item1.ChangedIndexs, item1, new[] { new ConditionExperssion<SubItemModel>().Eq(0, item1.Guid) });
                            if (!res.Status)
                            {
                                await this.MsgBox(res.Message);
                                return;
                            }
                            else
                            {
                                item1.CacheOldData(item1);
                                item1.ReInitChangesFlags();
                                item1.IsChanged = false;
                            }
                        }
                    }
                }
            }), "Click", itemEnable);
            #endregion
        }
        private void ContextMenuCreate()
        {

            //新增项0
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.New, "New . . .", PackIconKind.Add));
            //1
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.InsertAbove, "Insert Above", PackIconKind.ArrowTopRightThick));
            //2
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.InsertBelow, "Insert Below", PackIconKind.ArrowDownRightThick));
            //删除项3
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Delete, "Delete . . .", PackIconKind.Delete));
            //分隔符4
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Separator, "", default, true));
            //向上移动项5
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.MoveUp, "Move Up", PackIconKind.ArrowTopBold));
            //向下移动项6
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.MoveDown, "Move Down", PackIconKind.ArrowBottomBold));
            //分隔符7
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Separator, "", default, true));
            //展开项8
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Expand, "Expand . . .", PackIconKind.ArrowExpandVertical));
            //展开所有项9
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.ExpandAll, "Expand All", PackIconKind.AnimationMinus));
            //折叠项10
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Collapse, "Collapse . . .", PackIconKind.ArrowCollapseVertical));
            //折叠所有项11
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.CollapseAll, "Collapse All", PackIconKind.AnimationPlus));
            //分隔符12
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.Separator, "", default, true));
            //更新已更改项目13
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.SaveChangedItem, "Save . . .", PackIconKind.ContentSaveOutline));
            //更新所有已更改项目14
            this.Model().ContextMenu.Add(new ContextMenuModel(MenuKey.SaveAllChangedItems, "Save All . . .", PackIconKind.ContentSaveSettings));
        }
        private async void InsertItemAsync(object arg)
        {
            if (selectedItem?.DataContext is TopItemModel tim)
            {
                var item = new TopItemModel(Guid.NewGuid().ToString("N").ToUpper()) { Parent = _currentProcessGuid };
                var index = this.Insert(this.Model().TopItems, tim, item, (Position)arg);
                var res = this.SaveData(item, null);
                if (res.Status)
                    UpdateTopItemSequence(index);
                else await this.MsgBox(res.Message);
            }

            if (selectedItem?.DataContext is SubItemModel sim)
            {
                var item = new SubItemModel(Guid.NewGuid().ToString("N").ToUpper()) { Parent = parentItem.Guid };
                var index = this.Insert(parentItem.SubItems, sim, item, (Position)arg);
                var res = this.SaveData(item, null);
                if (res.Status)
                    UpdateSubItemSequence(index);
                else await this.MsgBox(res.Message);
            }
            if (needExpand)
                try
                {
                    selectedItem.IsExpanded = true;
                }
                catch { }
        }
        private async void UpdateTopItemSequence(int start, int end = -1)
        {
            for (var i = start; i < (end < 0 ? this.Model().TopItems.Count : end + 1); i++)
            {
                var res = this.UpdateData(new[] { 3 }, this.Model().TopItems[i],
                    new[] { new ConditionExperssion<TopItemModel>().Eq(0, this.Model().TopItems[i].Guid) });
                if (!res.Status)
                    await this.MsgBox(res.Message);
            }

        }
        private async void UpdateSubItemSequence(int start, int end = -1)
        {
            for (var i = start; i < (end < 0 ? parentItem.SubItems.Count : end + 1); i++)
            {
                var res = this.UpdateData(new[] { 3 }, parentItem.SubItems[i],
                    new[] { new ConditionExperssion<TopItemModel>().Eq(0, parentItem.SubItems[i].Guid) });
                if (!res.Status)
                    await this.MsgBox(res.Message);
            }
        }
        private async void SaveItem(object arg)
        {
            if (selectedItem?.DataContext is TopItemModel tim)
            {
                var res = this.UpdateData(tim.ChangedIndexs, tim,
                            new[] { new ConditionExperssion<TopItemModel>().Eq(0, tim.Guid) });
                if (!res.Status)
                    await this.MsgBox(res.Message);
                else
                {
                    tim.CacheOldData(tim);
                    tim.ReInitChangesFlags();
                    tim.IsChanged = false;
                }
            }
            if (selectedItem?.DataContext is SubItemModel sim)
            {
                var res = this.UpdateData(sim.ChangedIndexs, sim,
                            new[] { new ConditionExperssion<SubItemModel>().Eq(0, sim.Guid) });
                if (!res.Status)
                    await this.MsgBox(res.Message);
                else
                {
                    sim.CacheOldData(sim);
                    sim.ReInitChangesFlags();
                    sim.IsChanged = false;
                }
            }
        }

    }
}
