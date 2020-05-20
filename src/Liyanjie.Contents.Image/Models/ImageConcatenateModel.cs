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
            var fileName = options.ConcatenatedFileNameScheme.Invoke(this);
            var filePath = Path.Combine(options.ConcatenatedDirectory, fileName);
            var fileAbsolutePath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);

            if (!File.Exists(fileAbsolutePath))
            {
                var fileAbsolutePaths = ImagePaths.Process(options.RootDirectory).ToList();
                var fileImage = (await ImageHelper.FromFileOrNetworkAsync(fileAbsolutePaths[0]))?.Resize(Width, Height);
                foreach (var path in fileAbsolutePaths.Skip(1))
                {
                    using var image = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image == null)
                        continue;

                    fileImage = fileImage.Concatenate(image.Resize(Width, Height));
                }

                Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();

                using (fileImage)
                {
                    fileImage.CompressSave(fileAbsolutePath, options.CompressFlag);
                }
            }

            return filePath;
        }
    }
}
