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
    public class ImageQRCodeMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageQRCodeMiddleware(ImageModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task HandleAsync(HttpContext httpContext)
        {
            var query = httpContext.Request.QueryString;
            var model = query.AllKeys
                .ToDictionary(_ => _, _ => (object)query[_])
                .BuildModel<ImageQRCodeModel>();
            var imagePath = model?.GenerateQRCode(options);
            if (!imagePath.IsNullOrEmpty())
            {
                var response = httpContext.Response;
                response.StatusCode = 200;
                response.ContentType = "image/jpg";
                response.WriteFile(Path.Combine(options.RootDirectory, imagePath));
            }

            await Task.FromResult(0);

            httpContext.Response.End();
        }
    }
}
