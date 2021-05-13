using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using ZXing;

namespace Liyanjie.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageQRCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="options"></param>
        /// <returns></returns>
        public string GenerateQRCode(ImageOptions options)
        {
            var fileName = options.QRCodeImageFileNameScheme.Invoke(this);
            var filePath = Path.Combine(options.QRCodeImageDirectory, fileName);
            var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
            Path.GetDirectoryName(filePhysicalPath).CreateDirectory();

            if (!File.Exists(filePhysicalPath))
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = Width,
                        Height = Height,
                        Margin = Margin,
                    }
                };

                using var image = writer.Write(Content);
                image.CompressSave(filePhysicalPath, options.CompressFlag, ImageFormat.Jpeg);
            }

            return filePath;
        }
    }
}