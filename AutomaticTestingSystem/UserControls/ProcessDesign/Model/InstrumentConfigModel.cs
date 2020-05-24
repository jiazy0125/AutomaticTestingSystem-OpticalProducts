using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class InstrumentConfigModel
    {
        public InstrumentConfigModel() { }

        //仪器名称
        public string Name { get; set; } = "";
        //仪器插槽
        public string Slot { get; set; } = "0";
        //仪器通道
        public string Channel { get; set; } = "0";



    }
}
