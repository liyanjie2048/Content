namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ClickCodeMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public ClickCodeMiddleware(IOptions<CaptchaModuleOptions> options)
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

        var dic = context.Request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object);
        var model = dic.BuildModel<ClickCaptchaModel>();
        var (fontPoints, fontImage, boardImage) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            FontPoints = fontPoints,
            FontImage = fontImage.Encode(ImageFormat.Png),
            BoardImage = boardImage.Encode(ImageFormat.Png),
        });
    }
}
