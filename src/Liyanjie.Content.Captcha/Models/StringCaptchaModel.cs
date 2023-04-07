namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class StringCaptchaModel
{
    /// <summary>
    /// 
    /// </summary>
    public StringModel String { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ImageModel Image { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<(string Code, Image Image)> GenerateAsync(CaptchaOptions options)
    {
        await Task.FromResult(0);

        var str = String.Build();
        var image = Image.Generate(str.Select(_ => _.ToString()), options);
        return (str, image);
    }
}
