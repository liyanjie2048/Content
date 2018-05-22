using Liyanjie.Contents.AspNetCore.Extensions;

namespace Liyanjie.Contents.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] ThisHosts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath = true;

        /// <summary>
        /// 
        /// </summary>
        public ImageSetting ImageSetting { get; set; }
    }
}
