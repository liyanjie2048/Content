using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
    public class UploadModel
    {
        public UploadFileModel[] Files { get; set; }

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async Task<(bool Success, string FilePath)[]> SaveAsync(UploadOptions options, string dir = "temps")
        {
            dir = dir.TrimStart(new[] { '/', '\\' })
                .Replace($@"\:|\*|\?|{'"'}|\<|\>|\||\s", string.Empty, RegexOptions.None);

            var directory = Path.Combine(options.RootDirectory, dir).Replace('/', Path.DirectorySeparatorChar);

            Directory.CreateDirectory(directory);

            var filePaths = new List<(bool, string)>();
            foreach (var file in Files)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (options.AllowedExtensions.IndexOf(fileExtension) < 0)
                {
                    filePaths.Add((false, $"File \"{file.FileName}\" is not allowed."));
                    continue;
                }

                if (file.FileLength > options.AllowedMaximumSize)
                {
                    filePaths.Add((false, $"File \"{file.FileName}\" is too large."));
                    continue;
                }

                var fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";

                using var fs = File.Create(Path.Combine(directory, fileName));
                using (file.FileStream)
                {
#if NET45
                    file.FileStream.CopyTo(fs);
#else
                    await file.FileStream.CopyToAsync(fs);
#endif
                }

                filePaths.Add((true, Path.Combine(dir, fileName)));
            }

            return filePaths.ToArray();
        }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    }
    public class UploadFileModel
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
        public long FileLength { get; set; }
    }
}
