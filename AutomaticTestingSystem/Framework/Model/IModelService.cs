using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AutomaticTestingSystem.Framework.Controller;
using AutomaticTestingSystem.Framework.Common;
using System.Reflection;

namespace AutomaticTestingSystem.Framework.Model
{
    public interface IModelService<T> where T: PropertyChangedModel
    {

    }

    public static class IModelServiceExtensions
    {
        public static T Model<T>(this IModelService<T> imc) where T : PropertyChangedModel
        {
            if (imc is IController ctrl)
                return (T)ctrl.Model;

            if(imc is UserControl uc)
            {
                PropertyInfo[] pis = uc.DataContext.GetType().GetProperties();

                foreach (var pi in pis)
                {
                    var attr = pi.GetCustomAttribute<PropertyAttribute>();

                    if (attr.PropertyType == Property.Model)
                        return (T)pi.GetValue(uc.DataContext);

                }
                return default;

            }

            else return default;

        
        }



    }

}
