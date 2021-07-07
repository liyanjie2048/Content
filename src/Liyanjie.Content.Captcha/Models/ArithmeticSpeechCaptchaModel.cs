#if NETFRAMEWORK
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ArithmeticSpeechCaptchaModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ArithmeticModel Arithmetic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SpeechModel Speech { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<(int Code, byte[] Audio)> GenerateAsync(CaptchaOptions options)
        {
            await Task.FromResult(0);

            var (equation, answer) = Arithmetic.Build(options);
            var audio = Speech.Generate(equation.ToString(" "));
            return (answer, audio);
        }
    }
}
#endif
