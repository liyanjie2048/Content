namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class UploadModel
{
    /// <summary>
    /// 
    /// </summary>
    public UploadFileModel[] Files { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public async Task<(bool Success, string Path)[]> SaveAsync(UploadOptions options, string dir = "temps")
    {
        dir = options.Regex_PathIllegalChars.Replace(dir, string.Empty);
        dir = dir.TrimStart(new[] { '/', '\\' });

        var directory = Path.Combine(options.RootDirectory, dir).Replace('/', Path.DirectorySeparatorChar);
        Directory.CreateDirectory(directory);

        var paths = new List<(bool, string)>();
        foreach (var file in Files)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!Regex.IsMatch(fileExtension, options.AllowedExtensionsPattern))
            {
                paths.Add((false, $"File \"{file.FileName}\" is not allowed."));
                continue;
            }

            if (file.FileLength > options.AllowedMaximumSize)
            {
                paths.Add((false, $"File \"{file.FileName}\" is too large."));
                continue;
            }

            var fileName = options.FileNameScheme(file.FileName, fileExtension);
            try
            {
                await File.WriteAllBytesAsync(Path.Combine(directory, fileName), file.FileBytes);
                paths.Add((true, Path.Combine(dir, fileName)));
            }
            catch (Exception)
            {
                paths.Add((false, $"File \"{file.FileName}\" write failed."));
            }
        }

        return paths.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    public class UploadFileModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long FileLength { get; set; }
    }
}
