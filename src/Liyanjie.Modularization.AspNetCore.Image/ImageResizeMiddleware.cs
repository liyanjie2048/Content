namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageResizeMiddleware : IMiddleware
{
    readonly IOptions<ImageModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public ImageResizeMiddleware(IOptions<ImageModuleOptions> options)
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
            if (!await options.RequestConstrainAsync.Invoke(context))
                return;

        var request = context.Request;

        var model = new ImageResizeModel { ImagePath = request.Path };
        var imagePath = model.Resize(options)?.Replace(Path.DirectorySeparatorChar, '/');
        if (!string.IsNullOrEmpty(imagePath))
            context.Response.Redirect($"/{imagePath}");

        await Task.CompletedTask;
    }
}
