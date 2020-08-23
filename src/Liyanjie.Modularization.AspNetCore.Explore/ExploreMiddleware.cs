using System.Linq;
using System.Threading.Tasks;

using Liyanjie.Contents;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ExploreMiddleware : IMiddleware
    {
        readonly ExploreModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ExploreMiddleware(IOptions<ExploreModuleOptions> options)
        {
            this.options = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (options.ExploreConstrainAsync != null)
                if (!await options.ExploreConstrainAsync(context))
                    return;

            var request = context.Request;

            var contents = ExploreHelper.GetContents(options);
            if (options.ReturnAbsolutePath)
            {
                var pathPrefix = $"{request.Scheme}://{request.Host}/";
                foreach (var item in contents)
                {
                    fixPath(item, pathPrefix);
                }

                static void fixPath(ContentsModel.Directory dir, string pathPrefix)
                {
                    dir.Path = pathPrefix + dir.Path;
                    foreach (var item in dir.Files)
                    {
                        item.Path = pathPrefix + item.Path;
                    }
                    foreach (var item in dir.SubDirs)
                    {
                        fixPath(item, pathPrefix);
                    }
                }
            }

            await options.SerializeToResponseAsync(context.Response, contents);
        }
    }
}
