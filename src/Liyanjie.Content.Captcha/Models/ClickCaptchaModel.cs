using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ClickCaptchaModel
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
        public string String { get; set; } = "Hello,World";
        /// <summary>
        /// 
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<(Point[] FontPoints, Image FontImage, Image BoardImage)> GenerateAsync(CaptchaOptions options)
        {
            await Task.FromResult(0);

            var imageFile = Directory
                .GetFiles(Path.Combine(options.RootDirectory, options.ClickCodeImageDir))
                .RandomTake(1).SingleOrDefault();
            using var imageOrigin = Image.FromFile(imageFile).Resize(Width, Height, true, true);

            var strs = String.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var points = new List<Point>(strs.Length);
            var imageFont = new Bitmap(FontSize / 2 * String.Length + (int)(FontSize / 2 * 1.5D), (int)(FontSize / 2 * 1.5D));
            using var graphicsFont = Graphics.FromImage(imageFont);
            graphicsFont.Clear(CaptchaHelper.RandomColor(false));
            //绘制干扰线
            for (int i = 0; i < 4; i++)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var x = random.Next(imageFont.Width);
                var y = random.Next(imageFont.Height);
                var x1 = random.Next(imageFont.Width);
                var y1 = random.Next(imageFont.Height);
                var x2 = random.Next(imageFont.Width);
                var y2 = random.Next(imageFont.Height);
                var x3 = random.Next(imageFont.Width);
                var y3 = random.Next(imageFont.Height);
                using var pen = new Pen(CaptchaHelper.RandomColor(true));
                graphicsFont.DrawBezier(pen, x, y, x1, y1, x2, y2, x3, y3);
            }

            var imageBoard = (Image)imageOrigin.Clone();
            using var graphicsBoard = Graphics.FromImage(imageBoard);

            //绘制文字
            for (int i = 0; i < strs.Length; i++)
            {
                var item = strs[i];
                var random = new Random(Guid.NewGuid().GetHashCode());
                using var pen = new Pen(CaptchaHelper.RandomColor());

                using var font1 = new Font(options.FontFamilies[random.Next(options.FontFamilies.Length)], FontSize / 2, options.FontStyles[random.Next(options.FontStyles.Length)]);
                graphicsFont.DrawString(item, font1, pen.Brush, FontSize * i, 0);

                var point = new Point(random.Next(0, Width - item.Length * FontSize - FontSize), random.Next(0, Height - FontSize - FontSize));
                using var font2 = new Font(options.FontFamilies[random.Next(options.FontFamilies.Length)], FontSize, options.FontStyles[random.Next(options.FontStyles.Length)]);
                graphicsBoard.DrawString(item, font2, pen.Brush, point);

                points.Add(point);
            }

            return (points.ToArray(), imageFont, imageBoard);
        }
    }
}
