using System.Xml.Serialization;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{

    public class ModuleConfigModel
    {
       
        public ModuleConfigModel() { }

        public string Page { get; set; } = "";
        public string Address { get; set; } = "";
        public string DataLength { get; set; } = "0";

    }
}
