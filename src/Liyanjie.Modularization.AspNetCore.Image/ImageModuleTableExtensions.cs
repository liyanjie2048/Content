using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
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
        /// <param name="combineToGIFRouteTemplate"></param>
        /// <param name="concatenateRouteTemplate"></param>
        /// <param name="cropRouteTemplate"></param>
        /// <param name="qrcodeRouteTemplate"></param>
        /// <param name="resizeRouteTemplates"></param>
        /// <returns></returns>
        public static ModuleTable AddImage(this ModuleTable moduleTable,
            Action<ImageModuleOptions> configureOptions,
            string combineRouteTemplate = "image/combine",
            string combineToGIFRouteTemplate="image/combineToGIF",
            string concatenateRouteTemplate = "image/concatenate",
            string cropRouteTemplate = "image/crop",
            string qrcodeRouteTemplate = "image/qrcode",
            params string[] resizeRouteTemplates)
        {
            moduleTable.Services.AddSingleton<ImageCombineMiddleware>();
            moduleTable.Services.AddSingleton<ImageCombineToGIFMiddleware>();
            moduleTable.Services.AddSingleton<ImageConcatenateMiddleware>();
            moduleTable.Services.AddSingleton<ImageCropMiddleware>();
            moduleTable.Services.AddSingleton<ImageQRCodeMiddleware>();
            moduleTable.Services.AddSingleton<ImageResizeMiddleware>();

            var middlewares = new List<ModuleMiddleware>
            {
                new()
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = combineRouteTemplate,
                    HandlerType = typeof(ImageCombineMiddleware),
                },
                new()
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = combineToGIFRouteTemplate,
                    HandlerType = typeof(ImageCombineToGIFMiddleware),
                },
                new()
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = concatenateRouteTemplate,
                    HandlerType =  typeof(ImageConcatenateMiddleware),
                },
                new()
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = cropRouteTemplate,
                    HandlerType =  typeof(ImageCropMiddleware),
                },
                new()
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = qrcodeRouteTemplate,
                    HandlerType = typeof(ImageQRCodeMiddleware),
                },
            };
            if (!resizeRouteTemplates.IsNullOrEmpty())
                foreach (var routeTemplate in resizeRouteTemplates)
                {
                    middlewares.Add(new()
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
