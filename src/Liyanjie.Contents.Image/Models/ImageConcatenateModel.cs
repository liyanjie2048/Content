using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageConcatenateModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] ImagePaths { get; set; }

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
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<string> ConcatenateAsync(ImageOptions options)
        {
            var fileName = options.ConcatenatedImageFileNameScheme.Invoke(this);
            var filePath = Path.Combine(options.ConcatenatedImageDirectory, fileName);
            var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
            Path.GetDirectoryName(filePhysicalPath).CreateDirectory();

            if (!File.Exists(filePhysicalPath))
            {
                var fileAbsolutePaths = ImagePaths
                    .Where(_ => !_.IsNullOrWhiteSpace())
                    .Select(_ => _.PreProcess(options.RootDirectory))
                    .ToList();
                var fileImage = (await ImageHelper.FromFileOrNetworkAsync(fileAbsolutePaths[0]))?.Resize(Width, Height);
                foreach (var path in fileAbsolutePaths.Skip(1))
                {
                    using var image = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image == null)
                        continue;

                    fileImage = fileImage.Concatenate(image.Resize(Width, Height));
                }

                using (fileImage)
                    fileImage.CompressSave(filePhysicalPath, options.CompressFlag, ImageFormat.Jpeg);
            }

            return filePath;
        }
    }
}
