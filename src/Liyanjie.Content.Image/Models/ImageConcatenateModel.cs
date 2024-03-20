namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageConcatenateModel
{
    /// <summary>
    /// 
    /// </summary>
    public string[] ImagePaths { get; set; } = [];

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
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<string> ConcatenateAsync(ImageOptions options)
    {
        var fileName = options.ConcatenatedImageFileNameScheme.Invoke(this);
        var filePath = Path.Combine(options.ConcatenatedImageDirectory, fileName).TrimStart(ImageOptions.PathStarts);
        var filePhysicalPath = Path.Combine(options.RootDirectory, filePath).Replace('/', Path.DirectorySeparatorChar);
        Path.GetDirectoryName(filePhysicalPath)?.CreateDirectory();

        if (!File.Exists(filePhysicalPath))
        {
            var fileAbsolutePaths = ImagePaths
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Select(_ => _.PreProcess(options.RootDirectory))
                .ToList();

            var image = default(Image);
            var i = 0;
            do
            {
                image = (await ImageHelper.FromFileOrNetworkAsync(fileAbsolutePaths[i]))?.Resize(Width, Height);
            } while (image is null && i < fileAbsolutePaths.Count);

            if (image is not null)
            {
                foreach (var path in fileAbsolutePaths.Skip(i))
                {
                    using var image_ = await ImageHelper.FromFileOrNetworkAsync(path);
                    if (image_ is null)
                        continue;

                    image = image.Concatenate(image_.Resize(Width, Height));
                }

                try
                {
                    image.CompressSave(filePhysicalPath, options.ImageQuality, ImageFormat.Jpeg);
                }
                catch (Exception) { }
                finally { image.Dispose(); }
            }
        }

        return filePath;
    }
}
