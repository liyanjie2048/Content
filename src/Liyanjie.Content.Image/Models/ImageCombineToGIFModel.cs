namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageCombineToGIFModel
{
    /// <summary>
    /// 
    /// </summary>
    public ImageCombineToGIFItemModel[] Items { get; set; }

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
    public int Delay { get; set; } = 500;

    /// <summary>
    /// 
    /// </summary>
    public int Repeat { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<string> CombineToGIFAsync(ImageOptions options)
    {
        var fileName = options.CombinedGIFImageFileNameScheme.Invoke(this);
        var filePath = Path.Combine(options.CombinedImageDirectory, fileName).TrimStart(ImageOptions.PathStarts);
        var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
        Path.GetDirectoryName(filePhysicalPath).CreateDirectory();

        if (!File.Exists(filePhysicalPath))
        {
            var images = new List<(Point, Size, Image, int)>();
            foreach (var item in Items.Where(_ => !string.IsNullOrWhiteSpace(_.ImagePath)))
            {
                var image_ = await ImageHelper.FromFileOrNetworkAsync(item.ImagePath.PreProcess(options.RootDirectory));
                if (image_ is null)
                    continue;

                images.Add((new Point(item.X ?? 0, item.Y ?? 0), new Size(item.Width ?? 0, item.Height ?? 0), image_, item.Delay));
            }

            var image = new Bitmap(Width, Height);
            try
            {
                image.CombineToGif(Delay, Repeat, images.ToArray());
                image.CompressSave(filePhysicalPath, options.ImageQuality, ImageFormat.Gif);
            }
            catch (Exception) { }
            finally { image.Dispose(); }
        }

        return filePath;
    }
}
/// <summary>
/// 
/// </summary>
public class ImageCombineToGIFItemModel
{
    /// <summary>
    /// 
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? X { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Y { get; set; }

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
    public int Delay { get; set; } = 500;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{ImagePath}|{X},{Y}|{Width}x{Height}";
}
