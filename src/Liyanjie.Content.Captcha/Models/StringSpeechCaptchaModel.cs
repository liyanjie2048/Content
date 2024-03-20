#if NETFRAMEWORK

using System.Threading.Tasks;

namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class StringSpeechCaptchaModel
{
    /// <summary>
    /// 
    /// </summary>
    public StringModel String { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public SpeechModel Speech { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<(string Code, byte[] Audio)> GenerateAsync(CaptchaOptions options)
    {
        await Task.FromResult(0);

        var str = String.Build();
        var audio = Speech.Generate(str);
        return (str, audio);
    }
}

#endif
