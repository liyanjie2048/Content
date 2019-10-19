using System;

using Liyanjie.Modularization.AspNet;

namespace System.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class UploadModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddUpload(this ModularizationModuleTable builder,
            Action<UploadModuleOptions> configureOptions = null)
        {
            builder.AddModule<UploadModule, UploadModuleOptions>(configureOptions);

            return builder;
        }
    }
}
