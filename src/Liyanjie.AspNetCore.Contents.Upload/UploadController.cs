using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Liyanjie.AspNetCore.Contents.Upload
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadController : ControllerBase
    {
        readonly string webrootPath;
        readonly UploadOptions options;
        readonly ILogger<UploadController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public UploadController(
            IHostingEnvironment hostingEnvironment,
            IOptions<UploadOptions> options,
            ILogger<UploadController> logger)
        {
            this.webrootPath = hostingEnvironment?.WebRootPath;
            this.options = options?.Value ?? new UploadOptions();
            this.logger = logger;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Post(string dir = "temps")
        {
            logger?.LogInformation($"[FileUpload]files:{Request.Form.Files.Count}");

            dir = dir.TrimStart(new[] { '/', '\\' }).Replace(Path.DirectorySeparatorChar, '/');
            dir = Regex.Replace(dir, $@"\:|\*|\?|{'"'}|\<|\>|\||\s", string.Empty);
            var paths = new List<string>();

            foreach (var file in Request.Form.Files)
            {
                var filePath = Path.Combine(dir, $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(file.FileName).ToLower()}").Replace(Path.DirectorySeparatorChar, '/');
                logger?.LogInformation($"[FileUpload]filePath:{filePath}");

                var fileAbsolutePath = Path.Combine(webrootPath, filePath.Replace('/', Path.DirectorySeparatorChar));
                logger?.LogInformation($"[FileUpload]fileAbsolutePath:{fileAbsolutePath}");

                CreateDirectory(Path.GetDirectoryName(fileAbsolutePath));

                using (var fs = System.IO.File.Create(fileAbsolutePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                paths.Add(options.ReturnAbsolutePath ? $"{Request.Scheme}://{Request.Host}/{filePath}" : filePath);
            }

            return Ok(paths);
        }

        void CreateDirectory(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }
    }
}
