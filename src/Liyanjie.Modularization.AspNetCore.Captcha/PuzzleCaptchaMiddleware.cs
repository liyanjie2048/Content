﻿namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class PuzzleCaptchaMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public PuzzleCaptchaMiddleware(IOptions<CaptchaModuleOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var options = this.options.Value;

        if (options.RequestConstrainAsync != null)
            if (!await options.RequestConstrainAsync(context))
                return;

        var model = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<PuzzleCaptchaModel>();
        var (indexes, image_Origin, image_Blocks) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            Indexes = indexes,
            Image_Origin = image_Origin.Encode(ImageFormat.Png),
            Image_Blocks = image_Blocks.Select(_ => _.Encode(ImageFormat.Png)),
        });
    }
}