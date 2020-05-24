using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using AutomaticTestingSystem.Framework.Database;
using AutomaticTestingSystem.Framework.Interfaces;
using AutomaticTestingSystem.UserControls.ProductModelManage;

namespace AutomaticTestingSystem.Framework.Common
{
    public static class DataExtension
    {
        private static ActionResult<List<T>> DataRead<T>(string sql, object[] parameters, ColumnFlags flags)
        {
            var lt = new List<T>();
            try
            {
                DataTable dt = DbFactory.Execute().ExecuteTable(sql, CommandType.Text, parameters);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var model = (T)Assembly.Load("AutomaticTestingSystem").CreateInstance(typeof(T).FullName);
                        PropertyInfo[] properties = model.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            try
                            {
                                var attr = property.GetCustomAttribute<ColumnAttribute>();

                                if ((attr?.Flag & flags) != 0 && attr != null)
                                    property.SetValue(model, dr[attr.Name]);
                            }
                            catch { }
                        }

                        lt.Add(model);
                    }
                }
                return ActionResult<List<T>>.SetSuccess("Access data successful.", lt);
            }
            catch (Exception e)
            {
                return ActionResult<List<T>>.SetError(e.Message);
            }
        }
        private static ActionResult<List<T>> DataRead<T>(string sql, object[] parameters)
        {
            var lt = new List<T>();
            try
            {
                DataTable dt = DbFactory.Execute().ExecuteTable(sql, CommandType.Text, parameters);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var model = (T)Assembly.Load("AutomaticTestingSystem").CreateInstance(typeof(T).FullName);
                        PropertyInfo[] properties = model.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            try
                            {
                                var attr = property.GetCustomAttribute<ColumnAttribute>();
                                property.SetValue(model, dr[attr.Name]);
                            }
                            catch { }
                        }

                        lt.Add(model);
                    }
                }
                return ActionResult<List<T>>.SetSuccess("Access data successful.", lt);
            }
            catch (Exception e)
            {
                return ActionResult<List<T>>.SetError(e.Message);
            }
        }
      
        /// <summary>
        /// 获取Model对应存储数据库中的数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="columns"></param>
        /// <param name="cexps"></param>
        /// <returns></returns>
        public static ActionResult<List<T>> AccessData<T>(this IData instance, int[] columns, params ConditionExperssion<T>[] cexps) where T : class
        {
            var flags = SqlExpressionExtensions.ToFlag(null, columns);
            var sqlCommand = new SqlExpression().Select<T>(columns).Where(cexps);
            return DataRead<T>(sqlCommand.Command, sqlCommand.ToParameters(), flags);
        }

        public static ActionResult<List<T>> AccessData<T>(this IData instance, string table, string[] columns, params ISqlCommand[] cexps) where T : class
        {
            var sqlCommand = new SqlExpression().Select(table, columns).Where(cexps);
            return DataRead<T>(sqlCommand.Command, sqlCommand.ToParameters());
        }

        public static ActionResult<List<T>> AccessData<T>(this IData instance, string sql) where T : class
        {
            return DataRead<T>(sql, null);
        }
        /// <summary>
        /// 单表操作，存储数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult<int> SaveData<T>(this IData instance, T data, int[] columns)
        {
            var sqlCommand = new SqlExpression().Insert<T>().Values(columns, data);

            try
            {
                DbFactory.Execute().ExecuteNonQuery(sqlCommand.Command, CommandType.Text, sqlCommand.ToParameters());
                return ActionResult<int>.SetSuccess("Saved successful.");
            }
            catch (Exception e)
            {
                return ActionResult<int>.SetError(e.Message, -1);
            }
        }
        /// <summary>
        /// 单表操作，一次性存储多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="data"></param>
        public static ActionResult<int> SaveData<T>(this IData instance, T[] data, int[][] columns = null)
        {

            IDBHelper db = DbFactory.Execute();
            db.BeginTransaction();
            var i = 0;
            foreach (T t in data)
            {
                int[] column;
                try
                {
                    column = columns[i];
                }
                catch (Exception e)
                {
                    return ActionResult<int>.SetError(e.Message, -1);
                }
                var sqlCommand = new SqlExpression().Insert<T>().Values(column, t);

                try
                {
                    db.ExecuteNonQuery(sqlCommand.Command, CommandType.Text, sqlCommand.ToParameters());
                }
                catch (Exception e)
                {
                    return ActionResult<int>.SetError(e.Message, -1);
                }
                i++;
            }
            db.Commit();
            return ActionResult<int>.SetSuccess("Saved successful.");
        }

        /// <summary>
        /// 单表操作,删除指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static ActionResult<int> DeleteData<T>(this IData instance, params ConditionExperssion<T>[] conditions)
        {
            var sqlcommand = new SqlExpression().Delete<T>().Where(conditions);

            try
            {
                DbFactory.Execute().ExecuteNonQuery(sqlcommand.Command, CommandType.Text, sqlcommand.ToParameters());
                return ActionResult<int>.SetSuccess("Delete successful.");
            }
            catch (Exception e)
            {
                return ActionResult<int>.SetError(e.Message, -1);
            }

        }
        /// <summary>
        /// 单表操作,更新数据库中指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl"></param>
        /// <param name="column"></param>
        /// <param name="data"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public static ActionResult<int> UpdateData<T>(this IData instance, int[] columns, T data, params ISqlCommand[] conditions)
        {

            var sqlcommand = new SqlExpression().Update<T>().Set(columns, data).Where(conditions);

            try
            {
                DbFactory.Execute().ExecuteNonQuery(sqlcommand.Command, CommandType.Text, sqlcommand.ToParameters());
                return ActionResult<int>.SetSuccess("Update successful.");
            }
            catch (Exception e)
            {
                return ActionResult<int>.SetError($"{e.Message}'{data.ToString()}'", -1);
            }
        }

        public static ActionResult<int> UpdateData<T>(this IData instance, int[] columns, object[] data, params ISqlCommand[] conditions)
        {

            var sqlcommand = new SqlExpression().Update<T>().Set<T>(columns, data).Where(conditions);

            try
            {
                DbFactory.Execute().ExecuteNonQuery(sqlcommand.Command, CommandType.Text, sqlcommand.ToParameters());
                return ActionResult<int>.SetSuccess("Update successful.");
            }
            catch (Exception e)
            {
                return ActionResult<int>.SetError($"{e.Message}'{data.ToString()}'", -1);
            }
        }

        public static void LoadTableDefine(this IData instance, string parent)
        {
            var res = AccessData(null, null, new[]
            {
                new ConditionExperssion<TableDefineModel>().Eq(5, parent)
            });
            SystemSettings.TablesDetails.Clear();
            foreach (var item in res.Data)
            {
                SystemSettings.TablesDetails.Add(item);
            }

        }
















    }
}
