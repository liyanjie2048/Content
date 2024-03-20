namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class UploadImageModel
{
    /// <summary>
    /// 
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public long FileLength { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Image Image { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="dir"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public bool TrySave(UploadImageOptions options, string dir, out string filePath)
    {
        dir = options.Regex_PathIllegalChars.Replace(dir, string.Empty);
        dir = dir.TrimStart(['/', '\\']);

        var directory = Path.Combine(options.RootDirectory, dir).Replace('/', Path.DirectorySeparatorChar);
        Directory.CreateDirectory(directory);

        var fileExtension = Path.GetExtension(FileName).ToLower();
        if (!Regex.IsMatch(fileExtension, options.AllowedExtensionsPattern))
        {
            filePath = $"Image \"{FileName}\" is not allowed.";
            return false;
        }

        if (FileLength == 0)
        {
            filePath = $"Image data is empty.";
            return false;
        }

        if (FileLength > options.AllowedMaximumSize)
        {
            filePath = $"Image \"{FileName}\" is too large.";
            return false;
        }

        try
        {
            var fileName = options.FileNameScheme(FileName, fileExtension);
            Image.Save(Path.Combine(directory, fileName));
            Image.Dispose();
            filePath = Path.Combine(dir, fileName);
            return true;
        }
        catch (Exception ex)
        {
            filePath = $"Image \"{FileName}\" write failed: {ex.Message}.";
            return false;
        }
    }
}
