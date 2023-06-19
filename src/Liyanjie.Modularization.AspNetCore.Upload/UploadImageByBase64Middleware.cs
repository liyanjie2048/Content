namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadImageByBase64Middleware : IMiddleware
{
    readonly ILogger _logger;
    readonly UploadImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadImageByBase64Middleware(
        ILogger<UploadImageByBase64Middleware> logger,
        IOptions<UploadImageModuleOptions> options)
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

        var dir = request.Query.TryGetValue("dir", out var dir_) && !string.IsNullOrEmpty(dir_.FirstOrDefault())
            ? dir_.FirstOrDefault()
            : "temps";

        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();
        var files = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        var result = files.Select(_ =>
        {
            var image = default(Image);
            try
            {
                image = image.FromBase64String(_.Value);
            }
            catch (Exception)
            {
                return default;
            }

            var model = new UploadImageModel
            {
                FileName = _.Key,
                FileLength = 4,
                Image = image,
                Width = image.Width,
                Height = image.Height,
            };
            if (model.TrySave(_options, dir, out var path))
                return new { model.Width, model.Height, Path = _options.PathToWebPath(path, request) };

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, result);
    }
}
