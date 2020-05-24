using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutomaticTestingSystem.Framework.Common;
using Ivi.Visa;
using NationalInstruments.Visa;

namespace AutomaticTestingSystem.Framework.Communication
{
    public class NIInstrument
    {
        private static string ToStringFromPortType(PortType portType)
        {
            switch (portType)
            {
                case PortType.USB: return "USB";
                case PortType.GPIB: return "GPIB";
                case PortType.LAN: return "TCPIP";
                case PortType.None: return "";
                case PortType.RS232:
                default: return "ASRL";
            }
        }

        public static string[] FindAddresses(PortType portType)
        {
            IEnumerable<string> result = new List<string>();
            try
            {
                result = GlobalResourceManager.Find($"{ToStringFromPortType(portType)}?*INSTR");
            }
            catch (Exception ex)
            {
                if (!(ex is NativeVisaException))
                {
                    if (ex.InnerException != null) throw ex.InnerException;
                    else throw ex;
                }
            }

            return result.ToArray().Where(n => !n.Contains("//")).ToArray();
        }
    }

    #region delete1 Serial
    //class RS232PortOperator : PortOperatorBase, IPortType
    //{
    //    public int BaudRate { private set; get; }

    //    public SerialParity Parity { private set; get; }

    //    public SerialStopBitsMode StopBits { private set; get; }

    //    public int DataBits { private set; get; }

    //    public PortType PortType { get => PortType.RS232; }

    //    public SerialFlowControlModes FlowControl { set; get; } = SerialFlowControlModes.None;

    //    SerialSession serialSession;

    //    public RS232PortOperator(string address, int baudRate, SerialParity parity, SerialStopBitsMode stopBits, int dataBits) : base(new SerialSession(address), address)
    //    {
    //        if (!address.ToUpper().Contains("ASRL")) throw new ArgumentException($"该地址不含ASRL字样");
    //        BaudRate = baudRate;
    //        Parity = parity;
    //        StopBits = stopBits;
    //        if (dataBits < 5 || dataBits > 8) throw new NotSupportedException($"不支持数据位为：{dataBits.ToString()}");
    //        DataBits = dataBits;
    //        serialSession = (SerialSession)Session;
    //    }

    //    public override void Open()
    //    {
    //        base.Open();
    //        serialSession.BaudRate = BaudRate;
    //        switch (Parity)
    //        {
    //            case SerialParity.None:
    //                serialSession.Parity = SerialParity.None; break;
    //            case SerialParity.Odd:
    //                serialSession.Parity = SerialParity.Odd; break;
    //            case SerialParity.Even:
    //                serialSession.Parity = SerialParity.Even; break;
    //            case SerialParity.Mark:
    //                serialSession.Parity = SerialParity.Mark; break;
    //            case SerialParity.Space:
    //                serialSession.Parity = SerialParity.Space; break;
    //        }
    //        switch (StopBits)
    //        {
    //            case SerialStopBitsMode.One:
    //                serialSession.StopBits = SerialStopBitsMode.One; break;
    //            case SerialStopBitsMode.OneAndOneHalf:
    //                serialSession.StopBits = SerialStopBitsMode.OneAndOneHalf; break;
    //            case SerialStopBitsMode.Two:
    //                serialSession.StopBits = SerialStopBitsMode.Two; break;
    //        }
    //        serialSession.DataBits = (short)DataBits;
    //        switch (FlowControl)
    //        {
    //            case SerialFlowControlModes.None:
    //                serialSession.FlowControl = SerialFlowControlModes.None; break;
    //            case SerialFlowControlModes.XOnXOff:
    //                serialSession.FlowControl = SerialFlowControlModes.XOnXOff; break;
    //            case SerialFlowControlModes.RtsCts:
    //                serialSession.FlowControl = SerialFlowControlModes.RtsCts; break;
    //            case SerialFlowControlModes.DtrDsr:
    //                serialSession.FlowControl = SerialFlowControlModes.DtrDsr; break;
    //        }
    //    }
    //}
    #endregion 
    class USBPortOperator : PortOperatorBase, IPortType
    {
        public USBPortOperator(string address) : base(new UsbSession(address), address)
        {
            if (!address.ToUpper().Contains("USB"))
                throw new ArgumentException($"该地址不含USB字样");
        }
        public PortType PortType { get => PortType.USB; }
    }

    class GPIBPortOperator : PortOperatorBase, IPortType
    {
        public GPIBPortOperator(string address) : base(new GpibSession(address), address)
        {
            if (!address.ToUpper().Contains("GPIB"))
                throw new ArgumentException($"该地址不含GPIB字样");
        }
        public PortType PortType { get => PortType.GPIB; }
    }
    #region delete Lan
    //class LANPortOperator : PortOperatorBase, IPortType
    //{
    //    public LANPortOperator(string address) : base(new TcpipSession(address), address)
    //    {
    //        if (!address.ToUpper().Contains("TCPIP"))
    //            throw new ArgumentException($"该地址不含TCPIP字样");
    //    }
    //    public PortType PortType { get => PortType.LAN; }
    //}
    #endregion
    class PortEventArgs : EventArgs
    {
        public string Address { private set; get; }
        public bool Cancel { set; get; }
        public PortEventArgs(string address)
        {
            Address = address;
        }
    }

    abstract class PortOperatorBase : IPortOperator
    {
        public string Address { set; get; }

        public PortOperatorBase(IMessageBasedSession session)
        {
            Session = session;
        }
        public PortOperatorBase(IMessageBasedSession session, string address) : this(session)
        {
            Address = address;
        }

        public int Timeout { set; get; } = 2000;

        public event EventHandler<PortEventArgs> PortOpenning;

        public event EventHandler<PortEventArgs> PortClosing;

        protected virtual void OnPortOpenning(PortEventArgs e)
        {
            PortOpenning?.Invoke(this, e);
        }

        protected virtual void OnPortClosing(PortEventArgs e)
        {
            PortClosing?.Invoke(this, e);
        }

        public bool IsPortOpen { private set; get; } = false;

        protected IMessageBasedSession Session { private set; get; }

        public virtual void Open()
        {
            var e = new PortEventArgs(Address);
            OnPortOpenning(e);
            if (!e.Cancel)
            {
                Session.TimeoutMilliseconds = Timeout;
                IsPortOpen = true;
            }
        }

        public virtual void Close()
        {
            var e = new PortEventArgs(Address);
            OnPortClosing(e);
            if (!e.Cancel)
            {
                Session.Dispose();
                IsPortOpen = false;
            }
        }

        public virtual void Write(string command)
        {
            Session.RawIO.Write(command);
        }
        public virtual void Write(byte[] command)
        {
            Session.RawIO.Write(command);
        }
        public virtual void WriteLine(string command)
        {
            Write($"{command}\n");
        }

        public const int READ_BUFFER_COUNT = 1024;

        public virtual string Read()
        {
            return Encoding.UTF8.GetString(ReadByte());
        }
        public virtual byte[] ReadByte()
        {
            return Session.RawIO.Read();
        }

        public virtual string ReadLine()
        {
            var result = Read();
            return result.EndsWith("\n") ? result.TrimEnd(new char[] { '\n' }) : result;
        }

        public virtual void Clear()
        {
            Session.Clear();
        }
    }

    interface IPortOperator
    {
        void Open();
        void Close();
        void Write(string command);
        string Read();
        void Clear();
    }

    interface IPortType
    {
        PortType PortType { get; }
    }
}