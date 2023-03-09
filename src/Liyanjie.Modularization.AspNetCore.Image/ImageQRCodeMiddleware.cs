namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageQRCodeMiddleware : IMiddleware
{
    readonly IOptions<ImageModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public ImageQRCodeMiddleware(IOptions<ImageModuleOptions> options)
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

        if (options.RequestConstrainAsync is not null)
            if (!await options.RequestConstrainAsync(context))
                return;

        var request = context.Request;
        var response = context.Response;

        var model = request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<ImageQRCodeModel>();
        var imagePath = await model.GenerateQRCodeAsync(options);

        response.StatusCode = 200;
        response.ContentType = "image/svg+xml";
        using var stream = File.OpenRead(Path.Combine(options.RootDirectory, imagePath));
        await stream.CopyToAsync(response.Body);
    }
}
