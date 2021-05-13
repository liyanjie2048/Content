using System.Drawing;
using System.Threading.Tasks;

namespace Liyanjie.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ArithmeticImageCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ArithmeticModel Arithmetic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ImageModel Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<(int Code, Image Image)> GenerateAsync(VerificationCodeOptions options)
        {
            await Task.FromResult(0);

            var (equation, answer) = Arithmetic.Build(options);
            var image = Image.Generate(equation, options);
            return (answer, image);
        }
    }
}
