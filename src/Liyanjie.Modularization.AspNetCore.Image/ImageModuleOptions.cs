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
        /// 
        /// </summary>
        public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync = async (request, type) =>
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
        /// 
        /// </summary>
        public Func<HttpResponse, object, Task> SerializeToResponseAsync = async (response, obj) =>
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(obj));
        };

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
