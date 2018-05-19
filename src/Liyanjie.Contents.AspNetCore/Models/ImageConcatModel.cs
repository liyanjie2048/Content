using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Liyanjie.Contents.AspNetCore.Extensions;
using Liyanjie.Utility;
using Newtonsoft.Json;

namespace Liyanjie.Contents.AspNetCore.Models
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
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<string> Concat(string webRootPath, Settings.Settings settings)
        {
            var fileName = $"{JsonConvert.SerializeObject(this).MD5Encode()}.{this.Width}x{this.Height}.concat.jpg";
            var filePath = Path.Combine(settings.Image.ConcatsDir, fileName).Replace(Path.DirectorySeparatorChar, '/');
            var fileAbsolutePath = Path.Combine(webRootPath, filePath).Replace('/', Path.DirectorySeparatorChar);

            if (!File.Exists(fileAbsolutePath))
            {
                var fileAbsolutePaths = this.Paths.Process(webRootPath, settings).ToList();
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
                    fileImage.CompressSave(fileAbsolutePath, settings.Image.CompressFlag);
                }
            }

            return filePath;
        }
    }
}
