using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.ViewModel;
using AutomaticTestingSystem.Framework.Database;
using System.Data;
using AutomaticTestingSystem.Framework.Model;

namespace AutomaticTestingSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        public MainWindow()
        {
            InitializeComponent();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
            }).ContinueWith(t =>
            {
                //note you can use the message queue from any thread, but just for the demo here we 
                //need to get the message queue from the snackbar, so need to be on the dispatcher
                MainSnackbar.MessageQueue.Enqueue("Welcome To Automatic Testing System");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue) { MenuButton = MenuToggleButton };
            SystemSettings.SnackbarMessageQueue = MainSnackbar.MessageQueue;
            Snackbar = MainSnackbar;

            //Load System tables from database
            Task.Factory.StartNew(()=> 
            {
                //load systemsettings.Dbtables
                var sql = "select name from sys.tables";

                try
                {
                    DataTable dt = DbFactory.Execute().ExecuteTable(sql, CommandType.Text, null);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            SystemSettings.DbTables.Add(new ComboBoxItemModel(dr["name"].ToString(), "123"));
                        }
                    }
                }
                catch { }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
