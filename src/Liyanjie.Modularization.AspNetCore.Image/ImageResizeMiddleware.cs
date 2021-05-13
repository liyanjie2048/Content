﻿using System;
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
    public class ImageResizeMiddleware : IMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageResizeMiddleware(IOptions<ImageModuleOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync.Invoke(context))
                    return;

            var request = context.Request;

            var model = new ImageResizeModel { ImagePath = request.Path };
            var imagePath = model.Resize(options)?.Replace(Path.DirectorySeparatorChar, '/');
            if (!imagePath.IsNullOrEmpty())
                context.Response.Redirect($"/{imagePath}");

            await Task.CompletedTask;
        }
    }
}
