using Liyanjie.AspNetCore.Contents.Core;

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
        /// <returns></returns>
        public static ContentsBuilder AddContents(this IServiceCollection services)
        {
            var builder = new ContentsBuilder(services);
            services.AddSingleton(builder);

            return builder;
        }
    }
}
