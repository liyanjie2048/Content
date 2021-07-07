using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNet
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
        public static ModularizationModuleTable AddExplore(this ModularizationModuleTable moduleTable,
            Action<ExploreModuleOptions> configureOptions,
            string routeTemplate = "explore")
        {
            moduleTable.Services.AddSingleton<ExploreMiddleware>();

            moduleTable.AddModule("ExploreModule", new[]
            {
               new ModularizationModuleMiddleware
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
