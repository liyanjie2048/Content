using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Liyanjie.Contents.AspNetCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("")]
    public class UploadController : _Controller
    {
        readonly ILogger<UploadController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public UploadController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.logger = GetLogger<UploadController>();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="returnAbsolutePath"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult Post(string dir = "temps", bool returnAbsolutePath = true)
        {
            logger?.LogDebug($"[FileUpload]files:{Request.Form.Files.Count}");

            dir = dir.TrimStart('/');
            var paths = new List<string>();

            foreach (var file in Request.Form.Files)
            {
                var fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(file.FileName).ToLower()}";
                var filePath = Path.Combine(dir, fileName).Replace(Path.DirectorySeparatorChar, '/');
                paths.Add(returnAbsolutePath ? $"{Request.Scheme}://{Request.Host}/{filePath}" : filePath);

                logger?.LogDebug($"[FileUpload]filePath:{filePath}");

                var fileDirectory = Path.Combine(WebRootPath, dir);
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
