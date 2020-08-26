using System;

namespace Liyanjie.Modularization.AspNet
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
            string arithmeticSpeechCodeRoute = "verificationCode/arithmeticSpeech",
            string stringImageCodeRoute = "verificationCode/stringImage",
            string stringSpeechCodeRoute = "verificationCode/stringSpeech")
        {
            moduleTable.RegisterServiceType?.Invoke(typeof(ClickCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(PuzzleCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(SliderCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ArithmeticImageCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(ArithmeticSpeechCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(StringImageCodeMiddleware), "Singleton");
            moduleTable.RegisterServiceType?.Invoke(typeof(StringSpeechCodeMiddleware), "Singleton");

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
                   RouteTemplate = arithmeticSpeechCodeRoute,
                   HandlerType = typeof(ArithmeticSpeechCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringImageCodeRoute,
                   HandlerType = typeof(StringImageCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringSpeechCodeRoute,
                   HandlerType = typeof(StringSpeechCodeMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
