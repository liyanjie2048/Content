using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Liyanjie.Contents.AspNetCore.Extensions;
using Newtonsoft.Json;

namespace Liyanjie.Contents.AspNetCore.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageCombineModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ImageCombineItemModel[] Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webRootPath"></param>
        /// <param name="imageSetting"></param>
        /// <returns></returns>
        public async Task<string> Combine(string webRootPath, ImageSetting imageSetting)
        {
            var fileName = $"{JsonConvert.SerializeObject(this).MD5Encode()}.{this.Width}x{this.Height}.combine.jpg";
            var filePath = Path.Combine(imageSetting.CombineDir, fileName).Replace(Path.DirectorySeparatorChar, '/');
            var fileAbsolutePath = Path.Combine(webRootPath, filePath).Replace('/', Path.DirectorySeparatorChar);

            if (!File.Exists(fileAbsolutePath))
            {
                var imageAbsolutePaths = this.Items.Select(_ => _.Path).Process(webRootPath, imageSetting).ToList();
                var imagePoints = this.Items.Select(_ => (X: _.X ?? 0, Y: _.Y ?? 0)).ToList();
                var imageSizes = this.Items.Select(_ => (Width: _.Width ?? 0, Height: _.Height ?? 0)).ToList();
                var images = new List<(Point, Size, Image, bool)>();
                for (int i = 0; i < imageAbsolutePaths.Count; i++)
                {
                    var path = imageAbsolutePaths[i];
                    if (string.IsNullOrWhiteSpace(path))
                        continue;

                    var image = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image == null)
                        continue;

                    var size = imageSizes[i];
                    var point = imagePoints[i];

                    images.Add((new Point(point.X, point.Y), new Size(size.Width, size.Height), image, true));
                }

                var fileImage = new Bitmap(this.Width, this.Height);
                fileImage.Combine(images.ToArray());

                Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();

                using (fileImage)
                {
                    fileImage.CompressSave(fileAbsolutePath, imageSetting.CompressFlag);
                }
            }

            return filePath;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImageCombineItemModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Height { get; set; }
    }
}
