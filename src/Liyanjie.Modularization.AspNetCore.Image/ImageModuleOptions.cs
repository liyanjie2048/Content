namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageModuleOptions : ImageOptions
{
    readonly static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// 请求约束
    /// </summary>
    public Func<HttpContext, Task<bool>> RestrictRequestAsync { get; set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    public Func<HttpRequest, Type, Task<object>> DeserializeFromRequestAsync { get; set; }
        = async (request, type) =>
        {
            using var streamReader = new StreamReader(request.Body);
            var str = await streamReader.ReadToEndAsync();
            return JsonSerializer.Deserialize(str, type, _jsonSerializerOptions);
        };

    /// <summary>
    /// 序列化
    /// </summary>
    public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
        = async (response, obj) =>
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(obj, _jsonSerializerOptions));
            await response.CompleteAsync();
        };

    /// <summary>
    /// 
    /// </summary>
    public bool ReturnAbsolutePath { get; set; } = false;
}
