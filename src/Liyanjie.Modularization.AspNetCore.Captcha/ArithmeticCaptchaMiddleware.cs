namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ArithmeticCaptchaMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly CaptchaModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ArithmeticCaptchaMiddleware(
        ILogger<ArithmeticCaptchaMiddleware> logger,
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
            .BuildModel<ArithmeticCaptchaModel>();
        var (code, image) = await model.GenerateAsync(_options);

        await _options.SerializeToResponseAsync(context.Response, new
        {
            Code = code,
            Image = image.Encode(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Png),
        });
    }
}
