using System;
using System.Linq;

using Liyanjie.Contents.Models;

namespace Liyanjie.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// 拼接图片目录
        /// </summary>
        public string ConcatenatedImageDirectory { get; set; } = @"images\concatenated";

        /// <summary>
        /// 拼接图片文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageConcatenateModel, string> ConcatenatedImageFileNameScheme { get; set; }
            = model => $"{model.ImagePaths.ToString(",").MD5Encoded()}.combined.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 合并图片目录
        /// </summary>
        public string CombinedImageDirectory { get; set; } = @"images\combined";

        /// <summary>
        /// 合并图片文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageCombineModel, string> CombinedImageFileNameScheme { get; set; }
            = model => $"{model.Items.ToString(",").MD5Encoded()}.concatenated.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 二维码图片文件目录
        /// </summary>
        public string QRCodeImageDirectory { get; set; } = @"images\qrcodes";

        /// <summary>
        /// 二维码图片文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageQRCodeModel, string> QRCodeImageFileNameScheme { get; set; }
            = model => $"{model.Content.MD5Encoded()}-{model.Margin}.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 缩放图片路径规则
        /// </summary>
        public string ResizePathPattern { get; set; } = @"(?<parameters>(-(?<color>[0-9a-fA-F]{6}))?\.(?<size>\d*x\d*)?)\.(jpg|jpeg|png|gif|bmp)(\?.*)?$";

        /// <summary>
        /// 裁剪图片目录
        /// </summary>
        public string CroppedImageDirectory { get; set; } = @"images\cropped";

        /// <summary>
        /// 裁剪图片文件名生成方案
        /// </summary>
        public Func<ImageCropModel,string> CroppedImageFileNameScheme { get; set; }
            = model => $"{model.ImagePath.MD5Encoded()}.cropped.{model.Left}x{model.Top}.{model.Radius}.{model.Width}x{model.Height}.png";

        /// <summary>
        /// 空文件路径
        /// </summary>
        public string EmptyImagePath { get; set; } = @"images\empty.jpg";

        /// <summary>
        /// 1~100
        /// </summary>
        public long CompressFlag { get; set; } = 50;
    }
}
