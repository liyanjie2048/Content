using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using Liyanjie.Content;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Modularization.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModuleOptions : ImageOptions
    {
        /// <summary>
        /// 请求约束
        /// </summary>
        public Func<HttpContext, Task<bool>> RequestConstrainAsync { get; set; }

        /// <summary>
        /// 反序列化
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }
            = async (request, type) =>
            {
                using var streamReader = new StreamReader(request.Body);
                var str = await streamReader.ReadToEndAsync();
                return JsonSerializer.Deserialize(str, type, new()
                {
                    IgnoreNullValues = true,
                    IgnoreReadOnlyProperties = true,
                    PropertyNameCaseInsensitive = true,
                });
            };

        /// <summary>
        /// 序列化
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
            = async (response, obj) =>
            {
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonSerializer.Serialize(obj));
                await response.CompleteAsync();
            };

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = false;
    }
}
