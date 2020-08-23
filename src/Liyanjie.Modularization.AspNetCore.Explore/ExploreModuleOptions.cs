using System;
using System.Text.Json;
using System.Threading.Tasks;

using Liyanjie.Contents;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ExploreModuleOptions : ExploreOptions
    {
        /// <summary>
        /// 上传约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ExploreConstrainAsync { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonSerializer.Serialize(obj));
#if NETCOREAPP3_0
                await response.CompleteAsync();
#endif
            };

        /// <summary>
        /// 返回文件绝对路径，默认：false
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
