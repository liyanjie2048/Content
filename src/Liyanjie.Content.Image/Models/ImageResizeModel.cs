using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Liyanjie.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageResizeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public string Resize(ImageOptions options)
        {
            var path = ImagePath.TrimStart(new[] { '/', '\\' });
            var match = path.Match(options.ResizePathPattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return options.EmptyImagePath;
            }

            var str_parameters = match.Groups["parameters"].Value;
            var str_size = match.Groups["size"].Value;
            var str_color = match.Groups["color"].Value;

            var imageSourcePath = Path.Combine(options.RootDirectory, path.Replace(str_parameters, string.Empty));
            if (!File.Exists(imageSourcePath))
            {
                var dotIndex = options.EmptyImagePath.LastIndexOf(".");
                return $"{options.EmptyImagePath.Substring(0, dotIndex)}{str_parameters}{options.EmptyImagePath.Substring(dotIndex)}";
            }

            var size = str_size.Split('x');
            var width = int.TryParse(size[0], out var w) ? w : 0;
            var height = int.TryParse(size[1], out var h) ? h : 0;
            if (width == 0 && height == 0)
                return null;

            var image = Image.FromFile(imageSourcePath);
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
                    tmp.Combine((new Point((width - image.Width) / 2, (height - image.Height) / 2), new Size(image.Width, image.Height), image));
                    image = tmp;
                }
            }
            else if (width == 0)
                image = image.Resize(null, height);
            else if (height == 0)
                image = image.Resize(width, null);

            var imageDestinationPath = Path.Combine(options.RootDirectory, path.Replace('/', Path.DirectorySeparatorChar));
            using (image)
                image.CompressSave(imageDestinationPath, options.CompressFlag);

            return path;
        }
    }
}
