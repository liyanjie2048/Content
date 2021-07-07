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
    public class ArithmeticImageCodeMiddleware : IMiddleware
    {
        readonly IOptions<CaptchaModuleOptions> options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ArithmeticImageCodeMiddleware(IOptions<CaptchaModuleOptions> options)
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

            var model = context.Request.Query
                .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
                .BuildModel<ArithmeticImageCaptchaModel>();
            var (code, image) = await model.GenerateAsync(options);

            await options.SerializeToResponseAsync(context.Response, new
            {
                Code = code,
                Image = image.Encode(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Png),
            });
        }
    }
}
