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
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
