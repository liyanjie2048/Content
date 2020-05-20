using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Liyanjie.Contents
{
    public class ExploreHelper
    {
        public static IList<ContentsModel.Directory> GetContents(ExploreOptions options)
        {
            var rootDirectory = options.RootDirectory;

            return options.Directories.Select(_ =>
            {
                var di = new DirectoryInfo(Path.Combine(rootDirectory, _));
                return new ContentsModel.Directory
                {
                    Name = _,
                    Path = _,
                    Files = di.GetFiles().Select(__ => new ContentsModel.File
                    {
                        Name = __.Name,
                        Path = FixToRelativePath(__.FullName, rootDirectory),
                    }).ToList(),
                    SubDirs = EnumerateDir(di, rootDirectory),
                };
            }).ToList();
        }

        static IList<ContentsModel.Directory> EnumerateDir(DirectoryInfo di, string rootDirectory)
        {
            return di.GetDirectories().Select(_ => new ContentsModel.Directory
            {
                Name = _.Name,
                Path = FixToRelativePath(_.FullName, rootDirectory),
                Files = di.GetFiles().Select(_ => new ContentsModel.File
                {
                    Name = _.Name,
                    Path = FixToRelativePath(_.FullName, rootDirectory)
                }).ToList(),
                SubDirs = EnumerateDir(di, rootDirectory),
            }).ToList();
        }

        static string FixToRelativePath(string absolutePath, string rootDirectory)
        {
            return absolutePath.Substring(rootDirectory.Length).Replace(Path.DirectorySeparatorChar, '/').TrimStart('/');
        }
    }
}
