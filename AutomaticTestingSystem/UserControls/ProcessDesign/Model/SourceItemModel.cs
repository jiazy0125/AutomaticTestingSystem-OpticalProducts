using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    public class SourceItemModel
    {
        public SourceItemModel(OptionType name, object content)
        {
            Name = name;
            Content = content;        
        }

        public OptionType Name { get; }
        public object Content { get; }
    }
}
