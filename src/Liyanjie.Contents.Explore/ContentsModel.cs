using System.Collections.Generic;

namespace Liyanjie.Contents
{
    public class ContentsModel
    {
        public class Directory
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public IEnumerable<File> Files { get; set; }
            public IEnumerable<Directory> SubDirs { get; set; }
        }
        public class File
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }
    }
}
