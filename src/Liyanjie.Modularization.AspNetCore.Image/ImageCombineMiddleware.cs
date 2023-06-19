namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageCombineMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly ImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ImageCombineMiddleware(
        ILogger<ImageCombineMiddleware> logger,
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

        var model = (await _options.DeserializeFromRequestAsync(request, typeof(ImageCombineModel))) as ImageCombineModel;
        if (model is not null)
        {
            var imagePath = await model.CombineAsync(_options);

            await _options.SerializeToResponseAsync(context.Response, _options.PathToWebPath(imagePath, request));
        }
    }
}
