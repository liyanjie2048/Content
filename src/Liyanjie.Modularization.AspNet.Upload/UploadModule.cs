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
    public class UploadModule : IModularizationModule
    {
        readonly UploadModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public UploadModule(UploadModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public async Task<bool> TryMatchRequestingAsync(HttpContext httpContext)
        {
            return await Task.FromResult(options.TryMatchUpload(httpContext.Request));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        public async Task HandleResponsingAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var dir = request.QueryString["dir"];
            dir = dir.IsNullOrEmpty() ? "temps" : dir;
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
                filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"//{request.Url.Host}{port}/{_.FilePath}" : _.FilePath));
            }

            await ModularizationDefaults.SerializeToResponseAsync(httpContext.Response, filePaths.Select(_ => _.FilePath));
        }
    }
}
