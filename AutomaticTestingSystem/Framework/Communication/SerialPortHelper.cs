using System;
using System.IO.Ports;
using System.Text;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Communication
{

    class SerialPortHelper : CommunicationBase
    {

        private SerialPort _sp;
        private object _data1;//string 格式
        private object _data2;//byte[] 格式

        public SerialPortHelper()
        {
            _sp = new SerialPort();
        }

        public override bool Close()
        {
            try
            {
                if (_sp.IsOpen) _sp.Close();
            }
            catch (Exception exp)
            {
               throw exp;
            }
            return true;
        }

        public override IConfiguration Configuration
        {
            get
            {
                return base.Configuration;
            }

            set
            {

                base.Configuration = value;
                if (!_sp.IsOpen)
                {
                    var config = (SerialPortCfgModel)value;
                    _sp.PortName = config.PortName;
                    _sp.BaudRate = config.BaudRate;
                    _sp.DataBits = config.DataBits;
                    _sp.StopBits = config.StopBits;
                    _sp.Parity = config.Parity;
                    
                }
            }
        }

        public override bool Open()
        {
            if (Configuration == null) throw new Exception("Configuration is not correctlly.");           
            //if (IsAsync)
            //{
            //    _sp.DataReceived += _sp_DataReceived;
            //    _sp.ErrorReceived += _sp_ErrorReceived;
            //}

            try
            {
                if (!_sp.IsOpen)
                {
                    _sp.Open();
                }

            }
            catch(Exception exp)
            {
                throw exp;
            }
            return true;
        }

        public override T ReceiveData<T>()
        {
            object ret = default(T);
            if (typeof(T) != typeof(string) && typeof(T) != typeof(byte[]))
                throw new Exception("Only 'string' and 'byte[]' data type can be used.");
            try
            {
                var byteData = new byte[_sp.BytesToRead]; //定义缓冲区大小 
                _sp.Read(byteData, 0, byteData.Length); //从串口读取数据 
                //返回byte[]类型数据
                if (typeof(T) == typeof(byte[]))
                    ret = byteData;
                //返回string类型数据
                if (typeof(T) == typeof(string))
                    ret = Encoding.UTF8.GetString(byteData, 0, byteData.Length);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return (T)ret;
        }

        public override bool SendData(object data)
        {

            if (!_sp.IsOpen) Open();

            try
            {
                _sp.Write((byte[])data, 0, ((byte[])data).Length);
            }
            catch
            {
                try
                {
                    _sp.Write((string)data);
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
            return true;

        }


    }
}
