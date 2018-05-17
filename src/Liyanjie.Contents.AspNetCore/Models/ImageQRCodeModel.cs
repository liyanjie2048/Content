using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using Liyanjie.Contents.AspNetCore.Extensions;
using Liyanjie.Utility;
using ZXing;

namespace Liyanjie.Contents.AspNetCore.Models
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
        /// <param name="settings"></param>
        /// <returns></returns>
        public string CreateQRCode(string webRootPath, Settings.Settings settings)
        {
            var fileName = Path.Combine(settings.Image.QRCodesDir, $"{this.Content.MD5Encode()}.{this.Width}x{this.Height}-{this.Margin}.jpg").Replace(Path.DirectorySeparatorChar, '/');
            var filePhysical = Path.Combine(webRootPath, fileName).Replace('/', Path.DirectorySeparatorChar);
            if (!File.Exists(filePhysical))
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
                        Path.GetDirectoryName(filePhysical).CreateDirectory();
                        image.Save(filePhysical, ImageFormat.Jpeg);
                    }
                }
            }

            return fileName;
        }
    }
}
