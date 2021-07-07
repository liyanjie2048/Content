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
        public static ModuleTable AddUpload(this ModuleTable moduleTable,
            Action<UploadModuleOptions> configureOptions,
            string routeTemplate = "upload")
        {
            moduleTable.Services.AddSingleton<UploadMiddleware>();

            moduleTable.AddModule("UploadModule", new[]
            {
               new ModuleMiddleware
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
