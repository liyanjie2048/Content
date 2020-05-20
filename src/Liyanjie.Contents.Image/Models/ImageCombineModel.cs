using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
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
        /// <param name="imageOptions"></param>
        /// <returns></returns>
        public async Task<string> CombineAsync(ImageOptions imageOptions)
        {
            var fileName = imageOptions.CombinedFileNameScheme.Invoke(this);
            var filePath = Path.Combine(imageOptions.CombinedDirectory, fileName);
            var fileAbsolutePath = Path.Combine(imageOptions.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);

            if (!File.Exists(fileAbsolutePath))
            {
                var imageAbsolutePaths = Items.Select(_ => _.ImagePath).Process(imageOptions.RootDirectory).ToList();
                var imagePoints = Items.Select(_ => (X: _.X ?? 0, Y: _.Y ?? 0)).ToList();
                var imageSizes = Items.Select(_ => (Width: _.Width ?? 0, Height: _.Height ?? 0)).ToList();
                var images = new List<(Point, Size, System.Drawing.Image)>();
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

                    images.Add((new Point(point.X, point.Y), new Size(size.Width, size.Height), image));
                }

                var fileImage = new Bitmap(Width, Height);
                fileImage.Combine(images.ToArray());

                Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();

                using (fileImage)
                {
                    fileImage.CompressSave(fileAbsolutePath, imageOptions.CompressFlag);
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
        public string ImagePath { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{ImagePath}|{X},{Y}|{Width}x{Height}";
    }
}
