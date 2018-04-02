namespace Numbers.Content.Sdk
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsOptions
    {
        /// <summary>
        /// 是否返回绝对路径，默认true
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;

        /// <summary>
        /// 内容服务器根Url
        /// </summary>
        public string ServerUrlBase { get; set; }
    }
}
