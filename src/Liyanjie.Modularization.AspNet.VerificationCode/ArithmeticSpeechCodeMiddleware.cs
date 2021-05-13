using System;
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
    public class ArithmeticSpeechCodeMiddleware
    {
        readonly VerificationCodeModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ArithmeticSpeechCodeMiddleware(VerificationCodeModuleOptions options)
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
            var model = query.AllKeys
                .ToDictionary(_ => _.ToLower(), _ => query[_] as object)
                .BuildModel<ArithmeticSpeechCodeModel>();
            var (code, audio) = await model.GenerateAsync(options);

            await options.SerializeToResponseAsync(context.Response, new
            {
                Code = code,
                Audio = Convert.ToBase64String(audio),
            });

            context.Response.End();
        }
    }
}
