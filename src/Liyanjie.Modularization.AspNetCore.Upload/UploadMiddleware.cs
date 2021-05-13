using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Liyanjie.Content.Models;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var request = context.Request;

            var dir = "temps";
            if (request.Query.TryGetValue("dir", out var _dir) && !_dir.IsNullOrEmpty())
                dir = _dir.FirstOrDefault();

            var files = new List<UploadFileModel>(request.Form.Files.Count);
            foreach (var item in request.Form.Files)
            {
                using var memory = new MemoryStream();
                item.OpenReadStream().CopyTo(memory);
                files.Add(new UploadFileModel
                {
                    FileName = item.FileName,
                    FileBytes = memory.ToArray(),
                    FileLength = item.Length,
                });
            }
            var model = new UploadModel
            {
                Files = files.ToArray(),
            };

            var paths = await model.SaveAsync(options, dir);
            var filePaths = paths.Select(_ => (_.Success, FilePath: _.Success ? _.FilePath.Replace(Path.DirectorySeparatorChar, '/') : _.FilePath));
            if (options.ReturnAbsolutePath)
                filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"{request.Scheme}://{request.Host}/{_.FilePath}" : _.FilePath));

            await options.SerializeToResponseAsync(context.Response, filePaths.Select(_ => _.Success ? _.FilePath : string.Empty).ToArray());
        }
    }
}
