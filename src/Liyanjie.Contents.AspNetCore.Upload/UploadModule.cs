﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Liyanjie.Contents.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Liyanjie.Contents.AspNetCore
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
        public UploadModule(IOptions<UploadModuleOptions> options)
        {
            this.options = options.Value;
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
            var response = httpContext.Response;

            var dir = StringValues.IsNullOrEmpty(request.Query["dir"]) ? "temps" : request.Query["dir"][0];
            var form = await request.ReadFormAsync();
            var model = new UploadModel
            {
                Files = form.Files
                    .Select(_ => new UploadFileModel
                    {
                        FileName = _.FileName,
                        FileStream = _.OpenReadStream(),
                        FileLength = _.Length,
                    })
                    .ToArray(),
            };

            var filePaths = (await model.SaveAsync(options, dir)).Select(_ => (_.Success, FilePath: _.Success ? _.FilePath.Replace(Path.DirectorySeparatorChar, '/') : _.FilePath));
            if (options.ReturnAbsolutePath)
                filePaths = filePaths.Select(_ => (_.Success, _.Success ? $"//{request.Host}/{_.FilePath}" : _.FilePath));

            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(ContentsDefaults.JsonSerialize(filePaths.Select(_ => _.FilePath)));
        }
    }
}
