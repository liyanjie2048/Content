namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ImageModuleOptions : ImageOptions
{
    /// <summary>
    /// 
    /// </summary>
    public static JsonSerializerOptions JsonDeserializationOptions { get; set; } = new()
    {
        IgnoreReadOnlyProperties = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// 
    /// </summary>
    public static JsonSerializerOptions JsonSerializationOptions { get; set; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// 请求约束
    /// </summary>
    public Func<HttpContext, Task<bool>>? RestrictRequestAsync { get; set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    public Func<HttpRequest, Type, Task<object?>> DeserializeFromRequestAsync { get; set; }
        = async (request, type) =>
        {
            using var streamReader = new StreamReader(request.Body);
            var str = await streamReader.ReadToEndAsync();
            return JsonSerializer.Deserialize(str, type, JsonDeserializationOptions);
        };

    /// <summary>
    /// 序列化
    /// </summary>
    public Func<HttpResponse, object, Task> SerializeToResponseAsync { get; set; }
        = async (response, obj) =>
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";
            await response.WriteAsync(JsonSerializer.Serialize(obj, JsonSerializationOptions));
            await response.CompleteAsync();
        };

    /// <summary>
    /// 
    /// </summary>
    public bool ReturnAbsolutePath { get; set; } = false;

    internal string PathToWebPath(string path, HttpRequest request)
    {
        path = path.Replace(Path.DirectorySeparatorChar, '/');
        path = ReturnAbsolutePath
            ? $"{request.Scheme}://{request.Host}/{path}"
            : $"/{path}";
        return path;
    }
}
