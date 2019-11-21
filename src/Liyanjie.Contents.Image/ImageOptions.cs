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
        /// 拼接目录
        /// </summary>
        public string ConcatenatedDirectory { get; set; } = @"images\concatenated";

        /// <summary>
        /// 合并目录
        /// </summary>
        public string CombinedDirectory { get; set; } = @"images\combined";

        /// <summary>
        /// 二维码目录
        /// </summary>
        public string QRCodesDirectory { get; set; } = @"images\qrcodes";

        /// <summary>
        /// 
        /// </summary>
        public string ResizePathPattern { get; set; } = $@"^\S+(?<parameters>(-(?<color>[0-9a-fA-F]{6}))?\.(?<size>\d*x\d*)?)\.(jpg|jpeg|png|gif|bmp)$";

        /// <summary>
        /// 
        /// </summary>
        public string EmptyImagePath { get; set; } = @"images\empty.jpg";

        /// <summary>
        /// 1~100
        /// </summary>
        public long CompressFlag { get; set; } = 50;
    }
}
