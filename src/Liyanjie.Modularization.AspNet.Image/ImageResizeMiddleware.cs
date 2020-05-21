using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageResizeMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageResizeMiddleware(ImageModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            if (options.ResizeConstrainAsync != null)
                if (!await options.ResizeConstrainAsync.Invoke(context))
                    return;

            var model = new ImageResizeModel { ImagePath = context.Request.Path };
            var imagePath = model.Resize(options)?.Replace(Path.DirectorySeparatorChar, '/');
            if (!imagePath.IsNullOrEmpty())
                context.Response.Redirect(imagePath);

            await Task.FromResult(0);
        }
    }
}
