using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExploreModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static ModuleTable AddExplore(this ModuleTable moduleTable,
            Action<ExploreModuleOptions> configureOptions,
            string routeTemplate = "explore")
        {
            moduleTable.Services.AddSingleton<ExploreMiddleware>();

            moduleTable.AddModule("ExploreModule", new[]
            {
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = routeTemplate,
                   HandlerType = typeof(ExploreMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
