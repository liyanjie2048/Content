namespace Liyanjie.Content;

/// <summary>
/// 
/// </summary>
public class ContentModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<File> Files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Directory> SubDirs { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class File
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
    }
}
