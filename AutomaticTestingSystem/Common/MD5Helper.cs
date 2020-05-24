using System.Security.Cryptography;
using System.Text;

namespace AutomaticTestingSystem.Common
{
    public class MD5Helper
    {
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (var mi = MD5.Create())
            {
                var buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                var newBuffer = mi.ComputeHash(buffer);
                var sb = new StringBuilder();
                for (var i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
