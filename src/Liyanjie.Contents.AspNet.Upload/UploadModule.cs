using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Liyanjie.Contents.Models;

namespace Liyanjie.Contents.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadModule : IContentsModule
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
        public string Name => nameof(UploadModule);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public bool TryMatchRequesting(HttpContext httpContext)
        {
            return options.TryMatchUpload(httpContext.Request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        public void HandleResponsing(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;

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

            model.SaveAsync(options, dir)
                .ContinueWith(task =>
                {
                    var filePaths = task.Result.Select(_ => (_.Success, FilePath: _.Success ? _.FilePath.Replace(Path.DirectorySeparatorChar, '/') : _.FilePath));
                    if (options.ReturnAbsolutePath)
                    {
                        var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                        filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"//{request.Url.Host}{port}/{_.FilePath}" : _.FilePath));
                    }

                    response.StatusCode = 200;
                    response.ContentType = "application/json";
                    response.Write(ContentsDefaults.JsonSerialize(filePaths.Select(_ => _.FilePath)));
                })
                .Wait();
        }
    }
}
