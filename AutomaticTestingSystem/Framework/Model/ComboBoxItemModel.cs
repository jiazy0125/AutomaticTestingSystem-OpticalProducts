using System.ComponentModel;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Model
{
    public class ComboBoxItemModel : INotifyPropertyChanged
    {
        public ComboBoxItemModel() { }
        public ComboBoxItemModel(string name, string description)
        {
            Name = name;
            Description = description;

        }

        private string _name;
        [Column("Name", 0)]
        public string Name
        {
            get => _name;
            set => this.MutateVerbose(ref _name, value, args => PropertyChanged?.Invoke(this, args));
        }

        private string _description;
        [Column("Description", 0)]
        public string Description
        {
            get => _description;
            set => this.MutateVerbose(ref _description, value, args => PropertyChanged?.Invoke(this, args));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class OperatorComboBoxItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public OperatorComboBoxItemModel() { }
        public OperatorComboBoxItemModel(OperatorEnum op, string opString)
        {
            Operator = op;
            OperatorStr = opString;
        }

        private OperatorEnum _operator = OperatorEnum.None;
        public OperatorEnum Operator
        {
            get => _operator;
            set => this.MutateVerbose(ref _operator, value, args => PropertyChanged?.Invoke(this, args));
        }

        private string _operatorStr = "\\";
        public string OperatorStr
        {
            get => _operatorStr;
            set => this.MutateVerbose(ref _operatorStr, value, args => PropertyChanged?.Invoke(this, args));
        }
    }

    public class TerminationComboBoxItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TerminationComboBoxItemModel() { }
        public TerminationComboBoxItemModel(TerminationCharacter tc, string tcString)
        {
            TerminationChar = tc;
            TerminationCharStr = tcString;
        }

        private TerminationCharacter _tc = TerminationCharacter.None;
        public TerminationCharacter TerminationChar
        {
            get => _tc;
            set => this.MutateVerbose(ref _tc, value, args => PropertyChanged?.Invoke(this, args));
        }

        private string _terminationCharStr = "NA";
        public string TerminationCharStr
        {
            get => _terminationCharStr;
            set => this.MutateVerbose(ref _terminationCharStr, value, args => PropertyChanged?.Invoke(this, args));
        }
    }
}
