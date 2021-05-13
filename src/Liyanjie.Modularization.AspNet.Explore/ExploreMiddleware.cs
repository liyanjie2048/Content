using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ExploreMiddleware
    {
        readonly ExploreModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ExploreMiddleware(ExploreModuleOptions options)
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

            var request = context.Request;

            var contents = ExploreHelper.GetContents(options);
            if (options.ReturnAbsolutePath)
            {
                var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                var pathPrefix = $"{request.Url.Scheme}://{request.Url.Host}{port}/";
                foreach (var item in contents)
                {
                    fixPath(item, pathPrefix);
                }

                static void fixPath(ContentModel.Directory dir, string pathPrefix)
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

            context.Response.End();
        }
    }
}
