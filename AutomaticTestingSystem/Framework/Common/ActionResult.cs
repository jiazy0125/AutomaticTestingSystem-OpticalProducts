using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticTestingSystem.Framework.Common
{
    public class ActionResult<T>
    {

        public ActionResult(bool status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public string Message { private set; get; }
        public bool Status { private set; get; }
        public T Data { private set; get; }

        public static ActionResult<T> SetSuccess(string message, T data = default)
        {
            return new ActionResult<T>(true, message, data);
        
        }

        public static ActionResult<T> SetError(string message, T data = default)
        {
            return new ActionResult<T>(false, message, data);
        }

    }
}
