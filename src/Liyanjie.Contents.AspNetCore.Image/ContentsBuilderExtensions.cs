using System;

using Liyanjie.Contents.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ContentsBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ContentsBuilder AddImage(this ContentsBuilder builder,
            Action<ImageModuleOptions> configureOptions = null)
        {
            builder.AddModule<ImageModule, ImageModuleOptions>(configureOptions);

            return builder;
        }
    }
}
