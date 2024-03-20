namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageQRCodeMiddleware(
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

        var request = context.Request;
        var response = context.Response;

        var model = request.Query
            .ToDictionary(_ => _.Key.ToLower(), _ => _.Value.FirstOrDefault() as object)
            .BuildModel<ImageQRCodeModel>();
        if (model is null)
            return;

        var imagePath = await model.GenerateQRCodeAsync(_options);

        response.StatusCode = 200;
        response.ContentType = "image/svg+xml";
        using var stream = File.OpenRead(Path.Combine(_options.RootDirectory, imagePath));
        await stream.CopyToAsync(response.Body);
    }
}
