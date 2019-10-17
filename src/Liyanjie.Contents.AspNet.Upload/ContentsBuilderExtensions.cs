﻿using System;

using Liyanjie.Contents.AspNet;

namespace System.Web
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
            Action<UploadModuleOptions> configureOptions = null)
        {
            builder.AddModule<UploadModule, UploadModuleOptions>(configureOptions);

            return builder;
        }
    }
}