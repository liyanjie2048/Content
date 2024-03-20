namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class SliderCaptchaModel
{
    /// <summary>
    /// 宽
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 高
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 平滑度（值越大边缘越平滑，性能越底）
    /// </summary>
    public int Smooth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<(Point Point, Image Image_Origin, Image Image_Board, Image Image_Block)> GenerateAsync(CaptchaOptions options)
    {
        await Task.FromResult(0);

        var smooth = Smooth < 1 ? 1 : Smooth;
        var largeWidth = Width * smooth;
        var largeHeight = Height * smooth;

        var imageFile = Directory
            .GetFiles(Path.Combine(options.RootDirectory, options.SliderCodeImageDir))
            .RandomTake(1)
            .SingleOrDefault();
        if (imageFile is null || !File.Exists(imageFile))
            throw new Exception($"No image file found in {options.SliderCodeImageDir}");

        using var image_Origin = (Bitmap)Image.FromFile(imageFile).Resize(largeWidth, largeHeight, true, true);

        var s = Math.Min(largeWidth, largeHeight) / 12;
        var s_ = s / 2;
        var s2 = s * 2;
        var s3 = s * 3;
        var s4 = s * 4;
        var s5 = s * 5;
        var random = new Random();
        var x = random.Next(s * 3, largeWidth - s4);
        var y = random.Next(s, largeHeight - s5);

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

        var image_Board = (Bitmap)image_Origin.Clone();
        var image_Block = new Bitmap(width, height);
        for (int i = left; i < left + width; i++)
        {
            for (int j = top; j < top + height; j++)
            {
                if (path.IsVisible(i, j))
                {
                    image_Board.SetPixel(i, j, Color.Transparent);
                    image_Block.SetPixel(i - left, j - top, image_Origin.GetPixel(i, j));
                }
                else
                {
                    image_Block.SetPixel(i - left, j - top, Color.Transparent);
                }
            }
        }

        return (new Point(x / smooth, y / smooth),
            ((Image)image_Origin.Clone()).Resize(Width, Height),
            image_Board.Resize(Width, Height),
            image_Block.Resize(width / smooth, height / smooth));
    }
}
