using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ArithmeticImageCaptchaMiddleware
    {
        readonly CaptchaModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ArithmeticImageCaptchaMiddleware(CaptchaModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync(context))
                    return;

            var query = context.Request.QueryString;
            var dic = query.AllKeys
                .ToDictionary(_ => _.ToLower(), _ => query[_] as object);
            var model = dic.BuildModel<ArithmeticImageCaptchaModel>();
            var (code, image) = await model.GenerateAsync(options);

            await options.SerializeToResponseAsync(context.Response, new
            {
                Code = code,
                Image = image.Encode(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Png),
            });

            context.Response.End();
        }
    }
}
