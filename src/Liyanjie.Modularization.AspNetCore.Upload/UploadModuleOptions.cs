namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class UploadModuleOptions : UploadOptions
{
    /// <summary>
    /// 上传约束
    /// </summary>
    public Func<HttpContext, Task<bool>> RequestConstrainAsync { get; set; }

    /// <summary>
    /// 序列化输出
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
    /// 返回文件绝对路径，默认：false
    /// </summary>
    public bool ReturnAbsolutePath { get; set; } = false;
}
