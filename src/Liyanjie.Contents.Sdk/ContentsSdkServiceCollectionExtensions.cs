using System;
using Liyanjie.Content.Sdk;
using Liyanjie.Content.Sdk.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsSdkServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddContentsSdk(this IServiceCollection services, Action<ContentsOptions> optionsConfigure)
        {
            return services
                .Configure(optionsConfigure ?? throw new ArgumentNullException(nameof(optionsConfigure)))
                .AddTransient<ContentHelper>()
                .AddTransient<ImageHelper>();
        }
    }
}
