using System;
using System.Web;
using Liyanjie.Contents.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsHttpApplicationExtensions
    {
        public static ContentsBuilder ContentsBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ContentsBuilder AddContents(this HttpApplication app,
            Func<object, string> jsonSerialize,
            Func<string, Type, object> jsonDeserialize)
        {
            ContentsDefaults.JsonSerialize = jsonSerialize ?? throw new ArgumentNullException(nameof(jsonSerialize));
            ContentsDefaults.JsonDeserialize = jsonDeserialize ?? throw new ArgumentNullException(nameof(jsonDeserialize));
            
            ContentsBuilder = new ContentsBuilder();

            return ContentsBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static HttpApplication UseContents(this HttpApplication app)
        {
            if (new ContentsMiddleware(ContentsBuilder).Invoke(app.Context))
                app.Context.Response.End();

            return app;
        }
    }
}
