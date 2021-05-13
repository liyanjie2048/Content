using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Liyanjie.Content.Sample.AspNetCore_2_1
{
    public class VerificationCodeHelper
    {
        class Click
        {
            public Point[] FontPoints { get; set; }
            public string FontImage { get; set; }
            public string BoardImage { get; set; }
        }
        public static async Task<(Point[], string, string)> GetClickCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/verificationCode/click?width=200&height=200&fontsize=24&string={WebUtility.UrlEncode("风 花 雪 月")}");
            var json = JsonSerializer.Deserialize<Click>(str);
            return (json.FontPoints, json.FontImage, json.BoardImage);
        }
        class Puzzle
        {
            public int[] BlockIndexes { get; set; }
            public string ImageOrigin { get; set; }
            public string[] ImageBlocks { get; set; }
        }
        public static async Task<(int[], string, string[])> GetPuzzleCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/verificationCode/puzzle?width=200&height=200&hcount=2&vcount=2");
            var json = JsonSerializer.Deserialize<Puzzle>(str);
            return (json.BlockIndexes, json.ImageOrigin, json.ImageBlocks);
        }
        class Slider
        {
            public Point BlockPoint { get; set; }
            public string OriginImage { get; set; }
            public string BoardImage { get; set; }
            public string BlockImage { get; set; }
        }
        public static async Task<(Point, string, string, string)> GetSliderCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/verificationCode/slider?width=300&height=200");
            var json = JsonSerializer.Deserialize<Slider>(str);
            return (json.BlockPoint, json.OriginImage, json.BoardImage, json.BlockImage);
        }
        class ArithmeticImage
        {
            public int Code { get; set; }
            public string Image { get; set; }
        }
        public static async Task<(int, string)> GetArithmeticImageCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/verificationCode/arithmeticImage?arithmetic.MaxWhenAddition=50&arithmetic.MaxWhenSubtraction=100&arithmetic.MaxWhenMultiplication=10&arithmetic.MaxWhenDivision=100&arithmetic.UseZhInsteadOfOperator=true&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true");
            var json = JsonSerializer.Deserialize<ArithmeticImage>(str);
            return (json.Code, json.Image);
        }
        class StringImage
        {
            public string Code { get; set; }
            public string Image { get; set; }
        }
        public static async Task<(string, string)> GetStringImageCodeDataAsync(string urlbase)
        {
            using var http = new HttpClient();
            var str = await http.GetStringAsync($"{urlbase}/verificationCode/stringImage?string.Length=6&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true");
            var json = JsonSerializer.Deserialize<StringImage>(str);
            return (json.Code, json.Image);
        }
    }
}