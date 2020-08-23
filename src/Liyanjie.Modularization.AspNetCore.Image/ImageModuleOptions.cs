using System;
using System.Text.Json;
using System.Threading.Tasks;

using Liyanjie.Contents;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModuleOptions : ImageOptions
    {
        /// <summary>
        /// 图片合并约束
        /// </summary>
        public Func<HttpContext, Task<bool>> CombineConstrainAsync { get; set; }

        /// <summary>
        /// 图片拼接约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ConcatenateConstrainAsync { get; set; }

        /// <summary>
        /// 二维码图片约束
        /// </summary>
        public Func<HttpContext, Task<bool>> QRCodeConstrainAsync { get; set; }

        /// <summary>
        /// 图片缩放约束
        /// </summary>
        public Func<HttpContext, Task<bool>> ResizeConstrainAsync { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; } = async (request, type) =>
        {
            using var streamReader = new System.IO.StreamReader(request.Body);
            var _request = await streamReader.ReadToEndAsync();
            return JsonSerializer.Deserialize(_request, type, new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = true,
                PropertyNameCaseInsensitive = true,
            });
        };

        /// <summary>
        /// 序列化
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; } = async (response, obj) =>
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(obj));
        };

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
