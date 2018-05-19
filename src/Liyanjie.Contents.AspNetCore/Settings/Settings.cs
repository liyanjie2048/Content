namespace Liyanjie.Contents.AspNetCore.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class Settings
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
        public ImageSetting Image { get; set; } = new ImageSetting();
    }
}
