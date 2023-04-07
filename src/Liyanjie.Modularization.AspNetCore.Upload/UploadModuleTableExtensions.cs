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
    /// <param name="formUploadRouteTemplate"></param>
    /// <param name="base64UploadRouteTemplate"></param>
    /// <returns></returns>
    public static ModularizationModuleTable AddUpload(this ModularizationModuleTable moduleTable,
        Action<UploadModuleOptions> configureOptions,
        string formUploadRouteTemplate = "upload",
        string base64UploadRouteTemplate = "base64upload")
    {
        moduleTable.Services.AddSingleton<FormUploadMiddleware>();
        moduleTable.Services.AddSingleton<Base64UploadMiddleware>();

        moduleTable.AddModule("UploadModule", new[]
        {
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = formUploadRouteTemplate,
               HandlerType = typeof(FormUploadMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "POST" },
               RouteTemplate = base64UploadRouteTemplate,
               HandlerType = typeof(Base64UploadMiddleware),
           },
        }, configureOptions);

        return moduleTable;
    }
}
