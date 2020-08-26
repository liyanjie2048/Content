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
        /// <param name="clickCodeRouteTemplate"></param>
        /// <param name="puzzleCodeRouteTemplate"></param>
        /// <param name="sliderCodeRouteTemplate"></param>
        /// <param name="arithmeticImageCodeRouteTemplate"></param>
        /// <param name="arithmeticSpeechCodeRouteTemplate"></param>
        /// <param name="stringImageCodeRouteTemplate"></param>
        /// <param name="stringSpeechCodeRouteTemplate"></param>
        /// <returns></returns>
        public static ModularizationModuleTable AddVerificationCode(this ModularizationModuleTable moduleTable,
            Action<VerificationCodeModuleOptions> configureOptions,
            string clickCodeRouteTemplate = "verificationCode/click",
            string puzzleCodeRouteTemplate = "verificationCode/puzzle",
            string sliderCodeRouteTemplate = "verificationCode/slider",
            string arithmeticImageCodeRouteTemplate = "verificationCode/arithmeticImage",
            string arithmeticSpeechCodeRouteTemplate = "verificationCode/arithmeticSpeech",
            string stringImageCodeRouteTemplate = "verificationCode/stringImage",
            string stringSpeechCodeRouteTemplate = "verificationCode/stringSpeech")
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
                   RouteTemplate = arithmeticSpeechCodeRouteTemplate,
                   HandlerType = typeof(ArithmeticSpeechCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringImageCodeRouteTemplate,
                   HandlerType = typeof(StringImageCodeMiddleware),
               },
               new ModularizationModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringSpeechCodeRouteTemplate,
                   HandlerType = typeof(StringSpeechCodeMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
