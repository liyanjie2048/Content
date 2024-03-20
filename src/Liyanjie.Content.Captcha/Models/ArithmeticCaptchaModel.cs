namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ArithmeticCaptchaModel
{
    /// <summary>
    /// 
    /// </summary>
    public ArithmeticModel Arithmetic { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public ImageModel Image { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<(int Code, Image Image)> GenerateAsync(CaptchaOptions options)
    {
        await Task.FromResult(0);

        var (equation, answer) = Arithmetic.Build(options);
        var image = Image.Generate(equation, options);
        return (answer, image);
    }
}
