namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadImageByDataUrlMiddleware(
    IOptions<UploadImageModuleOptions> options)
    : IMiddleware
{
    readonly UploadImageModuleOptions _options = options.Value;
    readonly static Regex _regex_DataUrl = new(@"^data\:(?<MIME>[\w-]+\/[\w-]+)\;base64\,(?<DATA>.+)");

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
            var match = _regex_DataUrl.Match(_.Value);
            if (match.Success && _options.AllowedMIMETypes.TryGetValue(match.Groups["MIME"].Value, out var extension))
            {
                var image = default(Image);
                try
                {
                    image = ImageX.FromBase64String(match.Groups["DATA"].Value);
                }
                catch (Exception)
                {
                    return default;
                }

                var model = new UploadImageModel
                {
                    FileName = $"{Guid.NewGuid():N}.{extension}",
                    FileLength = 4,
                    Image = image!,
                    Width = image!.Width,
                    Height = image!.Height,
                };
                if (model.TrySave(_options, dir, out var path))
                    return new { model.Width, model.Height, Path = _options.PathToWebPath(path, request) };
            }

            return default;
        });

        await _options.SerializeToResponseAsync(context.Response, result);
    }
}
