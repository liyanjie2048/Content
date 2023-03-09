namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageCombineModel
{
    /// <summary>
    /// 
    /// </summary>
    public ImageCombineToItemModel[] Items { get; set; }

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
    public async Task<string> CombineAsync(ImageOptions options)
    {
        var fileName = options.CombinedImageFileNameScheme.Invoke(this);
        var filePath = Path.Combine(options.CombinedImageDirectory, fileName);
        var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
        Path.GetDirectoryName(filePhysicalPath).CreateDirectory();

        if (!File.Exists(filePhysicalPath))
        {
            var imageAbsolutePaths = Items
                .Select(_ => _.ImagePath)
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Select(_ => _.PreProcess(options.RootDirectory))
                .ToList();
            var imagePoints = Items
                .Select(_ => (X: _.X ?? 0, Y: _.Y ?? 0))
                .ToList();
            var imageSizes = Items
                .Select(_ => (Width: _.Width ?? 0, Height: _.Height ?? 0))
                .ToList();
            var images = new List<(Point, Size, Image)>();
            for (int i = 0; i < imageAbsolutePaths.Count; i++)
            {
                var image = await ImageHelper.FromFileOrNetworkAsync(imageAbsolutePaths[i]);
                if (image == null)
                    continue;

                var size = imageSizes[i];
                var point = imagePoints[i];

                images.Add((new Point(point.X, point.Y), new Size(size.Width, size.Height), image));
            }

            var fileImage = new Bitmap(Width, Height);
            fileImage.Combine(images.ToArray());

            using (fileImage)
                fileImage.CompressSave(filePhysicalPath, options.CompressFlag, ImageFormat.Jpeg);
        }

        return filePath;
    }
}
/// <summary>
/// 
/// </summary>
public class ImageCombineToItemModel
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
    /// <returns></returns>
    public override string ToString() => $"{ImagePath}|{X},{Y}|{Width}x{Height}";
}
