namespace Liyanjie.AspNetCore.Contents.Image
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;

        /// <summary>
        /// 拼接目录
        /// </summary>
        public string ConcatsDir { get; set; } = @"images\concatenated";

        /// <summary>
        /// 合并目录
        /// </summary>
        public string CombineDir { get; set; } = @"images\combined";

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
