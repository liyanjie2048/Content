namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ClickCaptchaMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public ClickCaptchaMiddleware(IOptions<CaptchaModuleOptions> options)
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
        var (points, image_Fonts, image_Board) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            Points = points,
            Image_Fonts = image_Fonts.Encode(ImageFormat.Png),
            Image_Board = image_Board.Encode(ImageFormat.Png),
        });
    }
}
