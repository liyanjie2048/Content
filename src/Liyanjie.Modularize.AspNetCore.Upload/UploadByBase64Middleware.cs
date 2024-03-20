namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByBase64Middleware(
    IOptions<UploadModuleOptions> options)
    : IMiddleware
{
    readonly UploadModuleOptions _options = options.Value;

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

        var dir = request.Query.GetDir();

        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync();
        var files = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        if (files is null || files.Count == 0)
            return;

        var result = files.Select(_ =>
        {
            var bytes = Convert.FromBase64String(_.Value);
            var model = new UploadModel
            {
                FileName = _.Key,
                FileLength = bytes.Length,
                FileData = bytes,
            };
            if (model.TrySave(_options, dir, out var path))
                return _options.PathToWebPath(path, request);

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, result);
    }
}
