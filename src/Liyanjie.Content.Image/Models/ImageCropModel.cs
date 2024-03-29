﻿namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageCropModel
{
    /// <summary>
    /// 
    /// </summary>
    public string ImagePath { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Top { get; set; }

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
    public int Radius { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<string> CropAsync(ImageOptions options)
    {
        var fileName = options.CroppedImageFileNameScheme.Invoke(this);
        var filePath = Path.Combine(options.CroppedImageDirectory, fileName).TrimStart(ImageOptions.PathStarts);
        var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
        Path.GetDirectoryName(filePhysicalPath)?.CreateDirectory();

        if (!File.Exists(filePhysicalPath))
        {
            var imageAbsolutePath = ImagePath.PreProcess(options.RootDirectory);
            if (await ImageHelper.FromFileOrNetworkAsync(imageAbsolutePath) is not Bitmap bitmap)
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

            var image = new Bitmap(Width, Height);
            for (int i = Left; i < Left + Width; i++)
            {
                for (int j = Top; j < Top + Height; j++)
                {
                    image.SetPixel(i - Left, j - Top, path.IsVisible(i, j) ? bitmap.GetPixel(i, j) : Color.Transparent);
                }
            }

            try
            {
                image.CompressSave(filePhysicalPath, options.ImageQuality, ImageFormat.Png);
            }
            catch (Exception) { }
            finally { image.Dispose(); }
        }

        return filePath;
    }
}
