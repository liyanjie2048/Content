namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageResizeMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly ImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ImageResizeMiddleware(
        ILogger<ImageResizeMiddleware> logger,
        IOptions<ImageModuleOptions> options)
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
        if (_options.RestrictRequestAsync is not null)
        {
            if (!await _options.RestrictRequestAsync(context))
                return;
        }

        var model = new ImageResizeModel { ImagePath = context.Request.Path };
        if (model.TryResize(_options, out var path))
        {
            context.Response.Redirect($"/{path.Replace(Path.DirectorySeparatorChar, '/')}");
            await context.Response.CompleteAsync();
        }
        else
            await next(context);
    }
}
