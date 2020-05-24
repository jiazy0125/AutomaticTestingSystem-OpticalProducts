using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutomaticTestingSystem.Framework.Common
{
    public class TreeViewHelper
    {
        private static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors = new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        public static object GetSelectedItem(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TreeView))
                return;

            if (!behaviors.ContainsKey(obj))
                behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as TreeView));

            TreeViewSelectedItemBehavior view = behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            readonly TreeView view;
            public TreeViewSelectedItemBehavior(TreeView view)
            {
                this.view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                var item1 = (TreeViewItem)view.ItemContainerGenerator.ContainerFromItem(p);
                if (item1 != null)
                {
                    item1.IsSelected = true;
                    item1.BringIntoView();
                }
                else
                {
                    foreach (var item in view.Items)
                    {
                        var tvi = (TreeViewItem)view.ItemContainerGenerator.ContainerFromItem(item);
                        //tvi.IsExpanded = true;
                        if (tvi.Items.Count > 0)
                        {
                            foreach (var it in tvi.Items)
                            {
                                var tviChild = (TreeViewItem)tvi.ItemContainerGenerator.ContainerFromItem(it);
                                if (tviChild!=null && it == p)
                                {

                                    tviChild.IsSelected = true;
                                    return;                               
                                }                          
                            }                        
                        }                    
                    }                 
                }
            }
        }
    }
}

