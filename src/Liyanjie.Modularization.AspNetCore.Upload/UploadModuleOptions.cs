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
    public class UploadModuleOptions : UploadOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync = async (response, obj) =>
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(obj));
        };

        /// <summary>
        /// 返回文件绝对路径，默认：true
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
