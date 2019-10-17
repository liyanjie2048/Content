using System.Collections.Generic;
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
            var models = request.Files.AllKeys
                .Select(_ => new UploadModel
                {
                    FileName = request.Files[_].FileName,
                    FileStream = request.Files[_].InputStream,
                    FileLength = request.Files[_].ContentLength,
                });
            var filePaths = new List<string>();
            foreach (var model in models)
            {
                var task = model.SaveAsync(options, dir);
                task.Wait();
                var filePath = task.Result;

                if (options.ReturnAbsolutePath)
                {
                    var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                    filePath = $"//{request.Url.Host}{port}/{filePath}";
                }

                filePaths.Add(filePath);
            }

            response.StatusCode = 200;
            response.ContentType = "application/json";
            response.Write(ContentsDefaults.JsonSerialize(filePaths));
        }
    }
}
