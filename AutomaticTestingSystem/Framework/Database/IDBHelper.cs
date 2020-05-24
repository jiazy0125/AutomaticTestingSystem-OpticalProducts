using System.Data;

namespace AutomaticTestingSystem.Framework.Database
{
    public interface IDBHelper
    {
        #region 常规查询
        /// <summary>
        /// 执行SQL语句返回受影响的行数
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql, CommandType type, object[] para);
        /// <summary>
        /// 执行SQL语句返回结果集
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        DataTable ExecuteTable(string sql, CommandType type, object[] para);
        /// <summary>
        /// 执行存储过程分页获取数据集
        /// </summary>
        /// <param name="pageSize">每页最大行数</param>
        /// <param name="pageIndex">获取页的编号</param>
        /// <param name="sql">执行的SQL语句</param>
        /// <param name="type">SQL语句的类型</param>
        /// <param name="para">参数数组</param>
        /// <returns></returns>
        DataTable ExecuteTableByPage(int pageSize, int pageIndex, string sql, CommandType type, object[] para);
        #endregion

        #region 事务处理
        /// <summary>
        /// 表示当前数据库公共访问层的实例是否在事务模式中
        /// </summary>
        bool IsInTransaction { get; }
        /// <summary>
        /// 表示当前数据库公共访问层的实例是否包含未提交至存储介质中的修改
        /// </summary>
        bool IsDirty { get; }
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 结束事务将所有存储中的仓储对象中位提交的修改保存至物理存储中
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚操作，抛弃所有未提交的修改，并重置事物模式
        /// </summary>
        void Rollback();
        #endregion
    }
}
