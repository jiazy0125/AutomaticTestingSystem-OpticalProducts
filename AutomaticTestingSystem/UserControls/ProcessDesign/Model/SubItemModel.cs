using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Common;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutomaticTestingSystem.UserControls.ProcessDesign
{
    [Database("SubItemInfo")]
    public class SubItemModel:PropertyChangedModel, ITreeItem
    {
        public SubItemModel()
        {
                CreateChangesFlag(this);
        }
        public SubItemModel(string guid, bool isNeedInit = true)
        {
            Guid = guid;
            Name = guid;
            if (isNeedInit)
            {
                CreateChangesFlag(this);
                CacheOldData(this);
            }
        }


        private bool _isChanged = false;
        public bool IsChanged 
        {
            get => _isChanged;
            set => NotifyPropertyChanged(ref _isChanged, value); 
        }

        #region Measurement Parameters

        private string _measValue ;
        public string MeasValue 
        {
            get => _measValue;
            set => NotifyPropertyChanged(ref _measValue, value);
        }

        private string _result = "Waiting . . .";
        public string Result
        {
            get => _result;
            set => NotifyPropertyChanged(ref _result, value);
        }

        #endregion

        #region Properties Saved in Database
        [Column("GUID", 0)]
        public string Guid { get; set; }

        private string _name = "";
        [Column("Name", 1)]
        public string Name
        {
            get => _name;
            set
            {
                NotifyPropertyChanged(ref _name, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _description = "";
        [Column("Description", 2)]
        public string Description
        {
            get => _description;
            set
            {
                NotifyPropertyChanged(ref _description, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        public LevelType Level { get; } = LevelType.Item;

        private int _sequence = 0;
        [Column("Sequence", 3)]
        public int Sequence 
        {
            get => _sequence;
            set
            {
                _sequence = value;
                IsChanged = UpdateChangeFlag(value);
            }
        }

        [Column("Parent", 4)]
        public string Parent { get; set; } = "";

        private bool _isIndependent = false;
        [Column("Independent", 5)]
        public bool Independent
        {
            get => _isIndependent;
            set
            {
                NotifyPropertyChanged(ref _isIndependent, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private bool _asVariable = false;
        [Column("AsVariable", 6)]
        public bool AsVariable
        {
            get => _asVariable;
            set
            {
                NotifyPropertyChanged(ref _asVariable, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private OperatorEnum _lowerOperator = OperatorEnum.None;
        [Column("LowerOperator",7)]
        public OperatorEnum LowerOperator
        {
            get => _lowerOperator;
            set
            {
                NotifyPropertyChanged(ref _lowerOperator, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _lowerLimit = "0";
        [Column("LowerLimit",8)]
        public string LowerLimit
        {
            get => _lowerLimit;
            set
            {
                NotifyPropertyChanged(ref _lowerLimit, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
        private OperatorEnum _upperOperator = OperatorEnum.None;
        [Column("UpperOperator", 9)]
        public OperatorEnum UpperOperator
        {
            get => _upperOperator;
            set
            {
                NotifyPropertyChanged(ref _upperOperator, value);
                IsChanged = UpdateChangeFlag(value);
            }

        }
        private string _upperLimit = "0";
        [Column("UpperLimit", 10)]
        public string UpperLimit
        {
            get => _upperLimit;
            set
            {
                NotifyPropertyChanged(ref _upperLimit, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _selProcedure = "";
        [Column("SelectedProcedure", 11)]
        public string SelectedProcedure 
        {
            get => _selProcedure;
            set
            {
                NotifyPropertyChanged(ref _selProcedure, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private OptionType _optionSelected = OptionType.Default;
        [Column("OptionSelected", 12)]
        public OptionType OptionSelected 
        {
            get => _optionSelected;
            set
            {
                NotifyPropertyChanged(ref _optionSelected, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _optionParameters = "";
        [Column("OptionParameters", 13)]
        public string OptionParameters
        {
            get => _optionParameters;
            set
            {
                NotifyPropertyChanged(ref _optionParameters, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private int _delay = 0;
        [Column("DelayMS", 14)]
        public int DelayMS
        {
            get => _delay;
            set
            {
                NotifyPropertyChanged(ref _delay, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
        private string _variantName = "";
        [Column("VariantName", 15)]
        public string VariantName
        {
            get => _variantName;
            set
            {
                NotifyPropertyChanged(ref _variantName, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _sourceData = "";
        [Column("SourceData", 16)]
        public string SourceData
        {
            get => _sourceData;
            set
            {
                NotifyPropertyChanged(ref _sourceData, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private bool _customDefine = false;
        [Column("CustomDefine", 17)]
        public bool CustomDefine
        {
            get => _customDefine;
            set
            {
                NotifyPropertyChanged(ref _customDefine, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }

        private string _customData = "";
        [Column("CustomData", 18)]
        public string CustomData
        {
            get => _customData;
            set
            {
                NotifyPropertyChanged(ref _customData, value);
                IsChanged = UpdateChangeFlag(value);
            }
        }
        #endregion


        #region Method

        public bool GetSourceData<T>(out T data)
        {
            data = default;
            if (CustomDefine)
            {
                data = (T)(object)CustomData;
                return true;
            }

            if (SystemSettings.VariantDic.ContainsKey(SourceData))
            {
                data = (T)SystemSettings.VariantDic[SourceData];
                return true;
            }

            return false;


        }

        public void GetTargetConfig<T>(out T config)
        {
            config = JsonConvert.DeserializeObject<T>(OptionParameters);       
        }

        #endregion


    }
}
