﻿namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageCropMiddleware : IMiddleware
{
    readonly IOptions<ImageModuleOptions> options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public ImageCropMiddleware(IOptions<ImageModuleOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var options = this.options.Value;

        if (options.RequestConstrainAsync is not null)
            if (!await options.RequestConstrainAsync(context))
                return;

        var request = context.Request;

        var model = (await options.DeserializeFromRequestAsync(request, typeof(ImageCropModel))) as ImageCropModel;
        if (model is not null)
        {
            var imagePath = (await model.CropAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');

            if (options.ReturnAbsolutePath)
                imagePath = $"{request.Scheme}://{request.Host}/{imagePath}";

            await options.SerializeToResponseAsync(context.Response, imagePath);
        }
    }
}
