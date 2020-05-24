using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Communication
{
    public class USBHelper : CommunicationBase
    {
        private PortOperatorBase _operator;
        private object _data1;//string格式
        private object _data2;//byte[]格式

        public USBHelper() { }


        public override bool Open()
        {
              
            try
            {
                _operator = new USBPortOperator(((USBCfgModel)Configuration).UsbAddress);
                _operator.Open();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        public override bool SendData(object data)
        {
            try
            {
                //端口是否打开
                if (!_operator.IsPortOpen) Open();
                //写入数据
                try
                {
                    //尝试写入string型数据
                    _operator.Write((string)data);
                }
                catch(Exception exp)
                {
                    try
                    {
                        //尝试写入byte[]型数据
                        _operator.Write((byte[])data);
                    }
                    catch (Exception exp1)
                    {
                        throw new Exception($"{exp.Message}\r\b{exp1.Message}");
                    }
                }              
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;


        }

        public override T ReceiveData<T>()
        {
            object ret = default(T);
            try
            {
                //返回byte[]类型数据
                if (typeof(T) == typeof(byte[]))
                    ret = _operator.ReadByte();
                //返回string类型数据
                if (typeof(T) == typeof(string))
                    ret = Encoding.UTF8.GetString(_operator.ReadByte());
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return (T)ret;
        }

        public override bool Close()
        {
            try
            {
                _operator.Close();
            }
            catch{}
            return true;
        }



    }
}
