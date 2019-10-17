using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Contents.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsMiddleware
    {
        readonly RequestDelegate next;
        readonly IServiceProvider serviceProvider;
        readonly ContentsBuilder contentsBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="serviceProvider"></param>
        public ContentsMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.contentsBuilder = serviceProvider.GetRequiredService<ContentsBuilder>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            foreach (var moduleType in contentsBuilder.ModuleTypes)
            {
                if (ActivatorUtilities.CreateInstance(serviceProvider, moduleType) is IContentsModule module)
                {
                    if (await module.TryMatchRequestingAsync(httpContext))
                    {
                        await module.HandleResponsingAsync(httpContext);
                        return;
                    }
                }
            }

            await next(httpContext);
        }
    }
}
