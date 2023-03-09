namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class PuzzleCaptchaModel
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
    public int HCount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int VCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<(int[] BlockIndexes, Image ImageOrigin, Image[] ImageBlocks)> GenerateAsync(CaptchaOptions options)
    {
        await Task.FromResult(0);

        var imageFile = Directory
            .GetFiles(Path.Combine(options.RootDirectory, options.PuzzleCodeImageDir))
            .RandomTake(1).SingleOrDefault();
        using var imageOrigin = Image.FromFile(imageFile).Resize(Width, Height, true, true);

        var width = imageOrigin.Width / HCount;
        var height = imageOrigin.Height / VCount;
        var images = new Dictionary<Image, int>();
        var index = 0;
        for (int i = 0; i < VCount; i++)
        {
            for (int j = 0; j < HCount; j++)
            {
                images.Add(imageOrigin.Crop(j * width, i * height, width, height), index);
                index++;
            }
        }
        images = images.OrderBy(_ => Guid.NewGuid()).ToDictionary(_ => _.Key, _ => _.Value);

        var output_index = images
            .Select((item, i) => new KeyValuePair<int, int>(item.Value, i))
            .ToDictionary(_ => _.Key, _ => _.Value)
            .OrderBy(_ => _.Key)
            .Select(_ => _.Value)
            .ToArray();

        return (output_index, (Image)imageOrigin.Clone(), images.Keys.ToArray());
    }
}
