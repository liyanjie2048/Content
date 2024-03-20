namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class UploadModel
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
    public byte[] FileData { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="dir"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public bool TrySave(UploadOptions options, string dir, out string filePath)
    {
        dir = options.Regex_PathIllegalChars.Replace(dir, string.Empty);
        dir = dir.TrimStart(['/', '\\']);

        var directory = Path.Combine(options.RootDirectory, dir).Replace('/', Path.DirectorySeparatorChar);
        Directory.CreateDirectory(directory);

        var fileExtension = Path.GetExtension(FileName).ToLower();
        if (!Regex.IsMatch(fileExtension, options.AllowedExtensionsPattern))
        {
            filePath = $"File \"{FileName}\" is not allowed.";
            return false;
        }

        if (FileLength == 0)
        {
            filePath = $"File data is empty.";
            return false;
        }

        if (FileLength > options.AllowedMaximumSize)
        {
            filePath = $"File \"{FileName}\" is too large.";
            return false;
        }

        var fileName = options.FileNameScheme(FileName, fileExtension);
        try
        {
            File.WriteAllBytes(Path.Combine(directory, fileName), FileData);
            filePath = Path.Combine(dir, fileName);
            return true;
        }
        catch (Exception ex)
        {
            filePath = $"File \"{FileName}\" write failed: {ex.Message}.";
            return false;
        }
    }
}
