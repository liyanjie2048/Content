using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    public class UploadModel
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
        public long FileLength { get; set; }

        public async Task<string> SaveAsync(UploadOptions options, string dir = "temps")
        {
            dir = dir.TrimStart(new[] { '/', '\\' })
                .Replace($@"\:|\*|\?|{'"'}|\<|\>|\||\s", string.Empty,RegexOptions.None);

            var directory = Path.Combine(options.RootPath, dir).Replace('/', Path.DirectorySeparatorChar);

            Directory.CreateDirectory(directory);

            var fileExtension = Path.GetExtension(FileName).ToLower();
            if (options.AllowedExtensions.IndexOf(fileExtension) < 0)
            {
                return $"File \"{FileName}\" is not allowed.";
            }

            if (FileLength > options.AllowedMaximumSize)
            {
                return $"File \"{FileName}\" is too large.";
            }

            var fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";

            using var fs = File.Create(Path.Combine(directory, fileName));
            await FileStream.CopyToAsync(fs);
            await fs.FlushAsync();

            return Path.Combine(dir, fileName);
        }
    }
}
