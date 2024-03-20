namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class SliderCaptchaMiddleware(
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
            .BuildModel<SliderCaptchaModel>();
        if (model is null)
            return;

        var (point, image_Origin, image_Board, image_Block) = await model.GenerateAsync(_options);
        var output = new
        {
            Point = point,
            Image_Origin = image_Origin.ToDataUrl(ImageFormat.Jpeg),
            Image_Board = image_Board.ToDataUrl(ImageFormat.Png),
            Image_Block = image_Block.ToDataUrl(ImageFormat.Png),
            Size_Origin = new { image_Origin.Width, image_Origin.Height },
            Size_Board = new { image_Board.Width, image_Board.Height },
            Size_Block = new { image_Block.Width, image_Block.Height },
        };
        image_Origin?.Dispose();
        image_Board?.Dispose();
        image_Block?.Dispose();
        await _options.SerializeToResponseAsync(context.Response, output);
    }
}
