using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Liyanjie.Contents.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class _ImageModel
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
        public int FontSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool GenerateGif { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public Image Generate(IEnumerable<string> strings, VerificationCodeOptions options)
        {
            var bg = new Random(Guid.NewGuid().GetHashCode()).NextBool();
            var bgColor = VerificationCodeHelper.RandomColor(bg);

            var image = new Bitmap(Width, Height);

            using var graphics = Graphics.FromImage(image);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;//设置高质量插值法
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
            graphics.Clear(bgColor);//设置背景

            //绘制字符
            var charCount = strings.Sum(_ => _.Length);
            var charWidth = (float)image.Width / (charCount + 3);//字符所占宽度
            var charLeft = FontSize / 2F;
            if (!GenerateGif)
            {
                //绘制干扰线
                for (int i = 0; i < 6; i++)
                {
                    var random = new Random(Guid.NewGuid().GetHashCode());
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    var x1 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var x2 = random.Next(image.Width);
                    var y2 = random.Next(image.Height);
                    var x3 = random.Next(image.Width);
                    var y3 = random.Next(image.Height);
                    using var pen = new Pen(VerificationCodeHelper.RandomColor(!bg));
                    graphics.DrawBezier(pen, x, y, x1, y1, x2, y2, x3, y3);
                }

                foreach (var str in strings)
                {
                    var random = new Random(Guid.NewGuid().GetHashCode());
                    using var font = new Font(options.FontFamilies[random.Next(options.FontFamilies.Length)], FontSize, options.FontStyles[random.Next(options.FontStyles.Length)]);
                    using var pen = new Pen(VerificationCodeHelper.RandomColor(!bg));
                    graphics.DrawString(str, font, pen.Brush, charLeft, random.Next(-FontSize / 2, FontSize / 2));
                    charLeft += str.Length * charWidth;
                }

                return image;
            }
            else
            {
                using var memory = new MemoryStream();
                using var gif = new GifWriter(memory, 500, 0);
                gif.WriteFrame(image);

                foreach (var str in strings)
                {
                    using var _image = new Bitmap(image.Width, image.Height);
                    using var _graphics = Graphics.FromImage(_image);
                    _graphics.Clear(bgColor);

                    //绘制干扰线
                    for (int i = 0; i < 3; i++)
                    {
                        var _random = new Random(Guid.NewGuid().GetHashCode());
                        var x = _random.Next(image.Width);
                        var y = _random.Next(image.Height);
                        var x1 = _random.Next(image.Width);
                        var y1 = _random.Next(image.Height);
                        var x2 = _random.Next(image.Width);
                        var y2 = _random.Next(image.Height);
                        var x3 = _random.Next(image.Width);
                        var y3 = _random.Next(image.Height);
                        using var _pen = new Pen(VerificationCodeHelper.RandomColor(!bg));
                        _graphics.DrawBezier(_pen, x, y, x1, y1, x2, y2, x3, y3);
                    }

                    var random = new Random(Guid.NewGuid().GetHashCode());
                    using var font = new Font(options.FontFamilies[random.Next(options.FontFamilies.Length)], FontSize, options.FontStyles[random.Next(options.FontStyles.Length)]);
                    using var pen = new Pen(VerificationCodeHelper.RandomColor(!bg));
                    _graphics.DrawString(str, font, pen.Brush, charLeft, random.Next(0, FontSize / 2));

                    charLeft += str.Length * charWidth;

                    gif.WriteFrame(_image);
                }

                return Image.FromStream(memory);
            }
        }
    }
}
