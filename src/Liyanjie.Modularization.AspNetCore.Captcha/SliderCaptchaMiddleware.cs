namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class SliderCaptchaMiddleware : IMiddleware
{
    readonly IOptions<CaptchaModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public SliderCaptchaMiddleware(IOptions<CaptchaModuleOptions> options)
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
        var (point, image_Origin, image_Board, image_Block) = await model.GenerateAsync(options);

        await options.SerializeToResponseAsync(context.Response, new
        {
            Point = point,
            Image_Origin = image_Origin.Encode(ImageFormat.Png),
            Image_Board = image_Board.Encode(ImageFormat.Png),
            Image_Block = image_Block.Encode(ImageFormat.Png),
        });
    }
}
