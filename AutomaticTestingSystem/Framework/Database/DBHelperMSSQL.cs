using System;
using System.Data;
using System.Data.SqlClient;

namespace AutomaticTestingSystem.Framework.Database
{
    class DBHelperMSSQL : BaseHelper
    {
        SqlConnection cn = new SqlConnection();
        SqlTransaction tran;

        #region 打开关闭
        public DBHelperMSSQL()
        {
            IsInTransaction = false;
            IsDirty = false;
        }

        void Open()
        {
            if (cn.State != ConnectionState.Open)
            {
                if (cn
                    == null)
                {
                    cn = new SqlConnection(ConnString);
                }
                if (cn.State.Equals(ConnectionState.Closed))
                {
                    cn.ConnectionString = ConnString;
                    cn.Open();
                }
            }
        }
        void Close()
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }
        #endregion

        #region 执行SQL语句返回受影响的行数
        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param> 
        /// <param name="type">SQL语句的类型</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string sql, CommandType type, object[] para)
        {
            try
            {
                int result = 0;
                using (var cmd = new SqlCommand() { Connection = cn, CommandText = sql, CommandType = type })
                {
                    if (para != null)
                    {
                        foreach (SqlParameter item in para)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }
                    //如果不是出于事务状态，则开启数据库连接，否则给cmd设置事务
                    if (!IsInTransaction)
                    {
                        Open();
                    }
                    else
                    {
                        cmd.Transaction = tran;
                    }
                    //执行SQL语句
                    result = cmd.ExecuteNonQuery();
                    //如果出于事务状态，则存在未保存的修改
                    if (IsInTransaction)
                    {
                        IsDirty = true;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                if (IsInTransaction)
                {
                    Rollback();
                }
                throw e;
            }
            finally
            {
                if (!IsInTransaction)
                {
                    Close();
                }
            }
        }
        #endregion

        #region 执行SQL语句返回结果集

        public override DataTable ExecuteTable(string sql, CommandType type, object[] para)
        {
            try
            {
                var ds = new DataSet();
                if (!IsInTransaction)
                {
                    Open();
                }
                //创建SqlCommand 对象
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = type;
                    cmd.CommandText = sql;
                    //判断cmd 参数是否为空，如果不为空则传入参数
                    if (para != null)
                    {
                        var cmdParms = (SqlParameter[])para;
                        foreach (SqlParameter parm in cmdParms)
                        {
                            cmd.Parameters.Add(parm);
                        }
                    }
                    //创建数据适配器对象 用于填充Dataset
                    var da = new SqlDataAdapter(cmd);
                    //填充DataSet 对象
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                if (IsInTransaction)
                {
                    Rollback();
                }
                throw e;
            }
            finally
            {
                if (!IsInTransaction)
                {
                    Close();
                }
            }
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageSize">每页显示的行数</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        public override DataTable ExecuteTableByPage(int pageSize, int pageIndex, string sql, CommandType type, object[] para)
        {
            try
            {
                using (var ds = new DataSet())
                {
                    if (!IsInTransaction)
                    {
                        Open();
                    }
                    //创建SqlCommand 对象
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = type;
                        cmd.CommandText = sql;
                        //判断cmd 参数是否为空，如果不为空则传入参数
                        if (para != null)
                        {
                            var cmdParms = (SqlParameter[])para;
                            foreach (SqlParameter parm in cmdParms)
                            {
                                cmd.Parameters.Add(parm);
                            }
                        }
                        //创建数据适配器对象 用于填充Dataset
                        var da = new SqlDataAdapter(cmd);
                        //填充DataSet 对象
                        da.Fill(ds, (pageIndex * pageSize), pageSize, "table1");
                    }
                    return ds.Tables["table1"];
                }
            }
            catch (Exception e)
            {
                if (IsInTransaction)
                {
                    Rollback();
                }
                throw e;
            }
            finally
            {
                if (!IsInTransaction)
                {
                    Close();
                }
            }
        }

        #endregion

        #region 事务操作

        public override void BeginTransaction()
        {
            try
            {
                Open();
                tran = cn.BeginTransaction();
                base.BeginTransaction();
            }
            catch
            {
                IsInTransaction = false;
            }
        }

        public override void Commit()
        {
            try
            {
                tran.Commit();
                Close();
                base.Commit();
            }
            catch
            {
                Rollback();
            }
        }

        public override void Rollback()
        {
            tran.Rollback();
            Close();
            base.Rollback();
        }

        #endregion
    }
}
