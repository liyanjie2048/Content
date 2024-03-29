﻿namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class StringModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Source { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    public int Length { get; set; } = 6;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string Build()
    {
        if (string.IsNullOrWhiteSpace(Source))
            Source = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz1234567890";
        if (Length <= 0)
            Length = 6;

        return Source.Random(6);
    }
}
