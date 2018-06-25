using Liyanjie.AspNetCore.Contents.Core;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseContents(this IApplicationBuilder app)
        {
            app.UseMiddleware<ContentsMiddleware>();

            return app;
        }
    }
}
