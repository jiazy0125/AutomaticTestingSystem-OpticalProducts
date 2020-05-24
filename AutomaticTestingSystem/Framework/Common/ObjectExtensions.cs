using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.UserControls.MessageDialog;
using MaterialDesignThemes.Wpf;

namespace AutomaticTestingSystem.Framework.Common
{
    public static class ObjectExtensions
    {

        public static async Task<object> MsgBox(this object instance, string message, string btn1Name = "OK", string btn2Name = "")
        {
            var msgDialog = new MessageDialog1();
            msgDialog.Model().Message = message;

            msgDialog.Model().Btn1Label = btn1Name;

            msgDialog.Model().Btn2Label = btn2Name;
            return await DialogHost.Show(msgDialog, "RootDialog", new DialogClosingEventHandler((s, t) => { msgDialog = null; }));
        }

        /// <summary>
        /// 通过反射设置目标属性值
        /// </summary>
        /// <param name="obj">调用实例</param>
        /// <param name="property">属性名称</param>
        /// <param name="value">属性值</param>
        public static void SetValue(this object instance, object obj, string property, object value)
        {
            obj?.GetType().GetProperty(property)?.SetValue(obj, value, null);
        }
        /// <summary>
        /// 通过反射获取实例的属性字段值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object instance, object obj, string property)
        {
            try
            {
                return (T)obj?.GetType()?.GetProperty(property)?.GetValue(obj);
            }
            catch { return default; }
        }

        /// <summary>
        /// 通过反射获取实例的属性字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object instance, object obj, string property, BindingFlags bindingFlags)
        {
            try
            {
                return (T)obj?.GetType()?.GetProperty(property, bindingFlags)?.GetValue(obj);
            }
            catch { return default; }
        }

        /// <summary>
        /// 利用VisualTree向上查找第一个与指定类型匹配的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DependencyObject VisualUpwardSearch<T>(this object instance, DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }
        /// <summary>
        /// 获取目标的指定类型的父级元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetParentObject<T>(this object instance, DependencyObject obj) where T : FrameworkElement
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
        /// 通过反射调用无返回方法
        /// </summary>
        /// <param name="obj">调用实例</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters"></param>
        public static void MethodInvoke(this object instance, object obj, string methodName, params object[] parameters)
        {
            try
            {
                MethodInfo mdi = obj?.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                _ = mdi?.Invoke(obj, parameters);
            }
            catch { }

        }
        /// <summary>
        /// 通过反射调用有返回方法
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="obj">调用实例</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">输入参数</param>
        /// <returns></returns>
        public static ActionResult<T> MethodInvoke<T>(this object instance, object obj, string methodName, params object[] parameters)
        {
            MethodInfo mdi = obj?.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            try
            {
                return ActionResult<T>.SetSuccess("", (T)mdi.Invoke(obj, parameters));
            }
            catch (Exception exp)
            {
                return ActionResult<T>.SetError(exp.Message, default);
            }
        }

        public static void MethodInvoke(this object instance, Type type, string methodName, params object[] parameters)
        {

            try
            {
                type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, parameters);

            }
            catch { }

        }

        public static ActionResult<T> MethodInvoke<T>(this object instance, Type type, string methodName, params object[] parameters)
        {
            try
            {
                var obj = (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, parameters);
                return ActionResult<T>.SetSuccess("", obj);
            }
            catch (Exception exp)
            {
                return ActionResult<T>.SetError(exp.Message, default);
            }
        }
        public static string GetLocalIP(this object instance)
        {
            var name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    return ipa.ToString();
            }

            return null;
        }

        /// <summary>
        /// 16进制原码字符串转字节数组
        /// </summary>
        /// <param name="hexString">"AABBCC"或"AA BB CC"格式的字符串</param>
        /// <returns></returns>
        public static byte[] ConvertHexStringToBytes(this object instance, string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("参数长度不正确");
            }

            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }





    }
}
