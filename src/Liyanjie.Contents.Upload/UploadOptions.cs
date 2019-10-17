namespace Liyanjie.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadOptions
    {
        /// <summary>
        /// 最大文件大小，单位：Byte
        /// </summary>
        public long AllowedMaximumSize { get; set; } = 4294967296;

        /// <summary>
        /// 允许的文件扩展名，默认：.jpg|.jpeg|.png|.gif
        /// </summary>
        public string AllowedExtensions { get; set; } = ".jpg|.jpeg|.png|.gif";

        /// <summary>
        /// 返回文件绝对路径，默认：true
        /// </summary>
        public string RootPath { get; set; }
    }
}
