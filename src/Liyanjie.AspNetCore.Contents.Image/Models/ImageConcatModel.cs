using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Liyanjie.AspNetCore.Contents.Image.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageConcatModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] Paths { get; set; }

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
        /// <param name="webRootPath"></param>
        /// <param name="imageOptions"></param>
        /// <returns></returns>
        public async Task<string> Concat(string webRootPath, ImageOptions imageOptions)
        {
            var fileName = $"{JsonConvert.SerializeObject(this).MD5Encode()}.{this.Width}x{this.Height}.concat.jpg";
            var filePath = Path.Combine(imageOptions.ConcatsDir, fileName).Replace(Path.DirectorySeparatorChar, '/');
            var fileAbsolutePath = Path.Combine(webRootPath, filePath).Replace('/', Path.DirectorySeparatorChar);

            if (!File.Exists(fileAbsolutePath))
            {
                var fileAbsolutePaths = this.Paths.Process(webRootPath, imageOptions).ToList();
                var fileImage = (await ImageHelper.FromFileOrNetworkAsync(fileAbsolutePaths[0]))?.Resize(this.Width, this.Height);
                foreach (var path in fileAbsolutePaths.Skip(1))
                {
                    var image = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image == null)
                        continue;

                    using (image)
                    {
                        fileImage = fileImage.Concat(image.Resize(this.Width, this.Height));
                    }
                }

                Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();

                using (fileImage)
                {
                    fileImage.CompressSave(fileAbsolutePath, imageOptions.CompressFlag);
                }
            }

            return filePath;
        }
    }
}
