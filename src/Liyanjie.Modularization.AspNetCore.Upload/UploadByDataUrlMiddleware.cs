namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByDataUrlMiddleware : IMiddleware
{
    readonly static Regex _regex_DataUrl = new(@"^data\:(?<MIME>[\w-]+\/[\w-]+)\;base64\,(?<DATA>.+)");

    readonly ILogger _logger;
    readonly UploadModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadByDataUrlMiddleware(
        ILogger<UploadByDataUrlMiddleware> logger,
        IOptions<UploadModuleOptions> options)
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

        var model = new UploadModel
        {
            Files = dataUrls.Select(_ =>
            {
                var match = _regex_DataUrl.Match(_);
                if (match.Success)
                {
                    try
                    {
                        if (_options.AllowedMIMETypes.TryGetValue(match.Groups["MIME"].Value, out var extension))
                        {
                            var bytes = Convert.FromBase64String(match.Groups["DATA"].Value);
                            return new UploadModel.UploadFileModel
                            {
                                FileName = $"{Guid.NewGuid():N}.{extension}",
                                FileBytes = bytes,
                                FileLength = bytes.Length,
                            };
                        }
                    }
                    catch (Exception) { }
                }
                return new() { FileName = $"{Guid.NewGuid():N}.unknown" };
            }).ToArray(),
        };
        var result = await model.SaveAsync(_options, dir);

        await _options.SerializeToResponseAsync(context.Response, result
            .Select(_ =>
            {
                if (_.Success)
                {
                    var path = _.Path.Replace(Path.DirectorySeparatorChar, '/');
                    if (_options.ReturnAbsolutePath)
                        path = $"{request.Scheme}://{request.Host}/{path}";
                    else
                        path = $"/{path}";
                    return path;
                }
                else
                    return default;
            }));
    }
}
