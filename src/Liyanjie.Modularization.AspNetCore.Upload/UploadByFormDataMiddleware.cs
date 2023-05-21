namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByFormDataMiddleware : IMiddleware
{
    readonly UploadModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public UploadByFormDataMiddleware(IOptions<UploadModuleOptions> options)
    {
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

        var model = new UploadModel
        {
            Files = request.Form.Files.Select(_ =>
            {
                using var memory = new MemoryStream();
                _.OpenReadStream().CopyTo(memory);
                var bytes = memory.ToArray();
                return new UploadModel.UploadFileModel()
                {
                    FileName = Regex.Replace(_.FileName, @"\.jpg$", ".jpeg"),
                    FileBytes = bytes,
                    FileLength = bytes.Length,
                };
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
