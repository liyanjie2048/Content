namespace Liyanjie.Content;

/// <summary>
/// 
/// </summary>
public class CaptchaOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string RootDirectory { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public string ClickCodeImageDir { get; set; } = @"images\captcha";

    /// <summary>
    /// 
    /// </summary>
    public string PuzzleCodeImageDir { get; set; } = @"images\captcha";

    /// <summary>
    /// 
    /// </summary>
    public string SliderCodeImageDir { get; set; } = @"images\captcha";

    /// <summary>
    /// 
    /// </summary>
    public string[] FontFamilies { get; set; } = ["Verdana", "Tahoma", "Arial", "Helvetica Neue", "Helvetica", "Sans - Serif"];

    /// <summary>
    /// 
    /// </summary>
    public FontStyle[] FontStyles { get; set; } = [FontStyle.Bold, FontStyle.Italic, FontStyle.Regular];

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, string> ArithmeticOperatorsText { get; set; } = new()
    {
        ["+"] = "加上",
        ["-"] = "减去",
        ["×"] = "乘以",
        ["÷"] = "除以",
        ["="] = "等于",
    };
}
