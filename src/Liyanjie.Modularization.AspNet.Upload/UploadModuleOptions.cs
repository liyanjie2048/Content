using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadModuleOptions : UploadOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;

        /// <summary>
        /// 返回文件绝对路径，默认：true
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
