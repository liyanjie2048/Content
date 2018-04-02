using System;
using System.Threading.Tasks;
using Liyanjie.Contents.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Liyanjie.Contents.AspNetCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("Image")]
    public class ImageController : _Controller
    {
        readonly Settings.Settings settings;
        readonly ILogger<ImageController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ImageController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.settings = GetRequiredService<IOptions<Settings.Settings>>().Value;
            this.logger = GetLogger<ImageController>();
        }

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("$Concat")]
        public async Task<IActionResult> Concat([FromBody]ImageConcatModel model)
        {
            if (model.Paths == null || model.Paths.Length == 0)
                return BadRequest();

            var fileName = await model.Concat(WebRootPath, settings);

            return Ok(fileName);
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("$Combine")]
        public async Task<IActionResult> Combine([FromBody]ImageCombineModel model)
        {
            if (model.Items == null || model.Items.Length == 0)
                return BadRequest();

            var fileName = await model.Combine(WebRootPath, settings);

            return Ok(fileName);
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("$QRCode")]
        public IActionResult QRCode([FromQuery]ImageQRCodeModel model)
        {
            var fileName = model.CreateQRCode(WebRootPath, settings);

            return File(fileName, "Image/JPEG");
        }
    }
}
