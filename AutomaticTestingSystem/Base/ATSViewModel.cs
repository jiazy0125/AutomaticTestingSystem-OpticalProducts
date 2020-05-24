using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticTestingSystem.Interface;

namespace AutomaticTestingSystem.Base
{
    public class ATSViewModel
    {
        private static List<ATSViewModel> AtsVMs = new List<ATSViewModel>();
        //public static IControllerService<TController> GetService<TController>(this ATSViewModel atsm) where TController : IController, new();


    }




    public static class ViewModelExtensions
    {

        public static T GetService<T>(this ATSViewModel atsvm) where T : ATSViewModel, new()
        {
            return default;
        
        }
    
    }
}
