namespace Liyanjie.Modularize.AspNetCore;

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
    /// <param name="uploadByFormDataRouteTemplate"></param>
    /// <param name="uploadByBase64RouteTemplate"></param>
    /// <param name="uploadByDataUrlRouteTemplate"></param>
    /// <returns></returns>
    public static ModularizeModuleTable AddUpload(this ModularizeModuleTable moduleTable,
        Action<UploadModuleOptions> configureOptions,
        string uploadByFormDataRouteTemplate = "upload",
        string uploadByBase64RouteTemplate = "uploadByBase64",
        string uploadByDataUrlRouteTemplate = "uploadByDataUrl")
    {
        moduleTable.Services.AddSingleton<UploadByFormDataMiddleware>();
        moduleTable.Services.AddSingleton<UploadByBase64Middleware>();
        moduleTable.Services.AddSingleton<UploadByDataUrlMiddleware>();

        moduleTable.AddModule("UploadModule",
        [
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadByFormDataRouteTemplate,
               HandlerType = typeof(UploadByFormDataMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadByBase64RouteTemplate,
               HandlerType = typeof(UploadByBase64Middleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadByDataUrlRouteTemplate,
               HandlerType = typeof(UploadByDataUrlMiddleware),
           },
        ], configureOptions);

        return moduleTable;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleTable"></param>
    /// <param name="configureOptions"></param>
    /// <param name="uploadImageByFormDataRouteTemplate"></param>
    /// <param name="uploadImageByBase64RouteTemplate"></param>
    /// <param name="uploadImageByDataUrlRouteTemplate"></param>
    /// <returns></returns>
    public static ModularizeModuleTable AddUploadImage(this ModularizeModuleTable moduleTable,
        Action<UploadImageModuleOptions> configureOptions,
        string uploadImageByFormDataRouteTemplate = "uploadImage",
        string uploadImageByBase64RouteTemplate = "uploadImageByBase64",
        string uploadImageByDataUrlRouteTemplate = "uploadImageByDataUrl")
    {
        moduleTable.Services.AddSingleton<UploadImageByFormDataMiddleware>();
        moduleTable.Services.AddSingleton<UploadImageByBase64Middleware>();
        moduleTable.Services.AddSingleton<UploadImageByDataUrlMiddleware>();

        moduleTable.AddModule("UploadImageModule",
        [
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadImageByFormDataRouteTemplate,
               HandlerType = typeof(UploadImageByFormDataMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadImageByBase64RouteTemplate,
               HandlerType = typeof(UploadImageByBase64Middleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["POST"],
               RouteTemplate = uploadImageByDataUrlRouteTemplate,
               HandlerType = typeof(UploadImageByDataUrlMiddleware),
           },
        ], configureOptions);

        return moduleTable;
    }
}
