using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Liyanjie.Content.Sample.AspNetCore.Pages
{
    public class CaptchaModel : PageModel
    {
        public CaptchaHelper.Click Click { get; set; }
        public CaptchaHelper.Puzzle Puzzle { get; set; }
        public CaptchaHelper.Slider Slider { get; set; }
        public CaptchaHelper.ArithmeticImage Arithmetic { get; set; }
        public CaptchaHelper.StringImage String { get; set; }
        public async Task OnGet()
        {
            var urlbase = $"{Request.Scheme}://{Request.Host}";

            Click = await CaptchaHelper.GetClickCodeDataAsync(urlbase);
            Puzzle = await CaptchaHelper.GetPuzzleCodeDataAsync(urlbase);
            Slider = await CaptchaHelper.GetSliderCodeDataAsync(urlbase);
            Arithmetic = await CaptchaHelper.GetArithmeticImageCodeDataAsync(urlbase);
            String = await CaptchaHelper.GetStringImageCodeDataAsync(urlbase);
        }
    }
}
