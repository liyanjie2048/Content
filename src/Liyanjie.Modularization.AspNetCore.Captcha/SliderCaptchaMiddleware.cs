namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class SliderCaptchaMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly CaptchaModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public SliderCaptchaMiddleware(
        ILogger<SliderCaptchaMiddleware> logger,
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

        var model = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<SliderCaptchaModel>();
        var (point, image_Origin, image_Board, image_Block) = await model.GenerateAsync(_options);

        await _options.SerializeToResponseAsync(context.Response, new
        {
            Point = point,
            Image_Origin = image_Origin.Encode(ImageFormat.Png),
            Image_Board = image_Board.Encode(ImageFormat.Png),
            Image_Block = image_Block.Encode(ImageFormat.Png),
        });
    }
}
