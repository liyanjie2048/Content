using System;
using System.Threading.Tasks;
using Liyanjie.Contents.AspNetCore.Extensions;
using Liyanjie.Contents.AspNetCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Liyanjie.Contents.AspNetCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageController : _Controller
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
        public ImageController(
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
        /// 拼接图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Concat([FromBody]ImageConcatModel model)
        {
            if (model.Paths == null || model.Paths.Length == 0)
                return BadRequest();

            var filePath = Process(await model.Concat(this.webRootPath, this.options.ImageSetting));

            return base.Ok(filePath);
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Combine([FromBody]ImageCombineModel model)
        {
            if (model.Items == null || model.Items.Length == 0)
                return BadRequest();

            var filePath = Process(await model.Combine(this.webRootPath, this.options.ImageSetting));

            return Ok(filePath);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QRCode([FromQuery]ImageQRCodeModel model)
        {
            var fileName = model.CreateQRCode(this.webRootPath, this.options.ImageSetting);

            return File(fileName, "Image/JPEG");
        }

        string Process(string filePath)
            => this.options.ReturnAbsolutePath
            ? $"{Request.Scheme}://{Request.Host}/{filePath}"
            : filePath;
    }
}
