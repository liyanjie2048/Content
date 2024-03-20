namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageResizeMiddleware(
    IOptions<ImageModuleOptions> options)
    : IMiddleware
{
    readonly ImageModuleOptions _options = options.Value;

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
