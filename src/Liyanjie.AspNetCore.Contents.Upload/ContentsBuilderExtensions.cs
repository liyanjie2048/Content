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
        public static ContentsBuilder AddUpload(this ContentsBuilder builder,
            Action<UploadOptions> configureOptions = null)
        {
            builder.AddModule<UploadModule, UploadOptions>(configureOptions);

            return builder;
        }
    }
}
