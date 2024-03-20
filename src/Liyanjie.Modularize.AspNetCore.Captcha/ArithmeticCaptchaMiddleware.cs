namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ArithmeticCaptchaMiddleware(
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
            .BuildModel<ArithmeticCaptchaModel>();
        if (model is null)
            return;

        var (code, image) = await model.GenerateAsync(_options);
        var output = new
        {
            Code = code,
            Image = image.ToDataUrl(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Jpeg),
            Size = new { image.Width, image.Height, },
        };
        image?.Dispose();
        await _options.SerializeToResponseAsync(context.Response, output);
    }
}
