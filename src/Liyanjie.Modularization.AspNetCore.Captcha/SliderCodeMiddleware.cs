namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class SliderCodeMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public SliderCodeMiddleware(IOptions<CaptchaModuleOptions> options)
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
            .BuildModel<SliderCaptchaModel>();
        var (blockPoint, originImage, boardImage, blockImage) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            BlockPoint = blockPoint,
            OriginImage = originImage.Encode(ImageFormat.Png),
            BoardImage = boardImage.Encode(ImageFormat.Png),
            BlockImage = blockImage.Encode(ImageFormat.Png),
        });
    }
}
