using System;

using Liyanjie.AspNetCore.Contents;

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
            Action<ImageOptions> configureOptions = null)
        {
            builder.AddModule<ImageModule, ImageOptions>(configureOptions);

            return builder;
        }
    }
}
