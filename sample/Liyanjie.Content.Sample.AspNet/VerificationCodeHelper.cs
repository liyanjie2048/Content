using System.Drawing;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

namespace Liyanjie.Content.Sample.AspNet
{
    public class VerificationCodeHelper
    {
        class Click
        {
            public Point[] FontPoints { get; set; }
            public string FontImage { get; set; }
            public string BoardImage { get; set; }
        }
        public static (Point[], string, string) GetClickCodeData(string urlbase)
        {
            (Point[], string, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/click?width=200&height=200&fontsize=24&string={WebUtility.UrlEncode("风 花 雪 月")}")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<Click>(t.Result);
                    output = (json.FontPoints, json.FontImage, json.BoardImage);
                }).Wait();

            return output;
        }
        class Puzzle
        {
            public int[] BlockIndexes { get; set; }
            public string ImageOrigin { get; set; }
            public string[] ImageBlocks { get; set; }
        }
        public static (int[], string, string[]) GetPuzzleCodeData(string urlbase)
        {
            (int[], string, string[]) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/puzzle?width=200&height=200&hcount=2&vcount=2")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<Puzzle>(t.Result);
                    output = (json.BlockIndexes, json.ImageOrigin, json.ImageBlocks);
                }).Wait();

            return output;
        }
        class Slider
        {
            public Point BlockPoint { get; set; }
            public string OriginImage { get; set; }
            public string BoardImage { get; set; }
            public string BlockImage { get; set; }
        }
        public static (Point, string, string, string) GetSliderCodeData(string urlbase)
        {
            (Point, string, string, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/slider?width=300&height=200")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<Slider>(t.Result);
                    output = (json.BlockPoint, json.OriginImage, json.BoardImage, json.BlockImage);
                }).Wait();

            return output;
        }
        class ArithmeticImage
        {
            public int Code { get; set; }
            public string Image { get; set; }
        }
        public static (int, string) GetArithmeticImageCodeData(string urlbase)
        {
            (int, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/arithmeticImage?arithmetic.MaxWhenAddition=50&arithmetic.MaxWhenSubtraction=100&arithmetic.MaxWhenMultiplication=10&arithmetic.MaxWhenDivision=100&arithmetic.UseZhInsteadOfOperator=true&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<ArithmeticImage>(t.Result);
                    output = (json.Code, json.Image);
                }).Wait();

            return output;
        }
        class StringImage
        {
            public string Code { get; set; }
            public string Image { get; set; }
        }
        public static (string, string) GetStringImageCodeData(string urlbase)
        {
            (string, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/stringImage?string.Length=6&image.width=100&image.height=30&image.fontsize=16&image.GenerateGif=true")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<StringImage>(t.Result);
                    output = (json.Code, json.Image);
                }).Wait();

            return output;
        }
        class ArithmeticSpeech
        {
            public int Code { get; set; }
            public string Audio { get; set; }
        }
        public static (int, string) GetArithmeticSpeechCodeData(string urlbase)
        {
            (int, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/arithmeticSpeech?arithmetic.MaxWhenAddition=50&arithmetic.MaxWhenSubtraction=100&arithmetic.MaxWhenMultiplication=10&arithmetic.MaxWhenDivision=100&arithmetic.UseZhInsteadOfOperator=true&speech.Begginning={WebUtility.UrlEncode("请填写答案：")}")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<ArithmeticSpeech>(t.Result);
                    output = (json.Code, json.Audio);
                }).Wait();

            return output;
        }
        class StringSpeech
        {
            public string Code { get; set; }
            public string Audio { get; set; }
        }
        public static (string, string) GetStringSpeechCodeData(string urlbase)
        {
            (string, string) output = default;

            using var http = new HttpClient();
            http.GetStringAsync($"{urlbase}/verificationCode/stringSpeech?string.Length=6&speech.Begginning={WebUtility.UrlEncode("验证码是：")}")
                .ContinueWith(t =>
                {
                    var json = JsonConvert.DeserializeObject<StringSpeech>(t.Result);
                    output = (json.Code, json.Audio);
                }).Wait();

            return output;
        }
    }
}