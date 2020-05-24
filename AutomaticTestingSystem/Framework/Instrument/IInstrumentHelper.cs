using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Instrument
{
    public interface IInstrumentHelper
    {

        bool Open();

        bool Close();
        /// <summary>
        /// 调用仪器方法,有返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">方法名称</param>
        /// <param name="data">所需传递的数据</param>
        /// <param name="slot">设备槽口号</param>
        /// <param name="channel">设备通道号</param>
        /// <returns></returns>
        T InvokeProc<T>(string method, object data, string slot, string channel);
        /// <summary>
        /// 调用仪器方法,有返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">方法名称</param>
        /// <returns></returns>
        T InvokeProc<T>(string method);
        /// <summary>
        /// 调用仪器方法,有返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">方法名称</param>
        /// <param name="data">所需传递的数据</param>
        /// <param name="channel">设备通道号</param>
        /// <returns></returns>
        T InvokeProc<T>(string method, object data, string channel);
        /// <summary>
        /// 调用仪器方法,有返回参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">方法名称</param>
        /// <param name="data">所需传递的数据</param>
        /// <returns></returns>
        T InvokeProc<T>(string method, object data);
        /// <summary>
        /// 调用仪器方法,无返回参数
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="data">执行参数</param>
        /// <param name="slot">设备槽口号</param>
        /// <param name="channel">设备通道号</param>
        void InvokeProc(string method, object data, string slot, string channel);
        /// <summary>
        /// 调用仪器方法,无返回参数
        /// </summary>
        /// <param name="method">方法名称</param>
        void InvokeProc(string method);

        /// <summary>
        /// 调用仪器方法,无返回参数
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="data">执行参数</param>
        /// <param name="channel">设备通道号</param>
        void InvokeProc(string method, object data, string channel);
        /// <summary>
        /// 调用仪器方法,无返回参数
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="data">执行参数</param>
        void InvokeProc(string method, object data);
    }


}
