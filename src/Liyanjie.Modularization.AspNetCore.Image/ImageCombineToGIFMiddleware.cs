namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageCombineToGIFMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly ImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ImageCombineToGIFMiddleware(
        ILogger<ImageCombineToGIFMiddleware> logger,
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
        if (_options.RequestConstrainAsync is not null)
        {
            if (!await _options.RequestConstrainAsync(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.CompleteAsync();
                return;
            }
        }

        var request = context.Request;

        var model = (await _options.DeserializeFromRequestAsync(request, typeof(ImageCombineToGIFModel))) as ImageCombineToGIFModel;
        if (model is not null)
        {
            var imagePath = (await model.CombineToGIFAsync(_options))?.Replace(Path.DirectorySeparatorChar, '/');

            if (_options.ReturnAbsolutePath)
                imagePath = $"{request.Scheme}://{request.Host}/{imagePath}";

            await _options.SerializeToResponseAsync(context.Response, imagePath);
        }
    }
}
