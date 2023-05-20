namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class PuzzleCaptchaMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly CaptchaModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public PuzzleCaptchaMiddleware(
        ILogger<PuzzleCaptchaMiddleware> logger,
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
            .BuildModel<PuzzleCaptchaModel>();
        var (indexes, image_Origin, image_Blocks) = await model.GenerateAsync(_options);
        var output = new
        {
            Indexes = indexes,
            Image_Origin = image_Origin.ToDataUrl(ImageFormat.Jpeg),
            Image_Blocks = image_Blocks.Select(_ => _.ToDataUrl(ImageFormat.Jpeg)),
            Size_Origin = new { image_Origin.Width, image_Origin.Height },
            Size_Blocks = new { image_Origin.Width, image_Origin.Height },
        };
        image_Origin?.Dispose();
        foreach (var image_Block in image_Blocks)
        {
            image_Block?.Dispose();
        }
        await _options.SerializeToResponseAsync(context.Response, output);
    }
}
