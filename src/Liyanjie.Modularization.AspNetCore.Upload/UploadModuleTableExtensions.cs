using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class UploadModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddUpload(this ModularizationModuleTable moduleTable,
            Action<UploadModuleOptions> configureOptions,
            string routeTemplate = "upload")
        {
            moduleTable.Services.AddSingleton<UploadMiddleware>();

            moduleTable.AddModule("UploadModule", new[]
            {
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "POST" },
                   RouteTemplate = routeTemplate,
                   HandlerType = typeof(UploadMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
