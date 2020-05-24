using System.Configuration;
using System.Reflection;

namespace AutomaticTestingSystem.Framework.Database
{
    public class DbFactory
    {
        public static IDBHelper Execute()
        {
            var className = ConfigurationManager.AppSettings["DBHelperName"];
            var fullName = "AutomaticTestingSystem.Framework.Database." + className;
            var result = (IDBHelper)Assembly.Load("AutomaticTestingSystem").CreateInstance(fullName);
            return result;
        }
    }
}
