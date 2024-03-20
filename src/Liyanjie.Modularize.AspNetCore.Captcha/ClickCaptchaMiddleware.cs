namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ClickCaptchaMiddleware(
    IOptions<CaptchaModuleOptions> options)
    : IMiddleware
{
    readonly CaptchaModuleOptions _options = options.Value;

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

        var model = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<ClickCaptchaModel>();
        if (model is null)
            return;

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
