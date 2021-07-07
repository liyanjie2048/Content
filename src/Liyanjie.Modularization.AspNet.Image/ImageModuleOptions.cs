using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content;

using Newtonsoft.Json;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModuleOptions : ImageOptions
    {
        /// <summary>
        /// 请求约束
        /// </summary>
        public Func<HttpContext, Task<bool>> RequestConstrainAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }
            = async (request, type) =>
            {
                await Task.FromResult(0);

                using var streamReader = new StreamReader(request.InputStream);
                var str = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject(str, type);
            };

        /// <summary>
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                await Task.FromResult(0);

                response.Clear();
                response.ContentType = "application/json";
                response.Write(JsonConvert.SerializeObject(obj));
            };

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
