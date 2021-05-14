using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Liyanjie.Content.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ClickCodeMiddleware : IMiddleware
    {
        readonly IOptions<VerificationCodeModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ClickCodeMiddleware(IOptions<VerificationCodeModuleOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var options = this.options.Value;

            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var dic = context.Request.Query
                .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object);
            var model = dic.BuildModel<ClickCodeModel>();
            var (fontPoints, fontImage, boardImage) = await model.GenerateAsync(options);

            await options.SerializeToResponseAsync(context.Response, new
            {
                FontPoints = fontPoints,
                FontImage = fontImage.Encode(ImageFormat.Png),
                BoardImage = boardImage.Encode(ImageFormat.Png),
            });
        }
    }
}
