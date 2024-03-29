﻿namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class CaptchaModuleOptions : CaptchaOptions
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
    /// 生成验证码请求约束
    /// </summary>
    public Func<HttpContext, Task<bool>>? RestrictRequestAsync { get; set; }

    /// <summary>
    /// 序列化输出
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
}
