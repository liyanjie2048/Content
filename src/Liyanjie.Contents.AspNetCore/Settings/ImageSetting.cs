namespace Liyanjie.Contents.AspNetCore.Settings
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
        /// 0.01~1.00
        /// </summary>
        public double CompressFlag { get; set; } = 0.5;
    }
}
