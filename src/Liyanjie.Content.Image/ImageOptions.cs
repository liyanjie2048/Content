﻿namespace Liyanjie.Content;

/// <summary>
/// 
/// </summary>
public class ImageOptions
{
    internal readonly static char[] PathStarts = ['/', '\\'];

    /// <summary>
    /// 
    /// </summary>
    public string RootDirectory { get; set; } = default!;

    /// <summary>
    /// 合并图片目录
    /// </summary>
    public string CombinedImageDirectory { get; set; } = @"images\combined";

    /// <summary>
    /// 合并图片文件名生成方案
    /// </summary>
    public Func<ImageCombineModel, string> CombineImageFileNameScheme { get; set; }
        = model => $"{model.Items.ToString(",").MD5Encode()}_combined-{model.Width}x{model.Height}.jpeg";

    /// <summary>
    /// 合并GIF图片目录
    /// </summary>
    public string CombinedGIFImageDirectory { get; set; } = @"images\combinedGif";

    /// <summary>
    /// 合并GIF图片文件名生成方案
    /// </summary>
    public Func<ImageCombineGifModel, string> CombinedGifImageFileNameScheme { get; set; }
        = model => $"{model.Items.ToString(",").MD5Encode()}_combined-{model.Width}x{model.Height}.gif";

    /// <summary>
    /// 拼接图片目录
    /// </summary>
    public string ConcatenatedImageDirectory { get; set; } = @"images\concatenated";

    /// <summary>
    /// 拼接图片文件名生成方案
    /// </summary>
    public Func<ImageConcatenateModel, string> ConcatenatedImageFileNameScheme { get; set; }
        = model => $"{model.ImagePaths.ToString(",").MD5Encode()}_concatenated-{model.Width}x{model.Height}.jpeg";

    /// <summary>
    /// 二维码图片文件目录
    /// </summary>
    public string QRCodeImageDirectory { get; set; } = @"images\qrcode";

    /// <summary>
    /// 二维码图片文件名生成方案
    /// </summary>
    public Func<ImageQRCodeModel, string> QRCodeImageFileNameScheme { get; set; }
        = model => $"{model.Content.MD5Encode()}_{model.Width}x{model.Height}-m{model.Margin}.svg";

    /// <summary>
    /// 缩放图片路径规则
    /// </summary>
    public string ResizePathPattern { get; set; } = @"(?<parameters>_(?<size>\d*x\d*)(\-(?<color>[0-9a-fA-F]{6}))?)\.(jpg|jpeg|png|gif|bmp)(\?.*)?$";

    /// <summary>
    /// 裁剪图片目录
    /// </summary>
    public string CroppedImageDirectory { get; set; } = @"images\cropped";

    /// <summary>
    /// 裁剪图片文件名生成方案
    /// </summary>
    public Func<ImageCropModel, string> CroppedImageFileNameScheme { get; set; }
        = model => $"{model.ImagePath.MD5Encode()}_cropped-{model.Left}x{model.Top}-{model.Width}x{model.Height}-{model.Radius}.png";

    /// <summary>
    /// 空文件路径
    /// </summary>
    public string EmptyImagePath { get; set; } = @"images\empty.png";

    /// <summary>
    /// 0~100，default is 75.
    /// </summary>
    public int ImageQuality
    {
        get { return _imageQuality; }
        set { _imageQuality = value < 0 ? 0 : _imageQuality > 100 ? 100 : _imageQuality; }
    }
    private int _imageQuality = 75;
}
