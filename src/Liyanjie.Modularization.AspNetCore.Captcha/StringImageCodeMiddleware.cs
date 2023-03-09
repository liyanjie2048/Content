namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class StringImageCodeMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public StringImageCodeMiddleware(IOptions<CaptchaModuleOptions> options)
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
            .BuildModel<StringImageCaptchaModel>();
        var (code, image) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            Code = code,
            Image = image.Encode(model.Image.GenerateGif ? ImageFormat.Gif : ImageFormat.Png),
        });
    }
}
