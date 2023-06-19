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
        if (_options.RestrictRequestAsync is not null)
        {
            if (!await _options.RestrictRequestAsync(context))
                return;
        }

        var request = context.Request;

        var model = (await _options.DeserializeFromRequestAsync(request, typeof(ImageCombineToGIFModel))) as ImageCombineToGIFModel;
        if (model is not null)
        {
            var imagePath = await model.CombineToGIFAsync(_options);

            await _options.SerializeToResponseAsync(context.Response, _options.PathToWebPath(imagePath, request));
        }
    }
}
