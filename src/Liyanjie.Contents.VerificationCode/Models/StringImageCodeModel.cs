using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class StringImageCodeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public _StringModel String { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public _ImageModel Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<(string Code, Image Image)> GenerateAsync(VerificationCodeOptions options)
        {
            await Task.FromResult(0);

            var str = String.Build();
            var image = Image.Generate(str.Select(_ => _.ToString()), options);
            return (str, image);
        }
    }
}
