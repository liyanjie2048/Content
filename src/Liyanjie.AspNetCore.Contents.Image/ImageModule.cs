using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

using Liyanjie.AspNetCore.Contents.Core;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Liyanjie.AspNetCore.Contents.Image
{
    public class ImageModule : IContentsModule
    {
        readonly IHostingEnvironment env;
        readonly ImageOptions options;

        public ImageModule(
            IHostingEnvironment env,
            IOptions<ImageOptions> options)
        {
            this.env = env;
            this.options = options.Value;
        }

        public string Name => nameof(ImageModule);

        static readonly string pattern_path = @"\S+";
        static readonly string pattern_extension = @"(jpg|jpeg|png|gif|bmp)";
        static readonly string pattern_size = @"(?<size>\d*x\d*)";
        static readonly string pattern_color = @"(?<color>[0-9a-fA-F]{6})";
        static readonly string pattern_parameters = $@"(?<parameters>\.{pattern_size}(-{pattern_color})?)";

        string path;

        public bool MatchRequesting(HttpRequest request)
        {
            if (true
                && "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//GET请求
                && Regex.IsMatch(request.Path, $@"^{pattern_path}\.{pattern_extension}$", RegexOptions.IgnoreCase)//图片文件
                && !env.WebRootFileProvider.GetFileInfo(request.Path.Value).Exists)//文件不存在
            {
                path = request.Path.Value;
                return true;
            }

            return false;
        }

        public void HandleResponsing(HttpResponse response)
        {
            var match = Regex.Match(path, $@"^{pattern_path}{pattern_parameters}\.{pattern_extension}$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                RedirectToEmpty(response);
                return;
            }

            var matchGroups = match.Groups;
            var str_parameters = matchGroups["parameters"].Value;
            var str_size = matchGroups["size"].Value;
            var str_color = matchGroups["color"].Value;

            var fileInfo = env.WebRootFileProvider.GetFileInfo(path.Replace(str_parameters, string.Empty));
            if (!fileInfo.Exists)
            {
                RedirectToEmpty(response, str_parameters);
                return;
            }

            var size = str_size.Split('x');
            var width = int.TryParse(size[0], out var w) ? w : 0;
            var height = int.TryParse(size[1], out var h) ? h : 0;
            if (width == 0 && height == 0)
                return;

            using (var stream = fileInfo.CreateReadStream())
            {
                var image = System.Drawing.Image.FromStream(stream);
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
                    var fileAbsolutePath = Path.Combine(env.WebRootPath, path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    image.CompressSave(fileAbsolutePath, options.CompressFlag);
                }

                response.Redirect(path);
            }
        }

        void RedirectToEmpty(HttpResponse response, string parameters = null)
        {
            if (File.Exists(Path.Combine(env.WebRootPath, options.EmptyPath)))
            {
                if (string.IsNullOrEmpty(parameters))
                    response.Redirect(options.EmptyPath);
                else
                {
                    var dotIndex = options.EmptyPath.LastIndexOf(".");
                    response.Redirect($"{options.EmptyPath.Substring(0, dotIndex)}{parameters}{options.EmptyPath.Substring(dotIndex)}");
                }
            }
        }
    }
}
