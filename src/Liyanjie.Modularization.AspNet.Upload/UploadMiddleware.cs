using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadMiddleware
    {
        readonly UploadModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UploadMiddleware(UploadModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var request = context.Request;

            var dir = "temps";
            var _dir = request.QueryString.GetValues("dir");
            if (!_dir.IsNullOrEmpty())
                dir = _dir.FirstOrDefault();

            var model = new UploadModel
            {
                Files = request.Files.AllKeys
                    .Select(_ => new UploadFileModel
                    {
                        FileName = request.Files[_].FileName,
                        FileStream = request.Files[_].InputStream,
                        FileLength = request.Files[_].ContentLength,
                    })
                    .ToArray(),
            };

            var paths = await model.SaveAsync(options, dir);
            var filePaths = paths.Select(_ => (_.Success, FilePath: _.Success ? _.FilePath.Replace(Path.DirectorySeparatorChar, '/') : _.FilePath));
            if (options.ReturnAbsolutePath)
            {
                var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"{request.Url.Scheme}://{request.Url.Host}{port}/{_.FilePath}" : _.FilePath));
            }

            await options.SerializeToResponseAsync(context.Response, filePaths.Select(_ => _.FilePath));

            context.Response.End();
        }
    }
}
