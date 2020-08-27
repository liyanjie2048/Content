using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    public class ImageCropModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get; set; }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Radius { get; set; }

        public async Task<string> CropAsync(ImageOptions options)
        {
            var fileName = options.CroppedImageFileNameScheme.Invoke(this);
            var filePath = Path.Combine(options.CombinedImageDirectory, fileName);
            var fileAbsolutePath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
            Path.GetDirectoryName(fileAbsolutePath).CreateDirectory();

            if (!File.Exists(fileAbsolutePath))
            {
                var imageAbsolutePath = ImagePath.PreProcess(options.RootDirectory);
                if (!(await ImageHelper.FromFileOrNetworkAsync(imageAbsolutePath) is Bitmap image))
                    return string.Empty;

                using var path = new GraphicsPath();
                path.AddLine(new Point(Left + Radius, Top), new Point(Left + Width - Radius, Top));
                path.AddArc(new Rectangle(Left + Width - Radius - Radius, Top, Radius * 2, Radius * 2), -90, 90);
                path.AddLine(new Point(Left + Width, Top + Radius), new Point(Left + Width, Top + Height - Radius));
                path.AddArc(new Rectangle(Left + Width - Radius - Radius, Top + Height - Radius - Radius, Radius * 2, Radius * 2), 0, 90);
                path.AddLine(new Point(Left + Width - Radius, Top + Height), new Point(Left + Radius, Top + Height));
                path.AddArc(new Rectangle(Left, Top + Height - Radius - Radius, Radius * 2, Radius * 2), 90, 90);
                path.AddLine(new Point(Left, Top + Height - Radius), new Point(Left, Top + Radius));
                path.AddArc(new Rectangle(Left, Top, Radius * 2, Radius * 2), 180, 90);

                using var output = new Bitmap(Width, Height);
                for (int i = Left; i < Left + Width; i++)
                {
                    for (int j = Top; j < Top + Height; j++)
                    {
                        output.SetPixel(i - Left, j - Top, path.IsVisible(i, j) ? image.GetPixel(i, j) : Color.Transparent);
                    }
                }

                output.CompressSave(fileAbsolutePath, options.CompressFlag, ImageFormat.Png);
            }

            return filePath;
        }
    }
}
