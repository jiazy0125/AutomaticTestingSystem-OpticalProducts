using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using AutomaticTestingSystem.Framework.Common;

namespace AutomaticTestingSystem.Framework.Database
{
    public static class SqlExpressionExtensions
    {

        /// <summary>
        /// 获取数据模型中与数据相关的字段信息 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlExpression"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static List<string> Columns<T>(this ISqlExpression sqlExpression, int[] columns)
        {

            var lt = new List<string>();
            var flags = ToFlag(null, columns);

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var attr = property.GetCustomAttribute<ColumnAttribute>();
                    if ((attr?.Flag & flags) != 0 && attr != null)
                        lt.Add(attr.Name);
                }
                catch { }
            }
            return lt;
        }

        public static string Column<T>(this ISqlExpression sqlExpression, int column)
        {

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var attr = property.GetCustomAttribute<ColumnAttribute>();
                    if ((attr?.Flag & ToFlag(null, column)) != 0 && attr != null)
                        return attr.Name;
                }
                catch { }
            }
            return null;
        }

        /// <summary>
        /// Create SQLParameter Name with'@' 
        /// </summary>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        public static string CreateParaID(this ISqlExpression sqlExpression)
        {
            return $"@{Guid.NewGuid().ToString("N")}";
        }

        /// <summary>
        /// 将列索引转换为位标志
        /// </summary>
        /// <param name="sqlExpression"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static ColumnFlags ToFlag(this ISqlExpression sqlExpression, int column)
        {
            return (ColumnFlags)(1 << column);

        }
        /// <summary>
        /// 将列索引转换为位标志
        /// </summary>
        /// <param name="sqlExpression"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static ColumnFlags ToFlag(this ISqlExpression sqlExpression, int[] columns)
        {
            var cf = (ColumnFlags)0;

            if (columns == null | columns?.Length == 0) return (ColumnFlags)(-1);
            foreach (var ci in columns)
            {
                cf |= (ColumnFlags)(1 << ci);
            }

            return cf;

        }

        /// <summary>
        /// 连接多个字段名称,以','分隔 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlExpression"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string ColumnConnection<T>(this ISqlExpression sqlExpression, int[] columns)
        {

            string columnStr = null;
            var flags = ToFlag(null, columns);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attr = property.GetCustomAttribute<ColumnAttribute>();

                if ((attr?.Flag & flags) != 0 && attr != null)
                    columnStr = columnStr == null ? attr.Name : columnStr + "," + attr.Name;

            }

            return columnStr;
        }

        public static string MultiEqualCondition<T>(this ISqlExpression sqlExpression, int[] columns, T data, List<SqlParameter> paras, Connection conn = Connection.Default)
        {
            var str = "";
            var flags = ToFlag(null, columns);
            var connStr = conn == Connection.Default ? "," : conn.ToString();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attr = property.GetCustomAttribute<ColumnAttribute>();

                if ((attr?.Flag & flags) != 0 && attr != null)
                {
                    var paraName = CreateParaID(null);

                    str = str.Length > 0 ? $"{str} {connStr} {attr.Name}={paraName} " : $"{attr.Name}={paraName}";

                    paras.Add(new SqlParameter(paraName, property.GetValue(data)));

                }
            }

            return str;
        }


    }
}
