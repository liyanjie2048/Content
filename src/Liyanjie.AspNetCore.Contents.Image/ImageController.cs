using System.Collections.Generic;
using System.Threading.Tasks;

using Liyanjie.AspNetCore.Contents.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Liyanjie.AspNetCore.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageController : ControllerBase
    {
        readonly string webRootPath;
        readonly ImageOptions options;
        readonly ILogger<ImageController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public ImageController(
            IHostingEnvironment hostingEnvironment,
            IOptions<ImageOptions> options,
            ILogger<ImageController> logger)
        {
            this.webRootPath = hostingEnvironment?.WebRootPath;
            this.options = options?.Value ?? new ImageOptions();
            this.logger = logger;
        }

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Concatenate([FromBody]ImageConcatenateModel model)
        {
            if (model.Paths.IsNullOrEmpty())
                return BadRequest();

            var filePath = Process(await model.Concatenate(webRootPath, options));

            return base.Ok(filePath);
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Combine([FromBody]ImageCombineModel model)
        {
            if (model.Items == null || model.Items.Length == 0)
                return BadRequest();

            var filePath = Process(await model.Combine(webRootPath, options));

            return Ok(filePath);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult QRCode([FromQuery]ImageQRCodeModel model)
        {
            var fileName = model.CreateQRCode(webRootPath, options);

            return File(fileName, "Image/JPEG");
        }

        string Process(string filePath)
            => this.options.ReturnAbsolutePath
            ? $"{Request.Scheme}://{Request.Host}/{filePath}"
            : filePath;
    }
}
