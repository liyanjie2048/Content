namespace Liyanjie.Content;

/// <summary>
/// 
/// </summary>
public class UploadOptions
{
    /// <summary>
    /// 根目录
    /// </summary>
    public string RootDirectory { get; set; }

    /// <summary>
    /// 文件名生成方案。默认：Guid
    /// </summary>
    public Func<string, string, string> FileNameScheme { get; set; }
        = (fileName, fileExtension) => $"{Guid.NewGuid():N}{fileExtension}";

    /// <summary>
    /// 最大文件大小。单位：Byte，默认：4294967296
    /// </summary>
    public long AllowedMaximumSize { get; set; } = 4294967296;

    /// <summary>
    /// 允许的文件扩展名正则表达式。默认：\.(jpeg|png|gif)
    /// </summary>
    public string AllowedExtensionsPattern { get; set; } = @"^\.(jpeg|png|gif)$";

    /// <summary>
    /// 允许的Base64 MIME Types
    /// </summary>
    public Dictionary<string, string> AllowedMIMETypes { get; set; } = new()
    {
        ["image/jpeg"] = ".jpeg",
        ["image/png"] = ".png",
        ["image/gif"] = ".gif",
    };

    /// <summary>
    /// 路径中的非法字符
    /// </summary>
    public Regex Regex_PathIllegalChars = new(@"\:|\*|\?|""|\<|\>|\||\s");
}
