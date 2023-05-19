﻿namespace Liyanjie.Modularization.AspNetCore;

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
    /// <returns></returns>
    public static ModularizationModuleTable AddUpload(this ModularizationModuleTable moduleTable,
        Action<UploadModuleOptions> configureOptions,
        string uploadByFormDataRouteTemplate = "upload",
        string uploadByBase64RouteTemplate = "uploadByBase64")
    {
        moduleTable.Services.AddSingleton<UploadByFormDataMiddleware>();
        moduleTable.Services.AddSingleton<UploadByBase64Middleware>();

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
        }, configureOptions);

        return moduleTable;
    }
}
