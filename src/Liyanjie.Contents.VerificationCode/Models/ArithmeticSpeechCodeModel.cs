using System;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
#if NET45
    /// <summary>
    /// 
    /// </summary>
    public class ArithmeticSpeechCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public _ArithmeticModel Arithmetic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public _SpeechModel Speech { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<(int Code, byte[] Audio)> GenerateAsync(VerificationCodeOptions options)
        {
            await Task.FromResult(0);

            var (equation, answer) = Arithmetic.Build(options);
            var audio = Speech.Generate(equation.ToString(" "));
            return (answer, audio);
        }
    }
#endif
}
