using System;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.UserControls.Settings;

namespace AutomaticTestingSystem.Framework.Instrument
{
    public class InstrumentBase : IInstrumentHelper
    {
        public InstrumentBase(InstrumentModel instr)
        {
            Instrument = instr.CommReference;
            Instrument.Configuration = CommunicationBase.FormatConfiguration(instr.CommnunicationType, instr.Config);
        }

        public ICommunication Instrument { get; private set; }



        public T InvokeProc<T>(string method, object data, string slot , string channel )
        {

            var res = this.MethodInvoke<T>(this, method, new[] { data, slot, channel });

            return res.Data;
        }

        public void InvokeProc(string method, object data, string slot, string channel)
        {
            this.MethodInvoke(this, method, new[] { data, slot, channel });
        }

        public T InvokeProc<T>(string method)
        {
            var res = this.MethodInvoke<T>(this, method);

            return res.Data;
        }

        public T InvokeProc<T>(string method, object data, string channel)
        {
            var res = this.MethodInvoke<T>(this, method, new[] { data, channel });

            return res.Data;
        }

        public T InvokeProc<T>(string method, object data)
        {
            var res = this.MethodInvoke<T>(this, method, new[] { data });

            return res.Data;
        }

        public void InvokeProc(string method)
        {
            this.MethodInvoke(this, method);
        }

        public void InvokeProc(string method, object data, string channel)
        {
            this.MethodInvoke(this, method, new[] { data, channel });
        }

        public void InvokeProc(string method, object data)
        {
            this.MethodInvoke(this, method, new[] { data });
        }

        public virtual bool Open()
        {            
            return Instrument.Open();
        }
        public bool Close()
        {
            return Instrument.Close();
        }
    }
}
