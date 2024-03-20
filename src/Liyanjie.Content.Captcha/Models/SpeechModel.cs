#if NETFRAMEWORK

using System.IO;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class SpeechModel
{
    /// <summary>
    /// 导语
    /// </summary>
    public string Begginning { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public VoiceGender VoiceGender { get; set; } = 0;
    /// <summary>
    /// 语速，-10 到 10
    /// </summary>
    public int Rate { get; set; } = 0;
    /// <summary>
    /// 音量，0 到 100
    /// </summary>
    public int Volume { get; set; } = 100;

    static object obj = new object();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public byte[] Generate(string text)
    {
        byte[] bytes = null;
        Task.Run(() =>
        {
            lock (reader)
            {
                using var memory = new MemoryStream();
                reader.SetOutputToWaveStream(memory);

                reader.Speak($"{Begginning},{text}");

                bytes = memory.ToArray();
            }
        }).Wait();
        return bytes;
    }

    static readonly SpeechSynthesizer reader = new SpeechSynthesizer();
}

#endif
