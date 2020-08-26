using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SliderCodeModel
    {
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
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<(Point BlockPoint, Image OriginImage, Image BoardImage, Image BlockImage)> GenerateAsync(VerificationCodeOptions options)
        {
            await Task.FromResult(0);

            var imageFile = Directory
                .GetFiles(Path.Combine(options.RootDirectory, options.SliderCodeImageDir))
                .RandomTake(1).SingleOrDefault();
            using var imageOrigin = (Bitmap)Image.FromFile(imageFile).Resize(Width, Height, true, true);

            var s = Math.Min(Width, Height) / 12;
            var s_ = s / 2;
            var s2 = s * 2;
            var s3 = s * 3;
            var s4 = s * 4;
            var s5 = s * 5;
            var random = new Random();
            var x = random.Next(s, Width - s4);
            var y = random.Next(s, Height - s5);

            using var path = new GraphicsPath();
            path.AddBezier(x + s, y + s, x + s_, y, x + s2 + s_, y, x + s2, y + s);
            path.AddBezier(x + s2, y + s, x + s3, y + s, x + s3, y + s, x + s3, y + s2);
            path.AddBezier(x + s3, y + s2, x + s2, y + s + s_, x + s2, y + s3 + s_, x + s3, y + s3);
            path.AddBezier(x + s3, y + s3, x + s3, y + s4, x + s3, y + s4, x + s2, y + s4);
            path.AddBezier(x + s2, y + s4, x + s2 + s_, y + s3, x + s_, y + s3, x + s, y + s4);
            path.AddBezier(x + s, y + s4, x, y + s4, x, y + s4, x, y + s3);
            path.AddBezier(x, y + s3, x + s, y + s3 + s_, x + s, y + s + s_, x, y + s2);
            path.AddBezier(x, y + s2, x, y + s, x, y + s, x + s, y + s);

            var b = path.GetBounds();
            var left = (int)b.Left;
            var top = (int)b.Top;
            var width = (int)b.Width;
            var height = (int)b.Height;

            var imageBoard = (Bitmap)imageOrigin.Clone();
            var imageBlock = new Bitmap(width, height);
            for (int i = left; i < left + width; i++)
            {
                for (int j = top; j < top + height; j++)
                {
                    if (path.IsVisible(i, j))
                    {
                        imageBoard.SetPixel(i, j, Color.FromArgb(64, imageOrigin.GetPixel(i, j)));
                        imageBlock.SetPixel(i - left, j - top, imageOrigin.GetPixel(i, j));
                    }
                    else
                    {
                        imageBlock.SetPixel(i - left, j - top, Color.Transparent);
                    }
                }
            }
            
            return (new Point(x, y), (Image)imageOrigin.Clone(), imageBoard, imageBlock);
        }
    }
}
