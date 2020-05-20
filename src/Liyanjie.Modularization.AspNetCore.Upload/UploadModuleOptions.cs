using System;
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
        /// 上传约束
        /// </summary>
        public Func<HttpContext, Task<bool>> UploadConstrainAsync { get; set; }

        /// <summary>
        /// 序列化输出
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }

        /// <summary>
        /// 返回文件绝对路径，默认：false
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
