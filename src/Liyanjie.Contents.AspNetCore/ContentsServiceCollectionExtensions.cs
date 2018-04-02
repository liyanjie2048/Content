using Liyanjie.Contents.AspNetCore.Settings;
using Microsoft.Extensions.Configuration;

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
        /// <param name="settingsConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddContents(this IServiceCollection services, IConfiguration settingsConfiguration)
        {
            services
                .AddLogging()
                .Configure<Settings>(settingsConfiguration)
                .AddMvcCore();

            return services;
        }
    }
}
