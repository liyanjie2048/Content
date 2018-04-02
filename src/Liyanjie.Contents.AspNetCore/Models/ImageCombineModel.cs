using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<string> Combine(string webRootPath, Settings.Settings settings)
        {
            var fileName = Path.Combine(settings.Image.CombineDir, $"{JsonConvert.SerializeObject(this).MD5Encode()}.{this.Width}x{this.Height}.combine.jpg").Replace(Path.DirectorySeparatorChar, '/');

            if (!File.Exists(fileName))
            {
                var imagePaths = this.Items.Select(_ => _.Path).Process(webRootPath, settings).ToList();
                var imagePoints = this.Items.Select(_ => (X: _.X ?? 0, Y: _.Y ?? 0)).ToList();
                var imageSizes = this.Items.Select(_ => (Width: _.Width ?? 0, Height: _.Height ?? 0)).ToList();
                var images = new List<(Point, Size, Image, bool)>();
                for (int i = 0; i < imagePaths.Count; i++)
                {
                    var path = imagePaths[i];
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

                var filePhysical = Path.Combine(webRootPath, fileName).Replace('/', Path.DirectorySeparatorChar);
                Path.GetDirectoryName(filePhysical).CreateDirectory();

                using (fileImage)
                {
                    fileImage.Save(filePhysical);
                }
            }

            return fileName;
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
