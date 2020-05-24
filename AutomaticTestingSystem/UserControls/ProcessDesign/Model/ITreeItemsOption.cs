using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public interface ITreeItem
    { }
    public interface ITreeItemsOption
    {
    }


    public static class ITreeItemsOptionExtensions
    {
        /// <summary>
        /// TreeItem中增加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static void AddItem<T>(this ITreeItemsOption instance, ObservableCollection<T> source, T item) where T : ITreeItem
        {
            item.SetValue(item, "Sequence", source.Count);
            source.Add(item);
        }
        
        /// <summary>
        /// TreeItem中插入项,并返回插入位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <param name="currentItem"></param>
        /// <param name="newitem"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int Insert<T>(this ITreeItemsOption instance, ObservableCollection<T> source, T currentItem, T newitem, Position position) where T : ITreeItem
        {

            var index = source.IndexOf(currentItem);
            var newIndex = position == Position.Above ? index : index + 1;

            source.Insert(newIndex, newitem);
            Resort(source);
            return newIndex;
        }
        /// <summary>
        /// TreeItem中删除项,返回删除项所在位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static int ReMoveItem<T>(this ITreeItemsOption instance, ObservableCollection<T> source, T item) where T : ITreeItem
        {
            var index = source.IndexOf(item);
            source.Remove(item);
            Resort(source);
            return index;
        
        }
        /// <summary>
        /// TreeItem中项下移, 返回当前菜单的新位置索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static int MoveDown<T>(this ITreeItemsOption instance, ObservableCollection<T> source, T item) where T : ITreeItem
        {

            var index = source.IndexOf(item);
            if (index < source.Count - 1)
            {
                source.Move(index, index + 1);
                Resort(source);
                return index + 1;
            }
            return -1;
        
        }
        /// <summary>
        /// TreeItem中项上移, 返回当前菜单的新位置索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <param name="item"></param>
        public static int MoveUp<T>(this ITreeItemsOption instance, ObservableCollection<T> source, T item) where T:ITreeItem
        {
            var index = source.IndexOf(item);
            if (index > 0) 
            {
                source.Move(index, index - 1);
                Resort(source);
                return index - 1;
            }
            return -1;

        }

        private static void Resort<T>(ObservableCollection<T> items)
        {
            var i = 0;
            foreach (var item in items)
            {
                item.SetValue(item, "Sequence", i);
                i++;                   
            }                  
        }

    }
}
