using System;
using System.Data;
using System.Data.SqlClient;
using AutomaticTestingSystem.Framework.Common;
using AutomaticTestingSystem.Framework.Database;

namespace AutomaticTestingSystem.Common
{
    public class SystemService 
    {
        public static int Authority { get; set; }

        public static string Remark { get; set; }


        public static ActionResult<int> Login(string userName, string password)
        {
            //登陆验证用户 Task

            var sql = " select * from UserInfo where UserName = @userName and Password=@Passwd";
            var para = new SqlParameter[2];
            para[0] = new SqlParameter("@userName", userName);
            para[1] = new SqlParameter("@Passwd", MD5Helper.GenerateMD5(password));
            try
            {
                DataTable dt = DbFactory.Execute().ExecuteTable(sql, CommandType.Text, para);
                if (dt.Rows.Count > 0) 
                {
                    Authority = Convert.ToInt32(dt.Rows[0]["Authority"]);
                    Remark= dt.Rows[0]["Remark"].ToString();
                    return ActionResult<int>.SetSuccess("Login Successful.");
                }
                return ActionResult<int>.SetError("User name or password not correct.");
            }
            catch
            {
                return ActionResult<int>.SetError("User name or password not correct.");
            }
      
        }



    }
}
