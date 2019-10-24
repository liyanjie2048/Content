using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Liyanjie.Contents.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageQRCodeMiddleware : IMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageQRCodeMiddleware(IOptions<ImageModuleOptions> options)
        {
            this.options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var model = context.Request.Query
                .ToDictionary(_ => _.Key, _ => (object)_.Value.FirstOrDefault())
                .BuildModel<ImageQRCodeModel>();
            var imagePath = model?.GenerateQRCode(options);
            if (!imagePath.IsNullOrEmpty())
            {
                var response = context.Response;
                response.StatusCode = 200;
                response.ContentType = "image/jpg";
                using var stream = File.OpenRead(Path.Combine(options.RootDirectory, imagePath));
                await stream.CopyToAsync(response.Body);
            }
        }
    }
}
