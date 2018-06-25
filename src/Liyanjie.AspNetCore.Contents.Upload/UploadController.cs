using System;
using System.Collections.Generic;
using System.IO;

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
        public IActionResult Post(string dir = "temps")
        {
            logger?.LogDebug($"[FileUpload]files:{Request.Form.Files.Count}");

            dir = dir.TrimStart('/');
            var paths = new List<string>();

            foreach (var file in Request.Form.Files)
            {
                var fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(file.FileName).ToLower()}";
                var filePath = Path.Combine(dir, fileName).Replace(Path.DirectorySeparatorChar, '/');
                paths.Add(this.options.ReturnAbsolutePath ? $"{Request.Scheme}://{Request.Host}/{filePath}" : filePath);

                logger?.LogDebug($"[FileUpload]filePath:{filePath}");

                var fileDirectory = Path.Combine(this.webrootPath, dir);
                CreateDirectory(fileDirectory);
                var filePhysical = Path.Combine(fileDirectory, fileName);

                logger?.LogDebug($"[FileUpload]filePhysical:{filePhysical}");

                using (var fs = System.IO.File.Create(filePhysical))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
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
