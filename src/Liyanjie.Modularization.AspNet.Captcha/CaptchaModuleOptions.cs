using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class CaptchaModuleOptions : CaptchaOptions
    {
        /// <summary>
        /// 生成验证码请求约束
        /// </summary>
        public Func<HttpContext, Task<bool>> RequestConstrainAsync { get; set; }

        /// <summary>
        /// 序列化输出
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                await Task.FromResult(0);

                response.Clear();
                response.ContentType = "application/json";
                response.Write(JsonSerializer.Serialize(obj));
            };

        /// <summary>
        /// 返回文件绝对路径，默认：false
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
