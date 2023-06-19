namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadImageByFormDataMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly UploadImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadImageByFormDataMiddleware(
        ILogger<UploadImageByFormDataMiddleware> logger,
        IOptions<UploadImageModuleOptions> options)
    {
        this._logger = logger;
        this._options = options?.Value ?? throw new ArgumentNullException(nameof(options));
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

        var images = request.Form.Files.Select(_ =>
        {
            var image = Image.FromStream(_.OpenReadStream());
            var model = new UploadImageModel()
            {
                FileName = Regex.Replace(_.FileName, @"\.jpg$", ".jpeg"),
                FileLength = _.Length,
                Image = image,
                Width = image.Width,
                Height = image.Height,
            };
            if (model.TrySave(_options, dir, out var path))
                return new { model.Width, model.Height, Path = _options.PathToWebPath(path, request) };

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, images);
    }
}
