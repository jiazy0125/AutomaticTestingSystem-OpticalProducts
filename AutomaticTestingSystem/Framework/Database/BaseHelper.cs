using System.Configuration;
using System.Data;

namespace AutomaticTestingSystem.Framework.Database
{
    class BaseHelper:IDBHelper
    {
        protected string ConnString = ConfigurationManager.AppSettings["ConnectionStr"];
        //protected string ConnString = "Data Source=ZKLIGHT-PC;Initial Catalog=DBUser;Integrated Security=True";

        #region 常规查询
        public virtual int ExecuteNonQuery(string sql, CommandType type, object[] para)
        {
            return 0;
        }

        public virtual DataTable ExecuteTable(string sql, CommandType type, object[] para)
        {
            return null;
        }

        public virtual DataTable ExecuteTableByPage(int pageSize, int pageIndex, string sql, CommandType type, object[] para)
        {
            return null;
        }
        #endregion

        #region 事务处理
        public bool IsInTransaction
        {
            get;
            set;
        }
        public bool IsDirty
        {
            get;
            set;
        }
        public virtual void BeginTransaction()
        {
            IsInTransaction = true;
        }

        public virtual void Commit()
        {
            IsInTransaction = false;
            IsDirty = false;
        }

        public virtual void Rollback()
        {
            IsInTransaction = false;
            IsDirty = false;
        }
        #endregion
    }
}
