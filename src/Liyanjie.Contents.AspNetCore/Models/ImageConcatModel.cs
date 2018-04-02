using System;
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
            var fileName = Path.Combine(settings.Image.ConcatsDir, $"{JsonConvert.SerializeObject(this).MD5Encode()}.{this.Width}x{this.Height}.concat.jpg").Replace(Path.DirectorySeparatorChar, '/');

            if (!File.Exists(fileName))
            {
                var filePaths = this.Paths.Process(webRootPath, settings).ToList();
                var fileImage = (await ImageHelper.FromFileOrNetworkAsync(filePaths[0]))?.Resize(this.Width, this.Height);
                foreach (var path in filePaths.Skip(1))
                {
                    var image = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image == null)
                        continue;

                    using (image)
                    {
                        fileImage = fileImage.Concat(image.Resize(this.Width, this.Height));
                    }
                }

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
}
