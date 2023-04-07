using System.Text.RegularExpressions;

namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class FormUploadMiddleware : IMiddleware
{
    readonly UploadModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public FormUploadMiddleware(IOptions<UploadModuleOptions> options)
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
        if (_options.RequestConstrainAsync != null)
            if (!await _options.RequestConstrainAsync(context))
                return;

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
                return new UploadModel.UploadFileModel()
                {
                    FileName = Regex.Replace(_.FileName, @"\.jpg$", ".jpeg"),
                    FileBytes = memory.ToArray(),
                };
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

        await _options.SerializeToResponseAsync(context.Response, paths.Select(_ => _.Success ? _.Path : default).ToArray());
    }
}
