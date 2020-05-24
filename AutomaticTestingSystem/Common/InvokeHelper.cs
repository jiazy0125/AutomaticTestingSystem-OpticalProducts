using System;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using AutomaticTestingSystem.Interface;
using AutomaticTestingSystem.Framework.Controller;

namespace AutomaticTestingSystem.Common
{
    public static class InvokeHelper
    {
        /// <summary>
        /// 通过反射调用无返回方法
        /// </summary>
        /// <param name="obj">调用实例</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters"></param>
        public static void MethodInvoke(this IInvokeHelper instance, object obj, string methodName, params object[] parameters)
        {

            MethodInfo mdi = obj?.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            _ = mdi?.Invoke(obj, parameters);

        }
        /// <summary>
        /// 通过反射调用有返回方法
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="obj">调用实例</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">输入参数</param>
        /// <returns></returns>
        public static T MethodInvoke<T>(this IInvokeHelper instance, object obj, string methodName, params object[] parameters)
        {
            MethodInfo mdi = obj?.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            if (mdi != null)
                return (T)mdi?.Invoke(obj, parameters);
            return default;
        }
        /// <summary>
        /// 通过反射调用Func方法
        /// </summary>
        /// <typeparam name="T">Func返回类型</typeparam>
        /// <param name="obj">调用实例</param>
        /// <param name="funcName">Func方法名称</param>
        /// <param name="parameters">输入参数</param>
        /// <param name="funcType">Func形式</param>
        /// <param name="funcParameters">输入参数类型,不包含返回类型</param>
        /// <returns></returns>
        public static T FuncInvoke<T>(this IInvokeHelper instance, object obj, string funcName, object[] parameters, Type funcType, params Type[] funcParameters)
        {
            try
            {
                //获取Func属性 get 返回值
                var formatterFunc = obj?.GetType().GetProperty(funcName).GetGetMethod().Invoke(obj, null);
                //创建Func
                var func = funcType.MakeGenericType(funcParameters);
                //获取Func方法
                var method = func.GetProperty("Method").GetGetMethod().Invoke(formatterFunc, null) as MethodInfo;
                //获取Func的target
                var target = func.GetProperty("Target").GetGetMethod().Invoke(formatterFunc, null);
                return (T)method?.Invoke(target, parameters);
            }
            catch { return default; }

        }
        /// <summary>
        /// 通过反射设置目标属性值
        /// </summary>
        /// <param name="obj">调用实例</param>
        /// <param name="property">属性名称</param>
        /// <param name="value">属性值</param>
        public static void SetValue(this IInvokeHelper instance, object obj, string property, object value)
        {
            obj?.GetType().GetProperty(property)?.SetValue(obj, value, null);
        }
        /// <summary>
        /// 通过反射获取实例的属性字段值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IInvokeHelper instance, object obj, string property)
        {
            try
            {
                return (T)obj?.GetType()?.GetProperty(property)?.GetValue(obj);
            }
            catch { return default; }
        }


        /// <summary>
        /// 获得指定元素的所有子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            var childList = new List<T>();

            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }
        /// <summary>
        /// 更新页面元素绑定的数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="dp"></param>
        public static void UpdateToSource<T>(DependencyObject obj,DependencyProperty dp) where T : FrameworkElement
        {
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    //调用方法获取绑定属性并更新
                    MethodInvoke<BindingExpression>(null,child, "GetBindingExpression",dp )?.UpdateSource();
                }
                UpdateToSource<T>(child, dp);
            }
        }
        /// <summary>
        /// 获取目标的指定类型的父级元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetParentObject<T>(this IInvokeHelper instance, DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// 利用VisualTree向上查找第一个与指定类型匹配的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DependencyObject VisualUpwardSearch<T>(this IInvokeHelper instance, DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="insetance"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Transform<T>(this IInvokeHelper insetance, object obj)
        {
            return (T)obj;       
        }
        /// <summary>
        /// 默认RelayCommand，无任何操作
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static RelayCommand DefaultCommand(this IInvokeHelper instance)
        {
            return new RelayCommand(_ => { });

        }

    }

}
