namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public static class CaptchaModuleTableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="moduleTable"></param>
    /// <param name="configureOptions"></param>
    /// <param name="clickCodeRouteTemplate"></param>
    /// <param name="puzzleCodeRouteTemplate"></param>
    /// <param name="sliderCodeRouteTemplate"></param>
    /// <param name="arithmeticImageCodeRouteTemplate"></param>
    /// <param name="stringImageCodeRouteTemplate"></param>
    /// <returns></returns>
    public static ModularizationModuleTable AddCaptcha(this ModularizationModuleTable moduleTable,
        Action<CaptchaModuleOptions> configureOptions,
        string clickCodeRouteTemplate = "captcha/click",
        string puzzleCodeRouteTemplate = "captcha/puzzle",
        string sliderCodeRouteTemplate = "captcha/slider",
        string arithmeticImageCodeRouteTemplate = "captcha/arithmeticImage",
        string stringImageCodeRouteTemplate = "captcha/stringImage")
    {
        moduleTable.Services.AddSingleton<ClickCodeMiddleware>();
        moduleTable.Services.AddSingleton<PuzzleCodeMiddleware>();
        moduleTable.Services.AddSingleton<SliderCodeMiddleware>();
        moduleTable.Services.AddSingleton<ArithmeticImageCodeMiddleware>();
        moduleTable.Services.AddSingleton<StringImageCodeMiddleware>();

        moduleTable.AddModule("CaptchaModule", new[]
        {
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "GET" },
               RouteTemplate = clickCodeRouteTemplate,
               HandlerType = typeof(ClickCodeMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "GET" },
               RouteTemplate = puzzleCodeRouteTemplate,
               HandlerType = typeof(PuzzleCodeMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "GET" },
               RouteTemplate = sliderCodeRouteTemplate,
               HandlerType = typeof(SliderCodeMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "GET" },
               RouteTemplate = arithmeticImageCodeRouteTemplate,
               HandlerType = typeof(ArithmeticImageCodeMiddleware),
           },
           new ModularizationModuleMiddleware
           {
               HttpMethods = new[]{ "GET" },
               RouteTemplate = stringImageCodeRouteTemplate,
               HandlerType = typeof(StringImageCodeMiddleware),
           },
        }, configureOptions);

        return moduleTable;
    }
}
