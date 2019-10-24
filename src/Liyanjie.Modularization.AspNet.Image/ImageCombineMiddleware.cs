using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageCombineMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageCombineMiddleware(ImageModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task HandleAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var model = (await options.DeserializeFromRequestAsync(request, typeof(ImageCombineModel))) as ImageCombineModel;
            var imagePath = (await model?.CombineAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');
            if (options.ReturnAbsolutePath)
            {
                var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                imagePath = $"//{request.Url.Host}{port}/{imagePath}";
            }

            await options.SerializeToResponseAsync(httpContext.Response, imagePath);

            httpContext.Response.End();
        }
    }
}
