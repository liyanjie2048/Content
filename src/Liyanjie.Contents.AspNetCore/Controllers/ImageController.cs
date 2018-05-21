using System;
using System.Threading.Tasks;
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
        readonly Settings.Settings settings;
        readonly ILogger<UploadController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public ImageController(
            IHostingEnvironment hostingEnvironment,
            IOptions<Settings.Settings> options,
            ILogger<UploadController> logger)
        {
            this.webRootPath = hostingEnvironment?.WebRootPath;
            this.settings = options?.Value ?? new Settings.Settings
            {
                Image = new Settings.ImageSetting()
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

            var filePath = Process(await model.Concat(this.webRootPath, this.settings));

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

            var filePath = Process(await model.Combine(this.webRootPath, this.settings));

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
            var fileName = model.CreateQRCode(this.webRootPath, this.settings);

            return File(fileName, "Image/JPEG");
        }

        string Process(string filePath)
            => this.settings.ReturnAbsolutePath
            ? $"{Request.Scheme}://{Request.Host}/{filePath}"
            : filePath;
    }
}
