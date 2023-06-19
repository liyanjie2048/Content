namespace Liyanjie.Modularization.AspNetCore;

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
    public static ModularizationModuleTable AddUpload(this ModularizationModuleTable moduleTable,
        Action<UploadModuleOptions> configureOptions,
        string uploadByFormDataRouteTemplate = "upload",
        string uploadByBase64RouteTemplate = "uploadByBase64",
        string uploadByDataUrlRouteTemplate = "uploadByDataUrl")
    {
        moduleTable.Services.AddSingleton<UploadByFormDataMiddleware>();
        moduleTable.Services.AddSingleton<UploadByBase64Middleware>();
        moduleTable.Services.AddSingleton<UploadByDataUrlMiddleware>();

        moduleTable.AddModule("UploadModule", new[]
        {
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadByFormDataRouteTemplate,
               HandlerType = typeof(UploadByFormDataMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadByBase64RouteTemplate,
               HandlerType = typeof(UploadByBase64Middleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadByDataUrlRouteTemplate,
               HandlerType = typeof(UploadByDataUrlMiddleware),
           },
        }, configureOptions);

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
    public static ModularizationModuleTable AddUploadImage(this ModularizationModuleTable moduleTable,
        Action<UploadImageModuleOptions> configureOptions,
        string uploadImageByFormDataRouteTemplate = "uploadImage",
        string uploadImageByBase64RouteTemplate = "uploadImageByBase64",
        string uploadImageByDataUrlRouteTemplate = "uploadImageByDataUrl")
    {
        moduleTable.Services.AddSingleton<UploadImageByFormDataMiddleware>();
        moduleTable.Services.AddSingleton<UploadImageByBase64Middleware>();
        moduleTable.Services.AddSingleton<UploadImageByDataUrlMiddleware>();

        moduleTable.AddModule("UploadImageModule", new[]
        {
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadImageByFormDataRouteTemplate,
               HandlerType = typeof(UploadImageByFormDataMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadImageByBase64RouteTemplate,
               HandlerType = typeof(UploadImageByBase64Middleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = uploadImageByDataUrlRouteTemplate,
               HandlerType = typeof(UploadImageByDataUrlMiddleware),
           },
        }, configureOptions);

        return moduleTable;
    }
}
