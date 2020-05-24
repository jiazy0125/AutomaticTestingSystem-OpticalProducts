using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using AutomaticTestingSystem.Common;
using AutomaticTestingSystem.Model;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.UserControls.ProcessDesign;
using AutomaticTestingSystem.UserControls.Home;
using AutomaticTestingSystem.UserControls.Settings;
using System.Windows.Input;
using AutomaticTestingSystem.Framework.Common;
using System.Windows.Controls;
using System.Reflection;
using AutomaticTestingSystem.Procedure;
using System.ComponentModel;
using AutomaticTestingSystem.Framework.Model;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.IO;
using AutomaticTestingSystem.Framework.IniFileHelper;
using Newtonsoft.Json;
using System.IO.Ports;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.UserControls.ModuleTables;
using AutomaticTestingSystem.UserControls.ProductModelManage;
using AutomaticTestingSystem.Framework.Interfaces;
using AutomaticTestingSystem.UserControls.PNManagment;

namespace AutomaticTestingSystem.ViewModel
{
    class MainWindowViewModel:IData
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));
            //登录界面
            AtsItems.Add(new AtsItem("Log In", new Home() { DataContext = new HomeViewModel(snackbarMessageQueue)}) 
            {
                VisibilityType = Visibility.Collapsed,
                Title = "Automatic Testing System"
            });
            //自动测试界面
            AtsItems.Add(new AtsItem("Auto Test", new TaskPage()) {
                VisibilityType = Visibility.Collapsed,
                Title = "Auto Test"
            });
            //测试计划管理界面
            AtsItems.Add(new AtsItem("Test Plan", new ProcessManage()) { VisibilityType = Visibility.Collapsed,
            Title="Test Plan Management"});

            //产品模型界面
            AtsItems.Add(new AtsItem("Product Model", new ProductManageView()) {
                VisibilityType = Visibility.Collapsed,
                Title = "Product Model Management"
            });
            //仪器设置界面
            AtsItems.Add(new AtsItem("PN Manage", new PartNumber()) {
                VisibilityType = Visibility.Collapsed,
                Title = "Part Number Management"
            });
            //仪器设置界面
            AtsItems.Add(new AtsItem("Settings", new Settings()){
                VisibilityType = Visibility.Collapsed,
                Title = "Instrument Settings"
            });

            //模块EEPROM读取界面
            AtsItems.Add(new AtsItem("EEPROM", new TablesView()) {
                VisibilityType = Visibility.Collapsed,
                Title = "EEPROM In Module"
            });

            PreviewMouseLeftButtonDown = new RelayCommand(PreviewMLBD);
            LoadAllEnumSource();
            CheckFiles();
            LoadInstruments();
            LoadProductModels();
        }

        public ObservableCollection<AtsItem> AtsItems { get; } = new ObservableCollection<AtsItem>();
        public RelayCommand Exit { get; }

        public RelayCommand PreviewMouseLeftButtonDown { get; }
        public ToggleButton MenuButton { get; set; }

        private void PreviewMLBD(object obj)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            MenuButton.IsChecked = false;
            var args = (MouseButtonEventArgs)obj;

            if (this.VisualUpwardSearch<ListBoxItem>((DependencyObject)args.OriginalSource) is ListBoxItem listBoxItem)
            {
                listBoxItem.IsSelected = true;
                args.Handled = true;
                SystemSettings.ProcFunctions.Clear();
                var item = (AtsItem)listBoxItem.DataContext;
                //如进入流程管理界面则加载所有自定义方法函数
                if (item.Content?.GetType() == typeof(ProcessManage))
                {
                    MethodInfo[] methods = typeof(ProcedureFunctionsCollection).GetMethods();

                    foreach (var method in methods)
                    {
                        //加载所有功能函数
                        try
                        {
                            var attr = method.GetCustomAttribute<DescriptionAttribute>();
                            if (attr != null)
                                SystemSettings.ProcFunctions.Add(new ComboBoxItemModel(method.Name, attr.Description));
                        }
                        catch
                        { }
                    }
                }

                //如进入PN管理界面则加载所有TestPlan列表
                if (item.Content?.GetType() == typeof(PartNumber))
                {
                    SystemSettings.ProcessItems.Clear();
                    foreach (ProcessListModel plm in this.AccessData<ProcessListModel>(new int[0]).Data)
                    {
                        SystemSettings.ProcessItems.Add(plm);
                    }
                }

            }

        }

        private void CheckFiles()
        {
            //检查仪器配置文件是否存在
            var filename = $@"{SystemSettings.Root}Instrument.ini";
            if (!File.Exists(filename)) File.Create(filename);
            //检查系统配置文件是否存在
            filename= $@"{SystemSettings.Root}System.ini";
            if (!File.Exists(filename)) File.Create(filename);

        }

        private void CheckDatabase()
        { 
        
        
        }
        /// <summary>
        /// 从仪器配置文件中读取所有仪器配置
        /// </summary>
        private void LoadInstruments()
        {
            //SerialPort,
            //GPIB,
            //TCP,
            //USB,
            //UDP,
            object obj = null;
            InIHelper.FileName = SystemSettings.InstrumentIniFile;
            //Json转换条件,忽略空值属性
            var jsSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            //读取所有设备字段信息-设备名称
            foreach (var item in InIHelper.ReadSections())
            {
                var cfg = InIHelper.Read(item, "Config", null);     //读取配置
                var typeStr = InIHelper.Read(item, "Type", null);   //读取通信方式
                if (Enum.IsDefined(typeof(CommunicationType), typeStr))
                {
                    var type = (CommunicationType)Enum.Parse(typeof(CommunicationType), typeStr, true);//格式化通讯方式为enum
                    //根据不同通讯方式创建不同仪器实例信息
                    switch (type)
                    {

                        case CommunicationType.UDP:
                            obj = JsonConvert.DeserializeObject<UdpCfgModel>(cfg, jsSetting);
                            break;
                        case CommunicationType.TCP:
                            obj = JsonConvert.DeserializeObject<TcpIpCfgModel>(cfg, jsSetting);
                            break;
                        case CommunicationType.SerialPort:
                            obj = JsonConvert.DeserializeObject<SerialPortCfgModel>(cfg, jsSetting);
                            break;
                        case CommunicationType.GPIB:
                            obj = JsonConvert.DeserializeObject<GPIBCfgModel>(cfg, jsSetting) ;
                            break;
                        case CommunicationType.USB:
                            obj = JsonConvert.DeserializeObject<USBCfgModel>(cfg, jsSetting) ;
                            break;
                        default:
                            break;
                    }
                    var instr = new InstrumentModel() {
                        Name = item,
                        Config = obj,
                        CommnunicationType = type,
                        CommReference = CommunicationBase.CreateCommunication(type)
                    };
                    //添加至系统全局变量中
                    SystemSettings.InstrumentsList.Add(instr);

                }
            }       
        }

        private void LoadAllEnumSource()
        {
            foreach (CommunicationType item in Enum.GetValues(typeof(CommunicationType)))
            {

                SystemSettings.Comm1.Add(new ComboBoxItemModel(item.ToString(), ""));
            }
            foreach (Parity item in Enum.GetValues(typeof(Parity)))
            {
                SystemSettings.Parities.Add(item);
            }
            foreach (StopBits item in Enum.GetValues(typeof(StopBits)))
            {
                SystemSettings.StopBit.Add(item);
            }
            foreach (OptionType item in Enum.GetValues(typeof(OptionType)))
            {
                SystemSettings.OptionTypes.Add(item);
            
            }
            //StopBits
        }

        private void LoadProductModels()
        {

            var sql = "SELECT * FROM ProductModel";
            var res = this.AccessData<ProductModel>(sql);
            SystemSettings.ProductModels.Clear();
            foreach (var item in res.Data)
            {
                SystemSettings.ProductModels.Add(item);
            }
            //SystemSettings.ProductModels = new ObservableCollection<ProductModel>(res.Data);
        }
    }
}
