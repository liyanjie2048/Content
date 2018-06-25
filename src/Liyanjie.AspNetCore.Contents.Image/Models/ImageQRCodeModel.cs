using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;

using ZXing;

namespace Liyanjie.AspNetCore.Contents.Image.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageQRCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        public int Height { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        public int Margin { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webRootPath"></param>
        /// <param name="imageOptions"></param>
        /// <returns></returns>
        public string CreateQRCode(string webRootPath, ImageOptions imageOptions)
        {
            var fileName = $"{this.Content.MD5Encode()}.{this.Width}x{this.Height}-{this.Margin}.jpg";
            var filePath = Path.Combine(imageOptions.QRCodesDir, fileName).Replace(Path.DirectorySeparatorChar, '/');
            var fileAbsolutePath = Path.Combine(webRootPath, filePath).Replace('/', Path.DirectorySeparatorChar);
            if (!File.Exists(fileAbsolutePath))
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = this.Width,
                        Height = this.Height,
                        Margin = this.Margin,
                    }
                };
                using (var image = writer.Write(this.Content))
                {
                    if (image != null)
                    {
                        Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();
                        image.CompressSave(fileAbsolutePath, imageOptions.CompressFlag);
                    }
                }
            }

            return filePath;
        }
    }
}
