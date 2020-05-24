using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Reflection;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.UserControls.Home;
using AutomaticTestingSystem.UserControls.ProcessDesign;
using AutomaticTestingSystem.UserControls.ProductModelManage;
using AutomaticTestingSystem.UserControls.Settings;
using AutomaticTestingSystem.UserControls.Test;
using MaterialDesignThemes.Wpf;

namespace AutomaticTestingSystem
{
    public static class SystemSettings
    {

        /// <summary>
        /// 输入函数列表
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> ProcFunctions { get; } = new ObservableCollection<ComboBoxItemModel>();

        /// <summary>
        /// 输出函数列表
        /// </summary>
        public static ObservableCollection<string> TargetFunctions { get; } = new ObservableCollection<string>() { "Default" };
        /// <summary>
        /// 数据库所有表集合
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> DbTables { get; } = new ObservableCollection<ComboBoxItemModel>();
        /// <summary>
        /// 数据库表字段集合
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> Columns { get; } = new ObservableCollection<ComboBoxItemModel>();
        /// <summary>
        /// 模块表集合
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> MdlTables { get; } = new ObservableCollection<ComboBoxItemModel>();
        /// <summary>
        /// 变量表集合
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> Variants { get; } = new ObservableCollection<ComboBoxItemModel>();
        /// <summary>
        /// 设备仪器列表
        /// </summary>

        public static ObservableCollection<InstrumentModel> InstrumentsList { get; } = new ObservableCollection<InstrumentModel>();
        /// <summary>
        /// 产品模型集合
        /// </summary>
        public static ObservableCollection<ProductModel> ProductModels { get;} = new ObservableCollection<ProductModel>();

        /// <summary>
        /// 产品模型\类型信息列表
        /// </summary>
        public static ObservableCollection<TableDefineModel> TablesDetails { get; } =  new ObservableCollection<TableDefineModel>();

        /// <summary>
        /// TestPlan集合
        /// </summary>
        public static ObservableCollection<ProcessListModel> ProcessItems { get; } = new ObservableCollection<ProcessListModel>();
        public static ObservableCollection<OperatorComboBoxItemModel> LowerOperators { get; } = new ObservableCollection<OperatorComboBoxItemModel>()
        {
            new OperatorComboBoxItemModel(OperatorEnum.MoreThan,"<"),
            new OperatorComboBoxItemModel(OperatorEnum.MoreThanAndEqual,"≤"),
            new OperatorComboBoxItemModel(OperatorEnum.None,"\\"),
        };
        public static ObservableCollection<OperatorComboBoxItemModel> UpperOperators { get; } = new ObservableCollection<OperatorComboBoxItemModel>()
        {
            new OperatorComboBoxItemModel(OperatorEnum.Equal,"="),
            new OperatorComboBoxItemModel(OperatorEnum.LessThan,"<"),
            new OperatorComboBoxItemModel(OperatorEnum.LessThanAndEqual,"≤"),
            new OperatorComboBoxItemModel(OperatorEnum.None,"\\"),
        };

        public static ObservableCollection<TerminationComboBoxItemModel> Termiantions { get; } = new ObservableCollection<TerminationComboBoxItemModel>()
{
            new TerminationComboBoxItemModel(TerminationCharacter.None,"NA"),
            new TerminationComboBoxItemModel(TerminationCharacter.LineFeed,"Line Feed-\\n"),
            new TerminationComboBoxItemModel(TerminationCharacter.CarriageReturn,"Carriage Return-\\r"),
            new TerminationComboBoxItemModel(TerminationCharacter.LF_CR,"All-\\r\\n"),
        };

        public static Collection<ExecType> ReadWrite { get; } = new Collection<ExecType>()
        {
            ExecType.Read,
            ExecType.Write
        };

       // public static ObservableCollection<CommunicationType> Communication { get; } = new ObservableCollection<CommunicationType>();

        /// <summary>
        /// 通讯类型集合
        /// </summary>
        public static ObservableCollection<ComboBoxItemModel> Comm1 { get; } = new ObservableCollection<ComboBoxItemModel>();

        /// <summary>
        /// 输入源集合
        /// </summary>
        public static Collection<OptionType> OptionTypes { get; } = new Collection<OptionType>();
        public static Collection<int> BaudRate { get; } = new Collection<int>()
        {
            300,1200,2400,4800,9600,19200,38400,115200,1000000
        };

        public static Collection<int> DataBits { get; } = new Collection<int>()
        {
            5,6,7,8
        };

        public static ObservableCollection<Parity> Parities { get; } = new ObservableCollection<Parity>();

        public static Collection<StopBits> StopBit { get; } = new Collection<StopBits>();



        public static UserModel Operator { get; set; } = new UserModel() { Name = "Guest" };

        public static string ID { get; set; }

        public static string Station { get; set; }

        public static string WorkOrder { get; set; }
        /// <summary>
        /// 测试SN集合
        /// </summary>
        public static ObservableCollection<SerialNumberModel> SerialNumbers { get; } = new ObservableCollection<SerialNumberModel>();
        /// <summary>
        /// 测试项集合
        /// </summary>
        public static ObservableCollection<TopItemModel> GroupItems { get; } = new ObservableCollection<TopItemModel>();
        /// <summary>
        /// 系统提示条
        /// </summary>
        public static ISnackbarMessageQueue SnackbarMessageQueue { get; set; }
        /// <summary>
        /// 函数调用过程入口点
        /// </summary>
        public static Type Procedure { get; } = Assembly.Load("AutomaticTestingSystem").GetType("AutomaticTestingSystem.Procedure.ProcedureFunctionsCollection");

        /// <summary>
        /// 中间变量存储字典
        /// </summary>
        public static Dictionary<string, object> VariantDic { get; } = new Dictionary<string, object>();

        /// <summary>
        /// 系统根目录
        /// </summary>
        public static string Root { get; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 仪器配置文件路径
        /// </summary>
        public static string InstrumentIniFile { get; } = $@"{Root}Instrument.ini";

        public static string SystemIniFile { get; } = $@"{Root}System.ini";


    }
}
