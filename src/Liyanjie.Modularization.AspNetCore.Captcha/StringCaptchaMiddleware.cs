namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class StringCaptchaMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly CaptchaModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public StringCaptchaMiddleware(
        ILogger<StringCaptchaMiddleware> logger,
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

        var model = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<StringCaptchaModel>();
        var (code, image) = await model.GenerateAsync(_options);
        var output = new
        {
            Code = code,
            Image = image.ToDataUrl(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Jpeg),
            Size = new { image.Width, image.Height },
        };
        image?.Dispose();
        await _options.SerializeToResponseAsync(context.Response, output);
    }
}
