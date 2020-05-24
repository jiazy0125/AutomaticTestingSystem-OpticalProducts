using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Communication
{
    public class CommunicationBase : ICommunication
    {
        public bool IsOpen { get; set; } = false;
        public virtual IConfiguration Configuration {get; set; }

        public virtual bool Close()
        {
            throw new Exception("null instance founded.");
        }

        public virtual bool Open()
        {
            throw new Exception("Open Failed.");
        }

        public virtual T ReceiveData<T>()
        {
            throw new Exception("null instance founded.");
        }

        public virtual bool SendData(object data)
        {
            throw new Exception("null instance founded."); ;
        }


        /// <summary>
        /// 创建通讯接口实例
        /// </summary>
        /// <param name="type">通讯类型</param>
        /// <returns></returns>
        public static ICommunication CreateCommunication(CommunicationType type)
        {
            switch (type)
            {
                case CommunicationType.SerialPort:
                    return new SerialPortHelper();
                case CommunicationType.TCP:
                    return new TcpIpHelper();
                case CommunicationType.UDP:
                    return new UdpHelper();
                case CommunicationType.GPIB:
                    return new GPIBHelper();
                case CommunicationType.USB:
                    return new USBHelper();
                default:
                    return null;
            }
        }
        /// <summary>
        /// 创建配置接口实例
        /// </summary>
        /// <param name="type">通讯类型</param>
        /// <returns></returns>
        public static IConfiguration CreateCfg(CommunicationType type)
        {
            switch (type)
            {
                case CommunicationType.SerialPort:
                    return new SerialPortCfgModel();
                case CommunicationType.TCP:
                    return new TcpIpCfgModel();
                case CommunicationType.UDP:
                    return new UdpCfgModel();
                case CommunicationType.GPIB:
                    return new GPIBCfgModel();
                case CommunicationType.USB:
                    return new USBCfgModel();
                default:
                    return null;
            }
        }
        /// <summary>
        /// 根据通讯类型格式化配置实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IConfiguration FormatConfiguration(CommunicationType type, object instance)
        {
            switch (type)
            {
                case CommunicationType.SerialPort:
                    return (SerialPortCfgModel)instance;
                case CommunicationType.TCP:
                    return (TcpIpCfgModel)instance;
                case CommunicationType.UDP:
                    return (UdpCfgModel)instance;
                case CommunicationType.GPIB:
                    return (GPIBCfgModel)instance;
                case CommunicationType.USB:
                    return (USBCfgModel)instance;
                default:
                    return null;
            }
        }

    }
}
