using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Liyanjie.Contents.AspNetCore.Extensions;
using Liyanjie.Contents.AspNetCore.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.Contents.AspNetCore.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostingEnvironment env;
        private readonly ImageSetting setting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="env"></param>
        /// <param name="options"></param>
        public ImageMiddleware(
            RequestDelegate next,
            IHostingEnvironment env,
            IOptions<Settings.Settings> options)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.env = env ?? throw new ArgumentNullException(nameof(env));

            this.setting = options.Value.Image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (MatchRequesting(httpContext.Request))
            {
                await HandleResposing(httpContext.Response, httpContext.Request.Path.Value);
                return;
            }

            await next(httpContext);
        }

        bool MatchRequesting(HttpRequest request)
        {
            if (true
                && "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//GET请求
                && Regex.IsMatch(request.Path, @"^\S+\.(jpg|jpeg|gif|bmp|png)$", RegexOptions.IgnoreCase)//图片文件
                && !env.WebRootFileProvider.GetFileInfo(request.Path.Value).Exists)//文件不存在
                return true;

            return false;
        }

        async Task HandleResposing(HttpResponse response, string path)
        {
            var regex = @"^\S+(?<parameters>(\.(?<size>\d+x\d+))(-(?<color>[0-9a-fA-F]{6}))?)\.(jpeg|jpg|gif|bmp|png)$";
            var match = Regex.Match(path, regex, RegexOptions.IgnoreCase);
            if (!match.Success)
                RedirectToEmpty(response);

            try
            {
                var matchGroups = match.Groups;
                var str_parameters = matchGroups["parameters"].Value;
                var str_size = matchGroups["size"].Value;
                var str_color = matchGroups["color"].Value;

                var fileInfo = env.WebRootFileProvider.GetFileInfo(path.Replace(str_parameters, string.Empty));
                if (!fileInfo.Exists)
                    RedirectToEmpty(response, str_parameters);

                var size = str_size.Split('x');
                var width = size[0].ToInt();
                var height = size[1].ToInt();
                if (width == 0 && height == 0)
                    return;

                using (var stream = fileInfo.CreateReadStream())
                {
                    var image = Image.FromStream(stream);
                    if (width > 0 && height > 0)
                    {
                        image = image.Resize(width, height);
                        if (!string.IsNullOrEmpty(str_color))
                        {
                            var r = str_color.Substring(0, 2).FromRadix16();
                            var g = str_color.Substring(2, 2).FromRadix16();
                            var b = str_color.Substring(4, 2).FromRadix16();
                            var tmp = new Bitmap(width, height);
                            tmp.Clear(Color.FromArgb(r, g, b));
                            tmp.Combine((new Point((width - image.Width) / 2, (height - image.Height) / 2), new Size(width, height), image, true));
                            image = tmp;
                        }
                    }
                    else
                        if (width == 0)
                        image = image.Resize(null, height);
                    else if (height == 0)
                        image = image.Resize(width, null);

                    using (image)
                    {
                        image.CompressSave(path, (long)(setting.CompressFlag * 100));
                    }

                    await response.SendFileAsync(path);
                }
            }
            catch
            {
                RedirectToEmpty(response);
            }
        }

        void RedirectToEmpty(HttpResponse response, string parameters = null)
        {
            if (File.Exists(Path.Combine(env.WebRootPath, setting.EmptyPath)))
            {
                if (string.IsNullOrEmpty(parameters))
                    response.Redirect(setting.EmptyPath);
                else
                {
                    var dotIndex = setting.EmptyPath.LastIndexOf(".");
                    response.Redirect($"{setting.EmptyPath.Substring(0, dotIndex)}{parameters}{setting.EmptyPath.Substring(dotIndex)}");
                }
            }
        }
    }
}
