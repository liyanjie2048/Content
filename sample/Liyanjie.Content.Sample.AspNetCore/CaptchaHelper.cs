using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Liyanjie.Content.Sample.AspNetCore
{
    public class CaptchaHelper
    {
        readonly static JsonSerializerOptions _jsonDeserializerOptions = new()
        {
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
        };

        public class Click
        {
            public Point[] Points { get; set; }
            public string Image_Fonts { get; set; }
            public string Image_Board { get; set; }
        }
        public static async Task<Click> GetClickCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/captcha/click?width=200&height=200&fontsize=24&string={WebUtility.UrlEncode("风 花 雪 月")}");
            System.Console.WriteLine(str);
            return JsonSerializer.Deserialize<Click>(str, _jsonDeserializerOptions);
        }

        public class Puzzle
        {
            public int[] Indexes { get; set; }
            public string Image_Origin { get; set; }
            public string[] Image_Blocks { get; set; }
        }
        public static async Task<Puzzle> GetPuzzleCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/captcha/puzzle?width=200&height=200&hcount=2&vcount=2");
            System.Console.WriteLine(str);
            return JsonSerializer.Deserialize<Puzzle>(str, _jsonDeserializerOptions);
        }

        public class Slider
        {
            public Point Point { get; set; }
            public string Image_Origin { get; set; }
            public string Image_Board { get; set; }
            public string Image_Block { get; set; }
        }
        public static async Task<Slider> GetSliderCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/captcha/slider?width=300&height=200");
            System.Console.WriteLine(str);
            return JsonSerializer.Deserialize<Slider>(str, _jsonDeserializerOptions);
        }

        public class ArithmeticImage
        {
            public int Code { get; set; }
            public string Image { get; set; }
        }
        public static async Task<ArithmeticImage> GetArithmeticImageCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/captcha/arithmeticImage?arithmetic.MaxWhenAddition=50&arithmetic.MaxWhenSubtraction=100&arithmetic.MaxWhenMultiplication=10&arithmetic.MaxWhenDivision=100&arithmetic.UseZhInsteadOfOperator=true&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true");
            System.Console.WriteLine(str);
            return JsonSerializer.Deserialize<ArithmeticImage>(str, _jsonDeserializerOptions);
        }

        public class StringImage
        {
            public string Code { get; set; }
            public string Image { get; set; }
        }
        public static async Task<StringImage> GetStringImageCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/captcha/stringImage?string.Length=6&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true");
            System.Console.WriteLine(str);
            return JsonSerializer.Deserialize<StringImage>(str, _jsonDeserializerOptions);
        }
    }
}