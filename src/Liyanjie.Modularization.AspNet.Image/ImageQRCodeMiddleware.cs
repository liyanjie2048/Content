using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content.Models;

using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageQRCodeMiddleware
    {
        readonly IOptions<ImageModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageQRCodeMiddleware(IOptions<ImageModuleOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            await Task.FromResult(0);

            var options = this.options.Value;

            if (options.RequestConstrainAsync is not null)
                if (!await options.RequestConstrainAsync.Invoke(context))
                    return;

            var query = context.Request.QueryString;
            var model = query.AllKeys
                .ToDictionary(_ => _.ToLower(), _ => query[_] as object)
                .BuildModel<ImageQRCodeModel>();
            var imagePath = model?.GenerateQRCode(options);
            if (imagePath.IsNotNullOrEmpty())
            {
                var response = context.Response;
                response.StatusCode = 200;
                response.ContentType = "image/jpeg";
                response.WriteFile(Path.Combine(options.RootDirectory, imagePath));
            }

            context.Response.End();
        }
    }
}
