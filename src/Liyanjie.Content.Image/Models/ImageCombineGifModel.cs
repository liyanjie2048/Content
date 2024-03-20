namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageCombineGifModel
{
    /// <summary>
    /// 
    /// </summary>
    public ItemModel[] Items { get; set; } = [];

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
    public ushort Repeat { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<string> CombineGifAsync(ImageOptions options)
    {
        var fileName = options.CombinedGifImageFileNameScheme.Invoke(this);
        var filePath = Path.Combine(options.CombinedImageDirectory, fileName).TrimStart(ImageOptions.PathStarts);
        var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
        Path.GetDirectoryName(filePhysicalPath)?.CreateDirectory();

        if (!File.Exists(filePhysicalPath))
        {
            var images = new List<(Image, int)>();
            foreach (var item in Items.Where(_ => !string.IsNullOrWhiteSpace(_.ImagePath)))
            {
                var image_ = await ImageHelper.FromFileOrNetworkAsync(item.ImagePath.PreProcess(options.RootDirectory));
                if (image_ is null)
                    continue;

                images.Add((image_, item.Delay));
            }

            var image = new Bitmap(Width, Height);
            try
            {
                image.CombineGif(Delay, Repeat, [.. images]);
                image.CompressSave(filePhysicalPath, options.ImageQuality, ImageFormat.Gif);
            }
            catch (Exception) { }
            finally { image.Dispose(); }
        }

        return filePath;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ItemModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImagePath { get; set; } = default!;

        /// <summary>
        /// 
        /// </summary>
        public int Delay { get; set; } = 500;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{ImagePath}";
    }
}
