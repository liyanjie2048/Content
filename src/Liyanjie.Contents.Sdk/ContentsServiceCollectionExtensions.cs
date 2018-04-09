using System;
using Liyanjie.Content.Sdk;
using Liyanjie.Content.Sdk.Helpers;

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
        /// <param name="optionsConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddContentsSdk(this IServiceCollection services, Action<ContentsOptions> optionsConfigure)
        {
            if (optionsConfigure == null)
                throw new ArgumentNullException(nameof(optionsConfigure));

            return services
                .Configure(optionsConfigure)
                .AddTransient<ContentsHelper>()
                .AddTransient<ImageHelper>();
        }
    }
}
