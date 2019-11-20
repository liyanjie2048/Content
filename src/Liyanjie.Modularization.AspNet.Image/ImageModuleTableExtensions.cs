using System.Collections.Generic;

using Liyanjie.Modularization.AspNet;

namespace System.Web
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
            string combineRouteTemplate="image/combine",
            string concatenateRouteTemplate = "image/concatenate",
            string qrCodeRouteTemplate = "image/qrCode",
            string resizeRouteTemplate = "images/{filename}.{size}.{extension}",
            Action<ImageModuleOptions> configureOptions = null)
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageCombineMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageConcatenateMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageQRCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ImageResizeMiddleware), "Singleton");

            moduleTable.AddModule("ImageModule", new[]
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
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = qrCodeRouteTemplate,
                    HandlerType = typeof(ImageQRCodeMiddleware),
                },
                new ModularizationModuleMiddleware
                {
                    HttpMethods = new[] { "GET" },
                    RouteTemplate = resizeRouteTemplate,
                    HandlerType =  typeof(ImageResizeMiddleware),
                },
            }, configureOptions);

            return moduleTable;
        }
    }
}
