using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Instrument;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.InstrumentDriver
{
    public class EVB1_QsfpDD :InstrumentBase
    {

        public EVB1_QsfpDD(InstrumentModel instr) : base(instr)
        { }

        /// <summary>
        /// I2C数据读取
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="offset"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public byte[] Read(byte addr, byte offset, int dataLength)
        {
            //first, write offset
            Write(addr, offset, null);

            //second, read data
                                         
            var original = new List<byte>() { 0x00, 0x00, 0x07, 0x52 }; //帧头低位
            original.Add(addr);                                         //I2c地址
            original.Add((byte)dataLength);                             //数据长度  
            original.Add((byte)original.Sum(t => t));                   //校验和
            original.Add(0x1A);                                         //帧尾
            original.Insert(0, 0x01);                                   //帧头首位

            Instrument.SendData(original.ToArray());
            Thread.Sleep(100);
            byte[] temp = Instrument.ReceiveData<byte[]>();
            if (temp[5] != 0x50)
                throw new Exception("I2C Read Failed.");
            var ret = new byte[dataLength];
            Array.Copy(temp, 7, ret, 0, dataLength);
            return ret;
        }

        /// <summary>
        /// I2C数据写入
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="offset"></param>
        /// <param name="data"></param>
        public void Write(byte addr, byte offset, byte[] data)
        {
            var tempData = new List<byte>() { offset };
            if (data != null) tempData.AddRange(data);
            var original = new List<byte>();   
            byte frameL = 0x00;                            //帧头低位
            original.Add(frameL);
            var dataAddr = (ushort)tempData.Count + 6;     //
            original.Add((byte)(dataAddr >> 8));            //数据地址高位
            original.Add((byte)(dataAddr & 0x00ff));        //数据地址低位
            original.Add(0x57);                             //
            original.Add(addr);                             //I2c地址
            original.AddRange(tempData);                    //数据
            original.Add((byte)original.Sum(t => t));       //校验和
            original.Add(0x1A);                             //帧尾
            original.Insert(0, 0x01);
            //发送数据
            Instrument.SendData(original.ToArray());
            Thread.Sleep(150);
            byte[] ret = Instrument.ReceiveData<byte[]>();
            if (ret[5] != 0x50) 
                throw new Exception("I2C Write Failed."); 
        }


    }
}
