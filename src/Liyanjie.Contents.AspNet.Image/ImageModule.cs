using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Liyanjie.Contents.Models;
using System.Collections.Generic;
using System.Linq;

namespace Liyanjie.Contents.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModule : IContentsModule
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
        public bool TryMatchRequesting(HttpContext httpContext)
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

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public void HandleResponsing(HttpContext httpContext)
        {
            var _ = requestMatch switch
            {
                "Combine" => CombineImages(httpContext),
                "Concatenate" => ConcatenateImages(httpContext),
                "QRCode" => GenerateQRCode(httpContext),
                "Resize" => ResizeImage(httpContext),
                _ => false,
            };
        }

        bool CombineImages(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;

            try
            {
                using var streamReader = new StreamReader(request.InputStream);
                var json = streamReader.ReadToEnd();
                var model = ContentsDefaults.JsonDeserialize(json, typeof(ImageCombineModel)) as ImageCombineModel;
                var task = model?.CombineAsync(options);
                task?.Wait();
                var imagePath = task?.Result?.Replace(Path.DirectorySeparatorChar, '/');

                if (options.ReturnAbsolutePath)
                {
                    var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                    imagePath = $"//{request.Url.Host}{port}/{imagePath}";
                }

                response.StatusCode = 200;
                response.ContentType = "application/json";
                response.Write(ContentsDefaults.JsonSerialize(imagePath));

                return true;
            }
            catch { }

            return false;
        }

        bool ConcatenateImages(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;

            try
            {
                using var streamReader = new StreamReader(request.InputStream);
                var json = streamReader.ReadToEnd();
                var model = ContentsDefaults.JsonDeserialize(json, typeof(ImageConcatenateModel)) as ImageConcatenateModel;
                var task = model?.ConcatenateAsync(options);
                task?.Wait();
                var imagePath = task?.Result?.Replace(Path.DirectorySeparatorChar, '/');

                if (options.ReturnAbsolutePath)
                {
                    var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                    imagePath = $"//{request.Url.Host}{port}/{imagePath}";
                }

                response.StatusCode = 200;
                response.ContentType = "application/json";
                response.Write(ContentsDefaults.JsonSerialize(imagePath));

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
