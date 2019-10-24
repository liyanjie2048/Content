using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModuleOptions : ImageOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync;

        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync;

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
