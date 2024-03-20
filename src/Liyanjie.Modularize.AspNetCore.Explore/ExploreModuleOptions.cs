namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ExploreModuleOptions : ExploreOptions
{
    /// <summary>
    /// 
    /// </summary>
    public static JsonSerializerOptions JsonSerializationOptions { get; set; } = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// 上传约束
    /// </summary>
    public Func<HttpContext, Task<bool>>? RestrictRequestAsync { get; set; }

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
    /// 返回文件绝对路径，默认：false
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
