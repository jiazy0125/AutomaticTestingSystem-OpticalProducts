using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Instrument;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.InstrumentDriver
{
    public class SteligentPW3256: InstrumentBase
    {

        public SteligentPW3256(InstrumentModel instr) : base(instr)
        { }

        public static Dictionary<SteCommand, string> cmdDic = new Dictionary<SteCommand, string>()
        {
            { SteCommand.ChannelSelect,"INST CH"},
            { SteCommand.ChannelSelectQuery,"INST?"},
            { SteCommand.OutputOn,"OUTP ON"},
            { SteCommand.OutputOff,"OUTP OFF"},
            { SteCommand.OutputQuery,"OUTP?"},
            { SteCommand.Voltage ,"VOLT "},
            { SteCommand.MeasureVoltage,"MEAS?"},
            { SteCommand.Current,"CURR "},
            { SteCommand.MeasureCurrent,"MEAS:CURR?"},
            { SteCommand.MeasureVoltageAll,"MEAS:ALL?"},
            { SteCommand.MeasureCurrentAll,"MEAS:CURR:ALL?"},

        };
        /// <summary>
        /// 电源通道选择
        /// </summary>
        /// <param name="ch"></param>
        private void ChannelSelect(string ch)
        {
            try
            {
                //切换通道
                Instrument.SendData(FormatCmd(SteCommand.ChannelSelect, ch));

                //查询切换状态
                Instrument.SendData(FormatCmd(SteCommand.ChannelSelectQuery));

                Thread.Sleep(100);
                if (Instrument.ReceiveData<string>() is string data)
                {
                    if (data.Substring(2) != ch)
                        throw new Exception("Channel selected fail.");
                }
            }
            catch (Exception exp)
            { }

        }
        /// <summary>
        /// 格式化命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private string FormatCmd(SteCommand cmd, string parameter = null)
        {
            return $"{cmdDic[cmd]}{(parameter == null ? "" : parameter)}\r\n"; 
        }

        /// <summary>
        /// 电源电压设置
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        public void SetVoltage(object data, string channel)
        {
            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.Voltage, data.ToString()));
        }
        /// <summary>
        /// 电源电压测量
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public string MeasureVlotage(string channel)
        {

            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.MeasureVoltage));
            Thread.Sleep(100);
            var value = Instrument.ReceiveData<string>();
            return value.Trim();
        }
        /// <summary>
        /// 读取所有通道电压
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public string MeasureVlotageAllChannel()
        {
            Instrument.SendData(FormatCmd(SteCommand.MeasureVoltageAll));
            Thread.Sleep(100);
            var value = Instrument.ReceiveData<string>();
            return value.Trim();
        }
        /// <summary>
        /// 电源开启
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        public void PowerOn(string channel)
        {
            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.OutputOn));
        }
        /// <summary>
        /// 电源关闭
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        public void PowerOff(string channel)
        {
            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.OutputOff));
        }
        /// <summary>
        /// 电源电流设置
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        public void SetCurrent(object data, string channel)
        {
            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.Current, data.ToString()));
        }
        /// <summary>
        /// 电源电流测量
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public string MeasureCurrent(string channel)
        {

            ChannelSelect(channel);
            Instrument.SendData(FormatCmd(SteCommand.MeasureCurrent));
            Thread.Sleep(100);
            var value = Instrument.ReceiveData<string>();
            return value.Trim();
        }
        /// <summary>
        /// 读取所有通道电压
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slot"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public string MeasureCurrentAllChannel()
        {
            Instrument.SendData(FormatCmd(SteCommand.MeasureCurrentAll));
            Thread.Sleep(100);
            var value = Instrument.ReceiveData<string>();
            return value.Trim();
        }

    }



    public enum SteCommand 
    { 
        ChannelSelect,
        ChannelSelectQuery,
        OutputOn,
        OutputOff,
        OutputQuery,
        OutputTrackOn,
        OutputTrackOff,
        OutputTrackQuery,
        OutputSerialOn,
        OutputSerialOff,
        OutputSerialQuery,
        OutputParallelOn,
        OutputParallelOff,
        OutputParallelQuery,
        Voltage,
        Current,
        MeasureVoltage,
        MeasureVoltageAll,
        MeasureCurrent,
        MeasureCurrentAll,


    }


}
