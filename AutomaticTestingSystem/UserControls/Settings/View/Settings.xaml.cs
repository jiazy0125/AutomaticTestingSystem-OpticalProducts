using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Communication;
using AutomaticTestingSystem.Framework.IniFileHelper;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.UserControls.InputDialog;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

namespace AutomaticTestingSystem.UserControls.Settings
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NewBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            //创建创建仪器对话框
            var dialog3 = new InputDialog3();
            dialog3.Model().Label1 = "Instrument Name";
            dialog3.Model().Label3 = "Communication Type";
            dialog3.Model().ItemsSource = new ObservableCollection<ComboBoxItemModel>(SystemSettings.Comm1);
            dialog3.Model().Input1 = "";
            dialog3.Model().WarningMsg1 = "Input not available!";
            dialog3.Model().Input1Validation = (
            s =>
            {
                return !SystemSettings.InstrumentsList.Any(t => t.Name.ToUpper() == s.ToUpper());
            });
            Input:
            var result = await DialogHost.Show(dialog3, "RootDialog", new DialogClosingEventHandler((s, t) => { }));
            if ((bool)result)
            {
                var typeStr = ((ComboBoxItemModel)dialog3.Model().Input3).Name;
                var type = (CommunicationType)Enum.Parse(typeof(CommunicationType), typeStr);

                //仪器通讯类型错误则返回重新选择类型
                if (type == CommunicationType.None)
                {
                    await this.MsgBox("Please select the correct communication type.");
                    goto Input;
                }

                var instr = new InstrumentModel() 
                {
                    Name = dialog3.Model().Input1,
                    Description = dialog3.Model().Input1,
                    CommnunicationType = type,
                    Config = CommunicationBase.CreateCfg(type),
                    CommReference = CommunicationBase.CreateCommunication(type)
                };
                //添加仪器至全局变量
                SystemSettings.InstrumentsList.Add(instr);
                //选中当前添加的仪器
                Instruments.SelectedItem = instr;
            }
        }

        /// <summary>
        /// 删除仪器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            //删除仪器前需选中一个仪器
            if (Instruments.SelectedItem == null)
                await this.MsgBox("Please select a instrument.");
            else
            {
                //弹出删除警告
                var message = "All information related to the selected item will be permanently deleted.\r\n" +
                                             "Please click 'DELETE' to confirm.";
                var result = await this.MsgBox(message, "DELETE", "CANCEL"); 

                if ((bool)result)
                {
                    //从全局变量中移除所删除的仪器
                    var item = (InstrumentModel)Instruments.SelectedItem;
                    //从配置文件中删除对应仪器
                    SystemSettings.InstrumentsList.Remove(item);
                    InIHelper.FileName = SystemSettings.InstrumentIniFile;
                    InIHelper.DeleteSection(item.Name);           
                }
            }
        }

        /// <summary>
        /// 保存仪器配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            //try
            //{
            //    var instr = ((InstrumentModel)Instruments.SelectedItem);

            //    var tt = new EVB1_QsfpDD(instr);

            //    tt.Open();

            //    tt.Write(0xA0, 0x7F, new[] { (byte)0x00 });

            //    var data= tt.Read(0xa0, 0x80, 128);

            //    tt.Close();


            //}
            //catch (Exception exp)
            //{ 

            //}

            try
            {
                foreach (InstrumentModel instr in Instruments.Items)
                {
                    var jSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
                    var value = JsonConvert.SerializeObject(instr.Config, jSetting);
                    InIHelper.FileName = SystemSettings.InstrumentIniFile;
                    InIHelper.Write(instr.Name, "Type", instr.CommnunicationType.ToString());
                    InIHelper.Write(instr.Name, "Config", value);
                }
                SystemSettings.SnackbarMessageQueue.Enqueue("All COnfigurations saved sucessful");
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// 测试成功时设置显示信息
        /// </summary>
        /// <param name="info"></param>
        private void SetSucess(string info)
        {
            ConnectResult.Badge = "Success";
            ConnectResult.BadgeBackground = new SolidColorBrush(Colors.Green);
            Info.Text = info;
            Info.Foreground = new SolidColorBrush(Colors.Green);
        }
        /// <summary>
        /// 测试失败时设置显示信息
        /// </summary>
        /// <param name="info"></param>
        private void SetFail( string info)
        {
            ConnectResult.Badge = "Failed";
            ConnectResult.BadgeBackground = new SolidColorBrush(Colors.Red);
            Info.Text = info;
            Info.Foreground = new SolidColorBrush(Colors.Red);
        }

        /// <summary>
        /// 测试仪器连通情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace (Command.Text))
            {
                SetFail("Please input command first.");
                return;
            }
            var instr = ((InstrumentModel)Instruments.SelectedItem);
            //try
            //{

            //    IInstrumentHelper dd = new SteligentPW3256();
            //    dd.Open(instr);

            //    dd.InvokeProc("SetVoltage", 12.0, "0", "1");

            //    dd.InvokeProc("PowerOn", null, null, "1");
            //    Thread.Sleep(500);
            //    var dc = dd.InvokeProc<string>("MeasureVlotage", null, null, "1");

            //    SetSucess(dc);

            //    dd.InvokeProc("PowerOff", null, null, "1");
            //    dd.Close();
            //}
            //catch (Exception exp)
            //{
            //    var aa = "";
            //}


            if (instr.CommReference == null)
            {
                SetFail("Instrument communication type error.");
                return;
            }
            try
            {

                instr.CommReference.Configuration = CommunicationBase.FormatConfiguration(instr.CommnunicationType, instr.Config);
                instr.CommReference.Open();
                var cmd = Command.Text;
                switch ((TerminationCharacter)TerminationChar.SelectedValue)
                {
                    case TerminationCharacter.LineFeed:
                        cmd += "\n";
                        break;
                    case TerminationCharacter.CarriageReturn:
                        cmd += "\r";
                        break;
                    case TerminationCharacter.LF_CR:
                        cmd += "\r\n";
                        break;
                    default:
                        break;

                }
                instr.CommReference.SendData(cmd);

                if ((bool)IsReadDataBtn.IsChecked)
                {
                    Thread.Sleep(Convert.ToInt32(Delay.Text));
                    var data=instr.CommReference.ReceiveData<string>();
                    SetSucess((bool)TrimBtn.IsChecked ? data?.Trim() : data);
                }
                else SetSucess("");

                instr.CommReference.Close();
            }
            catch (Exception exp)
            {
                instr.CommReference.Close();
                SetFail(exp.Message.Trim());
            }
        }
    }
}
