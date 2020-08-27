using System;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <param name="combineRouteTemplate"></param>
        /// <param name="concatenateRouteTemplate"></param>
        /// <param name="cropRouteTemplate"></param>
        /// <param name="qrCodeRouteTemplate"></param>
        /// <param name="resizeRouteTemplates"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddImage(this ModularizationModuleTable moduleTable,
            Action<ImageModuleOptions> configureOptions,
            string combineRouteTemplate="image/combine",
            string concatenateRouteTemplate = "image/concatenate",
            string cropRouteTemplate="image/crop",
            string qrCodeRouteTemplate = "image/qrCode",
            params string[] resizeRouteTemplates)
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageCombineMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageConcatenateMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageCropMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageQRCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageResizeMiddleware), "Singleton");
            
            var middlewares = new List<ModularizationModuleMiddleware>
            {
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = combineRouteTemplate,
                    HandlerType = typeof(ImageCombineMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = concatenateRouteTemplate,
                    HandlerType =  typeof(ImageConcatenateMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = cropRouteTemplate,
                    HandlerType =  typeof(ImageCropMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = qrCodeRouteTemplate,
                    HandlerType = typeof(ImageQRCodeMiddleware),
                },
            };
            if (!resizeRouteTemplates.IsNullOrEmpty())
                foreach (var routeTemplate in resizeRouteTemplates)
                {
                    middlewares.Add(new ModularizationModuleMiddleware
                    {
                        HttpMethods = new[] { "GET" },
                        RouteTemplate = routeTemplate,
                        HandlerType = typeof(ImageResizeMiddleware),
                    });
                }

            moduleTable.AddModule("ImageModule", middlewares.ToArray(), configureOptions);

            return moduleTable;
        }
    }
}
