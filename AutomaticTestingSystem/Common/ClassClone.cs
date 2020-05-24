using System;
using System.Reflection;

namespace AutomaticTestingSystem.Common
{
    public static class ClassClone<T>
    {
        public static void Clone(T source, T target)
        {

            PropertyInfo[] properties = source.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanWrite)
                    propertyInfo.SetValue(target, propertyInfo.GetValue(source), null);                      
            }

        }
    }
}
