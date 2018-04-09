using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Liyanjie.Content.Sdk.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsHelper
    {
        readonly ContentsOptions options;
        readonly ILogger<ContentsHelper> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="loggerFactory"></param>
        public ContentsHelper(IOptions<ContentsOptions> options, ILoggerFactory loggerFactory)
        {
            this.options = options.Value;
            this.logger = loggerFactory?.CreateLogger<ContentsHelper>();
        }

        /// <summary>
        /// 转发上传的文件到内容服务器
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<string[]> TransmitAsync(string targetDirectory, params IFormFile[] files)
        {
            if (files == null || files.Length == 0)
                return null;

            return await TransmitAsync(targetDirectory, files.Select(_ => (_.OpenReadStream(), _.FileName, _.ContentType)).ToArray());
        }

        /// <summary>
        /// 转发上传的文件流到内容服务器
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<string[]> TransmitAsync(string targetDirectory, params (Stream Stream, string FileName, string ContentType)[] files)
        {
            if (files == null || files.Length == 0)
                return null;

            logger?.LogDebug($"[TransmitAsync]files:{files.Length}");

            try
            {
                using (var httpClient = new HttpClient())
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var file in files)
                    {
                        var streamContent = new StreamContent(file.Stream);
                        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "\"files\"",
                            FileName = $"\"{file.FileName}\""
                        };
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        content.Add(streamContent);
                    }

                    var response = await httpClient.PostAsync($"{options.ServerUrlBase}/upload?dir={WebUtility.UrlEncode(targetDirectory)}&returnAbsolutePath={options.ReturnAbsolutePath}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var @string = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<string[]>(@string);
                    }
                    else
                        logger?.LogError($"Response error with status code:{response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                logger?.LogError(default(EventId), e, e.Message);
            }

            return null;
        }

        /// <summary>
        /// 下载文件到内容服务器
        /// </summary>
        /// <param name="targetDirectory"></param>
        /// <param name="urls"></param>
        /// <returns>文件链接地址</returns>
        public async Task<string[]> DownloadAsync(string targetDirectory, params string[] urls)
        {
            if (urls == null || urls.Length == 0)
                return null;

            var files = new List<(Stream Stream, string FileName, string ContentType)>();
            foreach (var url in urls)
            {
                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(url);
                    if (response.Content.Headers.TryGetValues("Content-Type", out var contentTypes))
                    {
                        var contentType = contentTypes.FirstOrDefault();
                        var fileExtension = contentType?.Substring(contentType.IndexOf('/') + 1);
                        var fileName = $"{Guid.NewGuid().ToString("N")}.{fileExtension}";
                        files.Add((await response.Content.ReadAsStreamAsync(), fileName, contentType));
                    }
                }
            }
            return await TransmitAsync(targetDirectory, files.ToArray());
        }
    }
}