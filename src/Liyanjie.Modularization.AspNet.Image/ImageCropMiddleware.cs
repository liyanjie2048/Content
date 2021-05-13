using System.IO;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Content.Models;

namespace Liyanjie.Modularization.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageCropMiddleware
    {
        readonly ImageModuleOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ImageCropMiddleware(ImageModuleOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            if (options.RequestConstrainAsync != null)
                if (!await options.RequestConstrainAsync.Invoke(context))
                    return;

            var request = context.Request;
            
            var model = (await options.DeserializeFromRequestAsync(request, typeof(ImageCropModel))) as ImageCropModel;
            var imagePath = (await model?.CropAsync(options))?.Replace(Path.DirectorySeparatorChar, '/');
            if (options.ReturnAbsolutePath)
            {
                var port = request.Url.IsDefaultPort ? null : $":{request.Url.Port}";
                imagePath = $"{request.Url.Scheme}://{request.Url.Host}{port}/{imagePath}";
            }

            await options.SerializeToResponseAsync(context.Response, imagePath);

            context.Response.End();
        }
    }
}
