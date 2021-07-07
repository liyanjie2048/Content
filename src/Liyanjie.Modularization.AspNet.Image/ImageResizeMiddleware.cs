using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content.Models;

using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageResizeMiddleware
    {
        readonly IOptions<ImageModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageResizeMiddleware(IOptions<ImageModuleOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            await Task.FromResult(0);

            var options = this.options.Value;

            if (options.RequestConstrainAsync is not null)
                if (!await options.RequestConstrainAsync.Invoke(context))
                    return;

            var model = new ImageResizeModel { ImagePath = context.Request.Path };
            var imagePath = model.Resize(options)?.Replace(Path.DirectorySeparatorChar, '/');
            if (imagePath.IsNotNullOrEmpty())
                context.Response.Redirect(imagePath);
        }
    }
}
