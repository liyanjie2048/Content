namespace Liyanjie.Modularize.AspNetCore;

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
    public static ModularizeModuleTable AddImage(this ModularizeModuleTable moduleTable,
        Action<ImageModuleOptions> configureOptions,
        string combineRouteTemplate = "image/combine",
        string combineToGIFRouteTemplate = "image/combineToGIF",
        string concatenateRouteTemplate = "image/concatenate",
        string cropRouteTemplate = "image/crop",
        string qrcodeRouteTemplate = "image/qrcode",
        params string[] resizeRouteTemplates)
    {
        moduleTable.Services.AddSingleton<ImageCombineMiddleware>();
        moduleTable.Services.AddSingleton<ImageCombineGifMiddleware>();
        moduleTable.Services.AddSingleton<ImageConcatenateMiddleware>();
        moduleTable.Services.AddSingleton<ImageCropMiddleware>();
        moduleTable.Services.AddSingleton<ImageQRCodeMiddleware>();
        moduleTable.Services.AddSingleton<ImageResizeMiddleware>();

        var middlewares = new List<ModularizeModuleMiddleware>
        {
            new()
            {
                HttpMethods = ["POST"],
                RouteTemplate = combineRouteTemplate,
                HandlerType = typeof(ImageCombineMiddleware),
            },
            new()
            {
                HttpMethods = ["POST"],
                RouteTemplate = combineToGIFRouteTemplate,
                HandlerType = typeof(ImageCombineGifMiddleware),
            },
            new()
            {
                HttpMethods = ["POST"],
                RouteTemplate = concatenateRouteTemplate,
                HandlerType =  typeof(ImageConcatenateMiddleware),
            },
            new()
            {
                HttpMethods = ["POST"],
                RouteTemplate = cropRouteTemplate,
                HandlerType =  typeof(ImageCropMiddleware),
            },
            new()
            {
                HttpMethods = ["GET"],
                RouteTemplate = qrcodeRouteTemplate,
                HandlerType = typeof(ImageQRCodeMiddleware),
            },
        };
        if (true == resizeRouteTemplates?.Any())
            foreach (var routeTemplate in resizeRouteTemplates)
            {
                middlewares.Add(new()
                {
                    HttpMethods = ["GET"],
                    RouteTemplate = routeTemplate,
                    HandlerType = typeof(ImageResizeMiddleware),
                });
            }

        moduleTable.AddModule("ImageModule", [.. middlewares], configureOptions);

        return moduleTable;
    }
}
