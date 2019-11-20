using System.Collections.Generic;

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
        /// <param name="moduleTable"></param>
        /// <param name="routeTemplate"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddUpload(this ModularizationModuleTable moduleTable,
            string routeTemplate = "upload",
            Action<UploadModuleOptions> configureOptions = null)
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(UploadMiddleware), "Singleton");

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
