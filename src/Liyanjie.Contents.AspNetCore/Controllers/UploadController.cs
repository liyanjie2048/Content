using System;
using System.Collections.Generic;
using System.IO;
using Liyanjie.Contents.AspNetCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Liyanjie.Contents.AspNetCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadController : _Controller
    {
        readonly string webRootPath;
        readonly ContentsOptions options;
        readonly ILogger<UploadController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public UploadController(
            IHostingEnvironment hostingEnvironment,
            IOptions<ContentsOptions> options,
            ILogger<UploadController> logger)
        {
            this.webRootPath = hostingEnvironment?.WebRootPath;
            this.options = options?.Value ?? new ContentsOptions
            {
                ImageSetting = new ImageSetting()
            };
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

                var fileDirectory = Path.Combine(this.webRootPath, dir);
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
    }
}
