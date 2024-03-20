namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public static class ExploreModuleTableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleTable"></param>
    /// <param name="configureOptions"></param>
    /// <param name="routeTemplate"></param>
    /// <returns></returns>
    public static ModularizeModuleTable AddExplore(this ModularizeModuleTable moduleTable,
        Action<ExploreModuleOptions> configureOptions,
        string routeTemplate = "explore")
    {
        moduleTable.Services.AddSingleton<ExploreMiddleware>();

        moduleTable.AddModule("ExploreModule",
        [
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = routeTemplate,
               HandlerType = typeof(ExploreMiddleware),
           },
        ], configureOptions);

        return moduleTable;
    }
}
