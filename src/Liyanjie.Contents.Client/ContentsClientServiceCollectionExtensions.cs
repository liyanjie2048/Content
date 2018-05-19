using System;
using Liyanjie.Content.Client;
using Liyanjie.Content.Client.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsClientServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsConfigure"></param>
        /// <returns></returns>
        public static IServiceCollection AddContentsSdk(this IServiceCollection services, Action<ContentsClientOptions> optionsConfigure)
        {
            return services
                .Configure(optionsConfigure ?? throw new ArgumentNullException(nameof(optionsConfigure)))
                .AddTransient<ContentHelper>()
                .AddTransient<ImageHelper>();
        }
    }
}
