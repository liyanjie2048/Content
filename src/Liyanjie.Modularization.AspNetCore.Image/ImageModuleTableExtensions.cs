using System;
using System.Collections.Generic;

using Liyanjie.Modularization.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
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
        /// <param name="combineRouteTemplate"></param>
        /// <param name="concatenateRouteTemplate"></param>
        /// <param name="qrCodeRouteTemplate"></param>
        /// <param name="resizeRouteTemplate"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddImage(this ModularizationModuleTable moduleTable,
            string combineRouteTemplate = @"image/combine",
            string concatenateRouteTemplate = @"image/concatenate",
            string qrCodeRouteTemplate = @"image/qrCode",
            string resizeRouteTemplate = @"images/{filename}.{size}.{extension}",
            Action<ImageModuleOptions> configureOptions = null)
        {
            moduleTable.Services.AddSingleton<ImageCombineMiddleware>();
            moduleTable.Services.AddSingleton<ImageConcatenateMiddleware>();
            moduleTable.Services.AddSingleton<ImageQRCodeMiddleware>();
            moduleTable.Services.AddSingleton<ImageResizeMiddleware>();

            moduleTable.AddModule("ImageModule", new[]
            {
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = combineRouteTemplate,
                    Type = typeof(ImageCombineMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "POST" },
                    RouteTemplate = concatenateRouteTemplate,
                    Type =  typeof(ImageConcatenateMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = qrCodeRouteTemplate,
                    Type = typeof(ImageQRCodeMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = resizeRouteTemplate,
                    Type =  typeof(ImageResizeMiddleware),
                },
            }, configureOptions);

            return moduleTable;
        }
    }
}
