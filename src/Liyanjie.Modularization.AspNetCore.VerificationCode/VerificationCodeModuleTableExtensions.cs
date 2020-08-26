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
        /// <param name="clickCodeRouteTemplate"></param>
        /// <param name="puzzleCodeRouteTemplate"></param>
        /// <param name="sliderCodeRouteTemplate"></param>
        /// <param name="arithmeticImageCodeRouteTemplate"></param>
        /// <param name="stringImageCodeRouteTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddVerificationCode(this ModularizationModuleTable moduleTable,
            Action<VerificationCodeModuleOptions> configureOptions,
            string clickCodeRouteTemplate = "verificationCode/click",
            string puzzleCodeRouteTemplate = "verificationCode/puzzle",
            string sliderCodeRouteTemplate = "verificationCode/slider",
            string arithmeticImageCodeRouteTemplate = "verificationCode/arithmeticImage",
            string stringImageCodeRouteTemplate = "verificationCode/stringImage")
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
}
