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
        /// 图片合并约束
        /// </summary>
        public Func<HttpContext, Task<bool>> CombineConstrainAsync { get; set; }

        /// <summary>
        /// 图片拼接约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ConcatenateConstrainAsync { get; set; }

        /// <summary>
        /// 二维码图片约束
        /// </summary>
        public Func<HttpContext, Task<bool>> QRCodeConstrainAsync { get; set; }

        /// <summary>
        /// 图片缩放约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ResizeConstrainAsync { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }
            = async (request, type) =>
            {
                using var streamReader = new System.IO.StreamReader(request.InputStream);
                var str = await streamReader.ReadToEndAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
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
                response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
                response.End();
            };

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
