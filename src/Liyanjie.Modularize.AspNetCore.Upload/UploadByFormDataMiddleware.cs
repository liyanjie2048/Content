namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByFormDataMiddleware(
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

        var result = request.Form.Files.Select(_ =>
        {
            using var memory = new MemoryStream();
            _.OpenReadStream().CopyTo(memory);
            var bytes = memory.ToArray();
            var model = new UploadModel
            {
                FileName = Regex.Replace(_.FileName, @"\.jpg$", ".jpeg"),
                FileData = bytes,
                FileLength = bytes.Length,
            };
            if (model.TrySave(_options, dir, out var path))
                return _options.PathToWebPath(path, request);

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, result);
    }
}
