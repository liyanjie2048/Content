namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadImageByDataUrlMiddleware : IMiddleware
{
    readonly static Regex _regex_DataUrl = new(@"^data\:(?<MIME>[\w-]+\/[\w-]+)\;base64\,(?<DATA>.+)");

    readonly ILogger _logger;
    readonly UploadImageModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadImageByDataUrlMiddleware(
        ILogger<UploadImageByDataUrlMiddleware> logger,
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
        var dataUrls = JsonSerializer.Deserialize<string[]>(json);

        var result = dataUrls.Select(_ =>
        {
            var match = _regex_DataUrl.Match(_);
            if (match.Success && _options.AllowedMIMETypes.TryGetValue(match.Groups["MIME"].Value, out var extension))
            {
                var image = default(Image);
                try
                {
                    image = image.FromBase64String(match.Groups["DATA"].Value);
                }
                catch (Exception)
                {
                    return default;
                }

                var model = new UploadImageModel
                {
                    FileName = $"{Guid.NewGuid():N}.{extension}",
                    FileLength = 4,
                    Image = image,
                    Width = image.Width,
                    Height = image.Height,
                };
                if (model.TrySave(_options, dir, out var path))
                    return new { model.Width, model.Height, Path = _options.PathToWebPath(path, request) };
            }

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, result);
    }
}
