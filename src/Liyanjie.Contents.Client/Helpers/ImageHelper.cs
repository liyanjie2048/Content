﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Liyanjie.Content.Client.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageHelper
    {
        readonly ContentsClientOptions options;
        readonly ILogger<ImageHelper> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public ImageHelper(
            IOptions<ContentsClientOptions> options,
            ILogger<ImageHelper> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paths"></param>
        /// <returns>图片链接地址</returns>
        public async Task<string> ConcatAsync(int? width = null, int? height = null, params string[] paths)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    paths,
                    width,
                    height,
                }));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpClient.PostAsync($"{options.ServerUrlBase}/image/concat", content);
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                    logger?.LogWarning($"Response error with status code:{response.StatusCode}");
            }

            return null;
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="items"></param>
        /// <returns>图片链接地址</returns>
        public async Task<string> CombineAsync(int width, int height, params (string Path, int? X, int? Y, int? Width, int? Height)[] items)
        {
            using (var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            })
            using (var content = new StringContent(JsonConvert.SerializeObject(new
            {
                items = items
                    .Select(_ => new
                    {
                        _.Path,
                        _.X,
                        _.Y,
                        _.Width,
                        _.Height,
                    })
                    .ToArray(),
                width,
                height,
            })))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                logger?.LogDebug($"【ImageHelper.Combine】send start:{options.ServerUrlBase}/image/combine");
                var response = await httpClient.PostAsync($"{options.ServerUrlBase}/image/combine", content);
                logger?.LogDebug($"【ImageHelper.Combine】send end:{response.StatusCode}");
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsStringAsync();
                else
                    logger?.LogWarning($"Response error with status code:{response.StatusCode}");
            }

            return null;
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin"></param>
        /// <returns>图片链接地址</returns>
        public string QRCode(string content, int width = 100, int height = 100, int margin = 0)
        {
            return $"{options.ServerUrlBase}/image/qrcode?width={width}&height={height}&margin={margin}&content={WebUtility.UrlEncode(content)}";
        }
    }
}