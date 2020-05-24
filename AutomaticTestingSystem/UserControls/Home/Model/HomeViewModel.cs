using AutomaticTestingSystem.Common;
using AutomaticTestingSystem.Model;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using AutomaticTestingSystem.Base;
using AutomaticTestingSystem.Framework.Controller;

namespace AutomaticTestingSystem.UserControls.Home
{
    public class HomeViewModel:ATSViewModel
    {
        //private bool _successful;
        public RelayCommand SignIn { get; }
        public UserModel Home { get; } = new UserModel();


        public HomeViewModel(ISnackbarMessageQueue snackbarMessageQueue = null)
        {
            SignIn = new RelayCommand(
                (s) =>
                {
                    Home.Pwd = ((PasswordBox)s).Password;
                    var tt = SystemService.Login(Home.Name, ((PasswordBox)s).Password);

                    snackbarMessageQueue.Enqueue(tt.Message);


                    //var tt = ModelService<HomeModel>.GetModel(this);

                    


                }

                );
            

        }

    }
}
