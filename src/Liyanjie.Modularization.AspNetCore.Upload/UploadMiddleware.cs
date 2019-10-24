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
    public class UploadMiddleware : IMiddleware
    {
        readonly UploadModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UploadMiddleware(IOptions<UploadModuleOptions> options)
        {
            this.options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            var dir = request.Query["dir"].FirstOrDefault();
            dir = dir.IsNullOrEmpty() ? "temps" : dir;
            var model = new UploadModel
            {
                Files = request.Form.Files
                    .Select(_ => new UploadFileModel
                    {
                        FileName = _.FileName,
                        FileStream = _.OpenReadStream(),
                        FileLength = _.Length,
                    })
                    .ToArray(),
            };

            var paths = await model.SaveAsync(options, dir);
            var filePaths = paths.Select(_ => (_.Success, FilePath: _.Success ? _.FilePath.Replace(Path.DirectorySeparatorChar, '/') : _.FilePath));
            if (options.ReturnAbsolutePath)
                filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"//{request.Host}/{_.FilePath}" : _.FilePath));

            await options.SerializeToResponseAsync(context.Response, filePaths.Select(_ => _.FilePath));
        }
    }
}
