namespace Liyanjie.Modularization.AspNetCore;

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
        if (_options.RequestConstrainAsync is not null)
        {
            if (!await _options.RequestConstrainAsync(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.CompleteAsync();
                return;
            }
        }

        var dic = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object);
        var model = dic.BuildModel<ClickCaptchaModel>();
        var (points, image_Fonts, image_Board) = await model.GenerateAsync(_options);

        await _options.SerializeToResponseAsync(context.Response, new
        {
            Points = points,
            Image_Fonts = image_Fonts.Encode(ImageFormat.Png),
            Image_Board = image_Board.Encode(ImageFormat.Png),
        });
    }
}
