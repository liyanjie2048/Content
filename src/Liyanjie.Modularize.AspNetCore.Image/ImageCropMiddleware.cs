﻿namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageCropMiddleware(
    IOptions<ImageModuleOptions> options)
    : IMiddleware
{
    readonly ImageModuleOptions _options = options.Value;

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

        var model = (await _options.DeserializeFromRequestAsync(request, typeof(ImageCropModel))) as ImageCropModel;
        if (model is not null)
        {
            var imagePath = await model.CropAsync(_options);

            await _options.SerializeToResponseAsync(context.Response, _options.PathToWebPath(imagePath, request));
        }
    }
}
