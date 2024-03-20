namespace Liyanjie.Modularize.AspNetCore;

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
    public static ModularizeModuleTable AddCaptcha(this ModularizeModuleTable moduleTable,
        Action<CaptchaModuleOptions> configureOptions,
        string clickCodeRouteTemplate = "captcha/click",
        string puzzleCodeRouteTemplate = "captcha/puzzle",
        string sliderCodeRouteTemplate = "captcha/slider",
        string arithmeticImageCodeRouteTemplate = "captcha/arithmeticImage",
        string stringImageCodeRouteTemplate = "captcha/stringImage")
    {
        moduleTable.Services.AddSingleton<ClickCaptchaMiddleware>();
        moduleTable.Services.AddSingleton<PuzzleCaptchaMiddleware>();
        moduleTable.Services.AddSingleton<SliderCaptchaMiddleware>();
        moduleTable.Services.AddSingleton<ArithmeticCaptchaMiddleware>();
        moduleTable.Services.AddSingleton<StringCaptchaMiddleware>();

        moduleTable.AddModule("CaptchaModule",
        [
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = clickCodeRouteTemplate,
               HandlerType = typeof(ClickCaptchaMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = puzzleCodeRouteTemplate,
               HandlerType = typeof(PuzzleCaptchaMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = sliderCodeRouteTemplate,
               HandlerType = typeof(SliderCaptchaMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = arithmeticImageCodeRouteTemplate,
               HandlerType = typeof(ArithmeticCaptchaMiddleware),
           },
           new ModularizeModuleMiddleware
           {
               HttpMethods = ["GET"],
               RouteTemplate = stringImageCodeRouteTemplate,
               HandlerType = typeof(StringCaptchaMiddleware),
           },
        ], configureOptions);

        return moduleTable;
    }
}
