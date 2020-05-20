using System;

namespace Liyanjie.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadOptions
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootDirectory { get; set; }
        
        /// <summary>
        /// 文件名生成方案。默认：Guid
        /// </summary>
        public Func<string, string, string> FileNameScheme { get; set; } = (fileName, fileExtension) => Guid.NewGuid().ToString("N");

        /// <summary>
        /// 最大文件大小。单位：Byte
        /// </summary>
        public long AllowedMaximumSize { get; set; } = 4294967296;

        /// <summary>
        /// 允许的文件扩展名。默认：.jpg|.jpeg|.png|.gif
        /// </summary>
        public string AllowedExtensions { get; set; } = ".jpg|.jpeg|.png|.gif";
    }
}
