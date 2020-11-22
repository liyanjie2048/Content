using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liyanjie.Contents.Models
{
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

                var fileName = options.FileNameScheme(file.FileName, fileExtension);
                var filePhysicalPath = Path.Combine(directory, fileName);
                using var fs = File.Create(filePhysicalPath);
                using (file.FileStream)
                {
#if NET45
                    file.FileStream.CopyTo(fs);
#else
                    await file.FileStream.CopyToAsync(fs);
#endif
                }

                options.WhenUploadComplete?.Invoke(filePhysicalPath);

                filePaths.Add((true, Path.Combine(dir, fileName)));
            }

            return filePaths.ToArray();
        }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
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
        public Stream FileStream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long FileLength { get; set; }
    }
}
