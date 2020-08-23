using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ExploreModuleOptions : ExploreOptions
    {
        /// <summary>
        /// 浏览约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ExploreConstrainAsync { get; set; }

        /// <summary>
        /// 序列化输出
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                await Task.FromResult(0);

                response.Clear();
                response.ContentType = "application/json";
                response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                response.End();
            };

        /// <summary>
        /// 返回文件绝对路径，默认：false
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
