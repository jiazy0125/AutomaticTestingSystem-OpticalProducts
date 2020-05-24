using System.ComponentModel;
using System.Linq;
using System.Windows;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Instrument;
using AutomaticTestingSystem.InstrumentDriver;
using AutomaticTestingSystem.UserControls.ProcessDesign;
using AutomaticTestingSystem.UserControls.Settings;
using Newtonsoft.Json;

namespace AutomaticTestingSystem.Procedure
{
    public partial class ProcedureFunctionsCollection
    {
        //范例代码,输入参数不可变 即为SubItemModel 类型
        [Description("This an example.Please do not select me.")]
        public static object Example(SubItemModel item)
        {
            try
            {
                //获取执行该操作的输入数据
                //如不需要输入则可屏蔽该段代码

                //----------代码区
                string data;
                item.GetSourceData(out data);
                //----------代码区

                //返回target配置信息,可返回类型有
                //InstrumentConfigModel
                //DatabaseConfigModel
                //ModuleConfigModel
                //FileConfigModel
                //VariantConfigModel
                //根据实际配置类型创建对应的数据类型变量

                //----------代码区
                InstrumentConfigModel config;
                item.GetTargetConfig(out config);
                //----------代码区

                //具体执行代码开始




                //具体执行代码结束
                //根据函数实际返回值返回数据
            }
            catch { }
            return null;
        }



        [Description("This method1 is a test")]
        public static object Method1(SubItemModel item)
        {
            MessageBox.Show(item.Guid); 
            return item.LowerLimit;
        }
        [Description("This method2 is a test")] 
        public static object Method2(SubItemModel item)
        {        
            return item.UpperLimit;
        }

        [Description("Set PW3256 Channel Voltage")]
        public static object SetPW3256Voltage(SubItemModel item)
        {

            string data;
            item.GetSourceData(out data);
            InstrumentConfigModel config;
            item.GetTargetConfig(out config);


            var instr = SystemSettings.InstrumentsList.First(t => t.Name == config.Name);

            IInstrumentHelper dd = new SteligentPW3256(instr);
            dd.Open();
            dd.InvokeProc("SetVoltage", data, config.Channel);
            dd.Close();

            return null;
        }
        [Description("Set PW3256 Power ON")]
        public static object SetPW3256PowerON(SubItemModel item)
        {

            InstrumentConfigModel config;
            item.GetTargetConfig(out config);

            var instr = SystemSettings.InstrumentsList.First(t => t.Name == config.Name);
            IInstrumentHelper dd = new SteligentPW3256(instr);
            dd.Open();
            dd.InvokeProc("PowerOn","1");
            dd.Close();

            return null;
        }

        [Description("Set PW3256 Power Off")]
        public static object SetPW3256PowerOff(SubItemModel item)
        {

            InstrumentConfigModel config;
            item.GetTargetConfig(out config);


            var instr = SystemSettings.InstrumentsList.First(t => t.Name == config.Name);
            IInstrumentHelper dd = new SteligentPW3256(instr);
            dd.Open();
            dd.InvokeProc("PowerOff", "1");
            dd.Close();

            return null;

        }
        [Description("PW3256 Measure Voltage")]
        public static object PW3256MeasureVoltage(SubItemModel item)
        {
            
            InstrumentConfigModel config;
            item.GetTargetConfig(out config);

            var instr = SystemSettings.InstrumentsList.First(t => t.Name == config.Name);
            IInstrumentHelper dd = new SteligentPW3256(instr);
            dd.Open();
            var ret = dd.InvokeProc<string>("MeasureVlotage", "1");
            dd.Close();

            return ret;

        }
    }
}
