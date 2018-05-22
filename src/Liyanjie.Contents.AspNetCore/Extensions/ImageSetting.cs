namespace Liyanjie.Contents.AspNetCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageSetting
    {
        /// <summary>
        /// 拼接目录
        /// </summary>
        public string ConcatsDir { get; set; } = @"images\concats";

        /// <summary>
        /// 合并目录
        /// </summary>
        public string CombineDir { get; set; } = @"images\combines";

        /// <summary>
        /// 二维码目录
        /// </summary>
        public string QRCodesDir { get; set; } = @"images\qrcodes";

        /// <summary>
        /// 
        /// </summary>
        public string EmptyPath { get; set; } = @"images/empty.jpg";

        /// <summary>
        /// 1~100
        /// </summary>
        public long CompressFlag { get; set; } = 50;
    }
}
