using System;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Contents.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadModuleOptions : UploadOptions
    {
        public Func<HttpRequest, bool> TryMatchUpload { get; set; }
            = request => true
                && "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//POST请求
                && ContentsDefaults.TryMatchTemplate(request.Path, "Upload");

        /// <summary>
        /// 返回文件绝对路径，默认：true
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
