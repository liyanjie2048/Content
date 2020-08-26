using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class VerificationCodeModuleTableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleTable"></param>
        /// <param name="configureOptions"></param>
        /// <param name="routeTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddVerificationCode(this ModularizationModuleTable moduleTable,
            Action<VerificationCodeModuleOptions> configureOptions,
            string clickCodeRoute = "verificationCode/click",
            string puzzleCodeRoute = "verificationCode/puzzle",
            string sliderCodeRoute = "verificationCode/slider",
            string arithmeticImageCodeRoute = "verificationCode/arithmeticImage",
            string stringImageCodeRoute = "verificationCode/stringImage")
        {
            moduleTable.Services.AddSingleton<ClickCodeMiddleware>();
            moduleTable.Services.AddSingleton<PuzzleCodeMiddleware>();
            moduleTable.Services.AddSingleton<SliderCodeMiddleware>();
            moduleTable.Services.AddSingleton<ArithmeticImageCodeMiddleware>();
            moduleTable.Services.AddSingleton<StringImageCodeMiddleware>();

            moduleTable.AddModule("VerificationCodeModule", new[]
            {
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = clickCodeRoute,
                   HandlerType = typeof(ClickCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = puzzleCodeRoute,
                   HandlerType = typeof(PuzzleCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = sliderCodeRoute,
                   HandlerType = typeof(SliderCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = arithmeticImageCodeRoute,
                   HandlerType = typeof(ArithmeticImageCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringImageCodeRoute,
                   HandlerType = typeof(StringImageCodeMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
