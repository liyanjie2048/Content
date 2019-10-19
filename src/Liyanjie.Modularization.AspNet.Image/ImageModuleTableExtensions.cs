using System;

using Liyanjie.Modularization.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddImage(this ModularizationModuleTable builder,
            Action<ImageModuleOptions> configureOptions = null)
        {
            builder.AddModule<ImageModule, ImageModuleOptions>(configureOptions);

            return builder;
        }
    }
}
