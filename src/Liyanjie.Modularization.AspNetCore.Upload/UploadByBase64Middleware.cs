﻿namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByBase64Middleware : IMiddleware
{
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

        var model = new UploadModel
        {
            Files = files.Select(_ =>
            {
                var bytes = Convert.FromBase64String(_.Value);
                return new UploadModel.UploadFileModel
                {
                    FileName = _.Key,
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
