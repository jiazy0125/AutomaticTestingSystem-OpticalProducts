using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Model;
using AutomaticTestingSystem.Framework.Controller;

namespace AutomaticTestingSystem.Framework.Database
{

    public  class SqlExpression:ISqlCommand, ISqlExpression
    {

        public string Command { get; private set; }
        public List<SqlParameter> Parameters { get; private set; }

        public SqlExpression() 
        {
            Parameters = new List<SqlParameter>();
        }

        public override string ToString()
        {

            return Command;
        
        }
        public object[] ToParameters()
        {

            return Parameters?.ToArray();

        }
        /// <summary>
        /// 查询指定字段信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlExpression Select<T>(int[] columns)
        {

            var dbAttr = (DatabaseAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DatabaseAttribute));

            Command = $"select {this.ColumnConnection<T>(columns)} from {dbAttr.Table} ";

            return this;
        
        }

        /// <summary>
        /// 查询指定表字段信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlExpression Select<T>(string table, int[] columns)
        {
            Command = $"select {this.ColumnConnection<T>(columns)} from {table} ";
            return this;
        }

        public SqlExpression Select(string table, string[] columns)
        {
            var tempCol="";
            foreach (var item in columns)
            {
                tempCol += $"{(tempCol.Length <= 0 ? "" : ",")} {item}";
            }
            Command = $"select {tempCol} from {table} ";
            return this;
        }
        /// <summary>
        /// 删除表中的内容，需要结合Where条件函数一起使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SqlExpression Delete<T>()
        {
            var dbAttr = (DatabaseAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DatabaseAttribute));

            Command = $"delete from {dbAttr.Table} ";

            return this;
        }
        /// <summary>
        /// 更新表中数据,需要结合Set函数一起使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SqlExpression Update<T>()
        {
            var dbAttr = (DatabaseAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DatabaseAttribute));

            Command = $"update {dbAttr.Table} ";


            return this;
        
        
        }

        //public SqlExpression Set<T>(ColumnFlags column, object value)
        //{
        //    var paraName = this.CreateParaID();

        //    Command += $"set {this.Columns<T>(column)[0]} = {paraName}";
        //    Parameters.Add(new SqlParameter(paraName, value));

        //    return this;
        //}
        public SqlExpression Set<T>(int[] columns, T data)
        {
            Command += $"set {this.MultiEqualCondition(columns, data, Parameters)} ";

            return this;
        
        }
        public SqlExpression Set<T>(int[] columns, object[] values)
        {
            Command += "set ";
            var i = 0;
            foreach (var column in columns)
            {
                var paraName = this.CreateParaID();
                Command += $"{this.Column<T>(column)} = {paraName}{(i == columns.Length - 1 ? "" : ",")} ";
                Parameters.Add(new SqlParameter(paraName, values?[i]));
                i++;
            }


            return this;
        }
        /// <summary>
        /// 向数据库中插入数据,需要结合values函数使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        public SqlExpression Insert<T>()
        {

            var dbAttr = (DatabaseAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DatabaseAttribute));

            Command = $"insert into {dbAttr.Table} ";

            return this;
        }

        public SqlExpression Values<T>(int[] columnsIndex, T data)
        {
            var values = "";
            var cmd = "";
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attr = property.GetCustomAttribute<ColumnAttribute>();

                if ((attr?.Flag & this.ToFlag(columnsIndex)) != 0 && attr != null)
                {
                    var paraName = this.CreateParaID();

                    cmd = cmd.Length > 0 ? $"{cmd}, {attr.Name} " : $"{attr.Name}";
                    values = values.Length > 0 ? $"{values}, {paraName} " : $"{paraName}";

                    Parameters.Add(new SqlParameter(paraName, property.GetValue(data)));

                }
            }
            Command += $"({cmd}) values ({values})";


            return this;
        
        }


        public SqlExpression Where(ISqlCommand[] conditions)
        {
            string con = "";
            foreach (ISqlCommand isc in conditions)
            {

                con += isc.ToString();

                if (isc.Parameters?.Count > 0)
                    (Parameters = Parameters ?? new List<SqlParameter>()).AddRange(isc.Parameters);
            
            }
            if (con.Length > 0)
                Command += $"where {con} ";
            return this;
        }

        
    }




    public class ConditionExperssion<T>:ISqlCommand, ISqlExpression
    {
        public string Command { get; private set; }
        public List<SqlParameter> Parameters { get; private set; }

        public ConditionExperssion()
        {
            Parameters = new List<SqlParameter>();
        }

        public override string ToString()
        {
            return Command;
        }
        /// <summary>
        /// 条件连接语句,and
        /// </summary>
        /// <returns></returns>
        public ConditionExperssion<T> And()
        {

            Command += "and ";

            return this;


        }
        /// <summary>
        /// 单字段操作函数,Between条件语句
        /// </summary>
        /// <param name="column"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public ConditionExperssion<T> Between(int column, object minValue, object maxValue)
        {
            var min = this.CreateParaID(); 
            var max = this.CreateParaID();
            Command += $"{this.Column<T>(column)} between {min} and {max} ";

            Parameters.Add(new SqlParameter(min, minValue));
            Parameters.Add(new SqlParameter(max, maxValue));

            return this; ;
        
        
        }
        /// <summary>
        /// 单字段操作函数,等于条件语句
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConditionExperssion<T> Eq(int column, object value)
        {
            var paraName = this.CreateParaID();
            Command += $"{this.Column<T>(column)} = {paraName} ";

            Parameters.Add(new SqlParameter(paraName, value));

            return this;

        }
        public ConditionExperssion<T> Eq(string column, object value)
        {
            var paraName = this.CreateParaID();
            Command += $"{column} = {paraName} ";
            Parameters.Add(new SqlParameter(paraName, value));
            return this;

        }


        public ConditionExperssion<T> Eq(int[] columns, T data, Connection conn = Connection.And)
        {
            Command += this.MultiEqualCondition(columns, data, Parameters, conn);

            return this;
        }




        /// <summary>
        /// 单字段操作函数,字段相等条件语句
        /// </summary>
        /// <param name="field1"></param>
        /// <param name="field2"></param>
        /// <returns></returns>
        public ConditionExperssion<T> EqColumn(int column1, int column2) 
        {
            Command += $"{this.Column<T>(column1)} = {this.Column<T>(column2)} ";
            return this; 

        }
        /// <summary>
        /// 单字段操作函数,In条件语句
        /// </summary>
        /// <param name="field"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public ConditionExperssion<T> In(int column, object[] values)
        {
            
            var paraName = this.CreateParaID();
            Command += $"{this.Column<T>(column)} in ({paraName}) ";
            Parameters.Add(new SqlParameter(paraName, values));

            return this; 
        }
        /// <summary>
        /// 单字段操作函数,In条件语句
        /// </summary>
        /// <param name="field"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public ConditionExperssion<T> In(int column, SqlExpression sql)
        {
            Command += $"{this.Column<T>(column)} in ({sql.Command}) ";
            Parameters.AddRange(sql.Parameters);
            return this; 
        }
        /// <summary>
        /// 单字段操作函数,Like条件语句
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="like"></param>
        /// <returns></returns>
        public ConditionExperssion<T> Like(int column, string value, SqlLike like)
        {
            var paraName = this.CreateParaID();

            switch (like)
            {
                case SqlLike.StartWith:
                    value += "%";
                    break;
                case SqlLike.EndWith:
                    value = "%" + value;
                    break;
                case SqlLike.AnyWhere:
                    value="%"+value+"%";
                    break;
                default:break;                                           
            }

            Command += $"{this.Column<T>(column)} like {paraName} ";
            Parameters.Add(new SqlParameter(paraName, value));


            return this; 
        }
        /// <summary>
        /// 单字段操作函数,Like条件语句
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConditionExperssion<T> Like(int column, string value)
        {

            Command += $"{this.Column<T>(column)} like {value} ";

            return this; 
        }
        /// <summary>
        /// 条件连接语句,or
        /// </summary>
        /// <returns></returns>
        public ConditionExperssion<T> Or()
        {
            Command += "or ";
            return this; 
        }

        public ConditionExperssion<T> OrderBy(int[] columns, Sequence seq)
        {
            Command += $"order by {this.ColumnConnection<T>(columns)} {seq.ToString()}";

            return this;
        }

        public static ConditionExperssion<T> operator &(ConditionExperssion<T> cexp1, ConditionExperssion<T> cexp2) 
        {
            cexp1.Command += $" and {cexp2.Command} ";
            cexp1.Parameters.AddRange(cexp2.Parameters);
            return cexp1; 
        }
        public static ConditionExperssion<T> operator |(ConditionExperssion<T> cexp1, ConditionExperssion<T> cexp2) 
        {
            cexp1.Command += $" or {cexp2.Command} ";
            cexp1.Parameters.AddRange(cexp2.Parameters);
            return cexp1;
        }

        /// <summary>
        /// 利用GUID生成不重复的Parameter ID
        /// </summary>
        /// <returns></returns>



    }

    


}
