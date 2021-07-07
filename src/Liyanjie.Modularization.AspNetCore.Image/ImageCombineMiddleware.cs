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
    public class ImageCombineMiddleware : IMiddleware
    {
        readonly IOptions<ImageModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageCombineMiddleware(IOptions<ImageModuleOptions> options)
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

            if (options.RequestConstrainAsync is not null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var request = context.Request;

            var model = (await options.DeserializeFromRequestAsync(request, typeof(ImageCombineModel))) as ImageCombineModel;
            if (model is not null)
            {
                var imagePath = (await model.CombineAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');

                if (options.ReturnAbsolutePath)
                    imagePath = $"{request.Scheme}://{request.Host}/{imagePath}";

                await options.SerializeToResponseAsync(context.Response, imagePath);
            }
        }
    }
}
