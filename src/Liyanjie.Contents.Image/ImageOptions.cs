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
        /// 图片拼接文件目录
        /// </summary>
        public string ConcatenatedDirectory { get; set; } = @"images\concatenated";

        /// <summary>
        /// 图片拼接文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageConcatenateModel, string> ConcatenatedFileNameScheme { get; set; }
            = model => $"{model.ImagePaths.ToString(",").MD5Encoded()}.combined.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 图片合并文件目录
        /// </summary>
        public string CombinedDirectory { get; set; } = @"images\combined";

        /// <summary>
        /// 图片合并文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageCombineModel, string> CombinedFileNameScheme { get; set; }
            = model => $"{model.Items.ToString(",").MD5Encoded()}.concatenated.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 二维码图片文件目录
        /// </summary>
        public string QRCodesDirectory { get; set; } = @"images\qrcodes";

        /// <summary>
        /// 二维码图片文件名生成方案。默认：Guid
        /// </summary>
        public Func<ImageQRCodeModel, string> QRCodeFileNameScheme { get; set; }
            = model => $"{model.Content.MD5Encoded()}-{model.Margin}.{model.Width}x{model.Height}.jpg";

        /// <summary>
        /// 图片缩放路径规则
        /// </summary>
        public string ResizePathPattern { get; set; } = @"(?<parameters>(-(?<color>[0-9a-fA-F]{6}))?\.(?<size>\d*x\d*)?)\.(jpg|jpeg|png|gif|bmp)(\?.*)?$";

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
