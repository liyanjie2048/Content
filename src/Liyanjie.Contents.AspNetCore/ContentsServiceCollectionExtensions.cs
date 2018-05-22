using System;

using Liyanjie.Contents.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddContents(this IServiceCollection services, Action<ContentsOptions> configureOptions)
        {
            services
                .Configure(configureOptions)
                .AddMvcCore();

            return services;
        }
    }
}
