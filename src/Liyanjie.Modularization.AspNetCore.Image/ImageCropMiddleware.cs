using System;
using System.IO;
using System.Threading.Tasks;

using Liyanjie.Content.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageCropMiddleware : IMiddleware
    {
        readonly IOptions<ImageModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageCropMiddleware(IOptions<ImageModuleOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var options = this.options.Value;

            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var request = context.Request;

            var model = (await options.DeserializeFromRequestAsync(request, typeof(ImageCropModel))) as ImageCropModel;
            var imagePath = (await model?.CropAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');

            if (options.ReturnAbsolutePath)
                imagePath = $"{request.Scheme}://{request.Host}/{imagePath}";

            await options.SerializeToResponseAsync(context.Response, imagePath);
        }
    }
}
