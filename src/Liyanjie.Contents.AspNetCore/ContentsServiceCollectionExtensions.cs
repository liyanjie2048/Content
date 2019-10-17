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
        /// <returns></returns>
        public static ContentsBuilder AddContents(this IServiceCollection services,
            Func<object, string> jsonSerialize,
            Func<string, Type, object> jsonDeserialize)
        {
            ContentsDefaults.JsonSerialize = jsonSerialize ?? throw new ArgumentNullException(nameof(jsonSerialize));
            ContentsDefaults.JsonDeserialize = jsonDeserialize ?? throw new ArgumentNullException(nameof(jsonDeserialize));

            var builder = new ContentsBuilder(services);
            services.AddSingleton(builder);

            return builder;
        }
    }
}
