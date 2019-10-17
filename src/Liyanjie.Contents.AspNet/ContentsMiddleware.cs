using System;
using System.Web;

namespace Liyanjie.Contents.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsMiddleware
    {
        readonly ContentsBuilder contentsBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="serviceProvider"></param>
        public ContentsMiddleware(ContentsBuilder contentsBuilder)
        {
            this.contentsBuilder = contentsBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool Invoke(HttpContext httpContext)
        {
            foreach (var moduleType in contentsBuilder.Modules.Keys)
            {
                if (Activator.CreateInstance(moduleType, contentsBuilder.Modules[moduleType]) is IContentsModule module)
                {
                    if (module.TryMatchRequesting(httpContext))
                    {
                        module.HandleResponsing(httpContext);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
