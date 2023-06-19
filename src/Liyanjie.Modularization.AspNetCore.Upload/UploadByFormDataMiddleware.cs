﻿namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadByFormDataMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly UploadModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public UploadByFormDataMiddleware(
        ILogger<UploadImageByFormDataMiddleware> logger,
        IOptions<UploadModuleOptions> options)
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
