using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Communication
{
    public class GPIBHelper : CommunicationBase
    {
        private PortOperatorBase _operator;
        private object _data1;//string格式
        private object _data2;//byte[]格式

        public GPIBHelper() { }


        public override bool Open()
        {
            if (IsOpen) return true;
            try
            {
                _operator = new GPIBPortOperator(((GPIBCfgModel)Configuration).GPIBAddress);
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
            Open();

            if (data.GetType() == typeof(string) || data.GetType() == typeof(byte[]))
            {
                try
                {
                    _operator.Write((string)data);
                }
                catch (Exception exp)
                {
                    try
                    {
                        _operator.Write((byte[])data);
                    }
                    catch (Exception exp1)
                    {
                        throw new Exception($"{exp.Message}\r\n{exp1.Message}");
                    }
                }
            }
            return true;
        }

        public override T ReceiveData<T>()
        {
            object ret = default(T);
            try
            {
                if (typeof(T) == typeof(byte[]))
                    ret = (T)(object)_operator.ReadByte();
                //返回string类型数据
                if (typeof(T) == typeof(string))
                    ret = (T)(object)Encoding.UTF8.GetString(_operator.ReadByte());
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
            catch { }
            return true;
        }

    }
}
