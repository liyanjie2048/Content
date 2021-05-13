using System.Drawing;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Liyanjie.Content.Sample.AspNetCore.Pages
{
    public class VerificationCodeModel : PageModel
    {
        public (Point[], string, string) Click { get; set; }
        public (int[], string, string[]) Puzzle { get; set; }
        public (Point, string, string, string) Slider { get; set; }
        public (int, string) Arithmetic { get; set; }
        public (string, string) String { get; set; }
        public async Task OnGet()
        {
            var urlbase = $"{Request.Scheme}://{Request.Host}";

            Click = await VerificationCodeHelper.GetClickCodeDataAsync(urlbase);
            Puzzle = await VerificationCodeHelper.GetPuzzleCodeDataAsync(urlbase);
            Slider = await VerificationCodeHelper.GetSliderCodeDataAsync(urlbase);
            Arithmetic = await VerificationCodeHelper.GetArithmeticImageCodeDataAsync(urlbase);
            String = await VerificationCodeHelper.GetStringImageCodeDataAsync(urlbase);
        }
    }
}
