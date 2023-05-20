﻿namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ClickCaptchaMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly CaptchaModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ClickCaptchaMiddleware(
        ILogger<ClickCaptchaMiddleware> logger,
        IOptions<CaptchaModuleOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_options.RestrictRequestAsync is not null)
        {
            if (!await _options.RestrictRequestAsync(context))
                return;
        }

        var dic = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object);
        var model = dic.BuildModel<ClickCaptchaModel>();
        var (points, image_Fonts, image_Board) = await model.GenerateAsync(_options);
        var output = new
        {
            Points = points,
            Image_Fonts = image_Fonts.ToDataUrl(ImageFormat.Jpeg),
            Image_Board = image_Board.ToDataUrl(ImageFormat.Jpeg),
            Size_Fonts = new { image_Fonts.Width, image_Fonts.Height },
            Size_Board = new { image_Board.Width, image_Board.Height },
        };
        image_Fonts?.Dispose();
        image_Board?.Dispose();
        await _options.SerializeToResponseAsync(context.Response, output);
    }
}
