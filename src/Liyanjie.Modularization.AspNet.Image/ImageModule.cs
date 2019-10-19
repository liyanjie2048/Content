using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Contents.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModule : IModularizationModule
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageModule(ImageModuleOptions options)
        {
            this.options = options;
        }

        string requestMatch;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> TryMatchRequestingAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;
            if (options.TryMatchImageCombine(request))
            {
                requestMatch = "Combine";
                return true;
            }
            if (options.TryMatchImageConcatenate(request))
            {
                requestMatch = "Concatenate";
                return true;
            }
            if (options.TryMatchImageQRCode(request))
            {
                requestMatch = "QRCode";
                return true;
            }
            if (options.TryMatchImageResize(request, options.RootPath))
            {
                requestMatch = "Resize";
                return true;
            }

            return await Task.FromResult(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public async Task HandleResponsingAsync(HttpContext httpContext)
        {
            var _ = requestMatch switch
            {
                "Combine" => await CombineImagesAsync(httpContext),
                "Concatenate" => await ConcatenateImagesAsync(httpContext),
                "QRCode" => GenerateQRCode(httpContext),
                "Resize" => ResizeImage(httpContext),
                _ => false,
            };
        }

        async Task<bool> CombineImagesAsync(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;
                var model = (await ModularizationDefaults.DeserializeFromRequestAsync(request, typeof(ImageCombineModel))) as ImageCombineModel;
                var imagePath = (await model?.CombineAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');
                if (options.ReturnAbsolutePath)
                {
                    var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                    imagePath = $"//{request.Url.Host}{port}/{imagePath}";
                }

                await ModularizationDefaults.SerializeToResponseAsync(httpContext.Response, imagePath);

                return true;
            }
            catch { }

            return false;
        }

        async Task<bool> ConcatenateImagesAsync(HttpContext httpContext)
        {
            try
            {
                var request = httpContext.Request;
                var model = (await ModularizationDefaults.DeserializeFromRequestAsync(request, typeof(ImageConcatenateModel))) as ImageConcatenateModel;
                var imagePath = (await model?.ConcatenateAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');

                if (options.ReturnAbsolutePath)
                {
                    var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                    imagePath = $"//{request.Url.Host}{port}/{imagePath}";
                }

                await ModularizationDefaults.SerializeToResponseAsync(httpContext.Response, imagePath);

                return true;
            }
            catch { }

            return false;
        }

        bool GenerateQRCode(HttpContext httpContext)
        {
            var query = httpContext.Request.QueryString;
            var model = query.AllKeys
                .ToDictionary(_ => _, _ => (object)query[_])
                .BuildModel<ImageQRCodeModel>();
            var imagePath = model?.GenerateQRCode(options);
            if (!imagePath.IsNullOrEmpty())
            {
                var response = httpContext.Response;
                response.StatusCode = 200;
                response.ContentType = "image/jpg";
                response.WriteFile(Path.Combine(options.RootPath, imagePath));

                return true;
            }

            return false;
        }

        bool ResizeImage(HttpContext httpContext)
        {
            var model = new ImageResizeModel { ImagePath = httpContext.Request.Path };
            var imagePath = model.Resize(options)?.Replace(Path.DirectorySeparatorChar, '/');
            if (!imagePath.IsNullOrEmpty())
                httpContext.Response.Redirect(imagePath);

            return true;
        }
    }
}
