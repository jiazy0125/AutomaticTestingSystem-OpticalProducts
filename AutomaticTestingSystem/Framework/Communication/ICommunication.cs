using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Communication
{
    public interface ICommunication
    {
        /// <summary>
        /// 打开通讯接口
        /// </summary>
        /// <returns></returns>
        bool Open();
        /// <summary>
        /// 关闭通讯接口
        /// </summary>
        /// <returns></returns>
        bool Close();

        /// <summary>
        /// 读取返回数据,T只支持string.byte[]两种格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        T ReceiveData<T>();

        /// <summary>
        /// 发送数据,只支持string.byte[]两种格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SendData(object data);
        
        IConfiguration Configuration { set; }

    }

    public static class CommunicationExtensions
    {


    }
}
