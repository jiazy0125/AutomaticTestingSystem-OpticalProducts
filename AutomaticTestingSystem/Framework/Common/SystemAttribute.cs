using System;

namespace AutomaticTestingSystem.Framework.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SystemAttribute : Attribute
    {

        public SystemAttribute(string description)
        {
            Description = description;

        }


        public string AssemblyGuid { get; set; }
        public string Description { get; }



    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute(string description)
        {
            Description = description;

        }
        public Property PropertyType { get; set; }
        public string Description { get; }

    }


    /// <summary>
    /// 定义数据库字段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]   
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, int index)
        {
            Name = name;
            Index = index;
            Flag = (ColumnFlags)(index >= 0 ? 1 << index : 0);
        }
        public string Name { get; }

        public ColumnFlags Flag { get; }

        public int Index { get; }

    
    }

    /// <summary>
    /// 定义数据库表名属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatabaseAttribute : Attribute
    {

        public DatabaseAttribute(string table)
        {
            Table = table;
        }
        public string Table { get; }
    
    }

    public enum Property
    { 
        Model,
        Controller,
    
 
    
    
    }



}
