using System.Net;

namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByBase64Middleware : IMiddleware
{
    readonly static Regex _regex_Base64 = new(@"^data\:(?<MIME>[\w-]+\/[\w-]+)\;base64\,(?<DATA>.+)");
    readonly ILogger _logger;
    readonly UploadModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadByBase64Middleware(
        ILogger<UploadByBase64Middleware> logger,
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

        var dir = request.Query.TryGetValue("dir", out var dir_) && !string.IsNullOrEmpty(dir_.FirstOrDefault())
            ? dir_.FirstOrDefault()
            : "temps";

        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();

        var array = JsonSerializer.Deserialize<string[]>(json);

        var model = new UploadModel
        {
            Files = array.Select(_ =>
            {
                var match = _regex_Base64.Match(_);
                if (match.Success)
                {
                    var mime = match.Groups["MIME"].Value;
                    if (_options.AllowedMIMETypes.TryGetValue(mime, out var extension))
                    {
                        var data = match.Groups["DATA"].Value;
                        var bytes = Convert.FromBase64String(data);
                        return new UploadModel.UploadFileModel
                        {
                            FileName = _options.FileNameScheme(Guid.NewGuid().ToString("N"), extension),
                            FileBytes = bytes,
                            FileLength = bytes.Length,
                        };
                    }
                }
                return new() { FileName = $"{Guid.NewGuid():N}.unknown" };
            }).ToArray(),
        };
        var paths = (await model.SaveAsync(_options, dir))
            .Select(_ =>
            {
                var path = _.Success ? _.Path.Replace(Path.DirectorySeparatorChar, '/') : _.Path;
                if (_options.ReturnAbsolutePath)
                    path = _.Success ? $"{request.Scheme}://{request.Host}/{_.Path}" : _.Path;
                return (_.Success, Path: path);
            });

        await _options.SerializeToResponseAsync(context.Response, paths.Select(_ => _.Success ? _.Path : default));
    }
}
