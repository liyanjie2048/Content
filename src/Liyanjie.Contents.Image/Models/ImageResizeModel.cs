﻿using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Liyanjie.Contents.Models
{
    public class ImageResizeModel
    {
        public string ImagePath { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageOptions"></param>
        /// <returns></returns>
        public string Resize(ImageOptions imageOptions)
        {
            var path = ImagePath.TrimStart(new[] { '/', '\\' });
            var match = path.Match($@"^\S+(?<parameters>(-(?<color>[0-9a-fA-F]{6}))\.(?<size>\d*x\d*)?)\.(jpg|jpeg|png|gif|bmp)$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return imageOptions.EmptyPath;
            }

            var matchGroups = match.Groups;
            var str_parameters = matchGroups["parameters"].Value;
            var str_size = matchGroups["size"].Value;
            var str_color = matchGroups["color"].Value;

            var imageSourcePath = Path.Combine(imageOptions.RootPath, path.Replace(str_parameters, string.Empty));
            if (!File.Exists(imageSourcePath))
            {
                var dotIndex = imageOptions.EmptyPath.LastIndexOf(".");
                return $"{imageOptions.EmptyPath.Substring(0, dotIndex)}{str_parameters}{imageOptions.EmptyPath.Substring(dotIndex)}";
            }

            var size = str_size.Split('x');
            var width = int.TryParse(size[0], out var w) ? w : 0;
            var height = int.TryParse(size[1], out var h) ? h : 0;
            if (width == 0 && height == 0)
            {
                return null;
            }

            var image = System.Drawing.Image.FromFile(imageSourcePath);
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
            else if (width == 0)
                image = image.Resize(null, height);
            else if (height == 0)
                image = image.Resize(width, null);

            using (image)
            {
                var imageDestinationPath = Path.Combine(imageOptions.RootPath, path).Replace('/', Path.DirectorySeparatorChar);
                image.CompressSave(imageDestinationPath, imageOptions.CompressFlag);
            }

            return path;
        }
    }
}