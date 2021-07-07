using System;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Modularization.AspNet
{
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
        /// <param name="arithmeticSpeechCodeRouteTemplate"></param>
        /// <param name="stringImageCodeRouteTemplate"></param>
        /// <param name="stringSpeechCodeRouteTemplate"></param>
        /// <returns></returns>
        public static ModuleTable AddCaptcha(this ModuleTable moduleTable,
            Action<CaptchaModuleOptions> configureOptions,
            string clickCodeRouteTemplate = "captcha/click",
            string puzzleCodeRouteTemplate = "captcha/puzzle",
            string sliderCodeRouteTemplate = "captcha/slider",
            string arithmeticImageCodeRouteTemplate = "captcha/arithmeticImage",
            string arithmeticSpeechCodeRouteTemplate = "captcha/arithmeticSpeech",
            string stringImageCodeRouteTemplate = "captcha/stringImage",
            string stringSpeechCodeRouteTemplate = "captcha/stringSpeech")
        {
            moduleTable.Services.AddSingleton<ClickCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<PuzzleCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<SliderCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<ArithmeticImageCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<ArithmeticSpeechCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<StringImageCaptchaMiddleware>();
            moduleTable.Services.AddSingleton<StringSpeechCaptchaMiddleware>();

            moduleTable.AddModule("Captcha(Module", new[]
            {
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = clickCodeRouteTemplate,
                   HandlerType = typeof(ClickCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = puzzleCodeRouteTemplate,
                   HandlerType = typeof(PuzzleCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = sliderCodeRouteTemplate,
                   HandlerType = typeof(SliderCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = arithmeticImageCodeRouteTemplate,
                   HandlerType = typeof(ArithmeticImageCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = arithmeticSpeechCodeRouteTemplate,
                   HandlerType = typeof(ArithmeticSpeechCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringImageCodeRouteTemplate,
                   HandlerType = typeof(StringImageCaptchaMiddleware),
               },
               new ModuleMiddleware
               {
                   HttpMethods = new[]{ "GET" },
                   RouteTemplate = stringSpeechCodeRouteTemplate,
                   HandlerType = typeof(StringSpeechCaptchaMiddleware),
               },
            }, configureOptions);

            return moduleTable;
        }
    }
}
