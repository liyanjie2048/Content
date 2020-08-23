using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Liyanjie.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public class ExploreHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IList<ContentsModel.Directory> GetContents(ExploreOptions options)
        {
            var rootDirectory = options.RootDirectory;

            return options.Directories
                .Select(_ => new DirectoryInfo(Path.Combine(rootDirectory, _)))
                .Where(_ => _.Exists)
                .Select(_ => new ContentsModel.Directory
                {
                    Name = _.Name,
                    Path = _.FullName,
                    Files = _.GetFiles().Select(__ => new ContentsModel.File
                    {
                        Name = __.Name,
                        Path = FixToRelativePath(__.FullName, rootDirectory),
                    }).ToList(),
                    SubDirs = EnumerateDirectories(_, rootDirectory),
                }).ToList();
        }

        static IList<ContentsModel.Directory> EnumerateDirectories(DirectoryInfo directory, string rootDirectory)
        {
            var directories = directory.GetDirectories();
            return directories.Length > 0
                ? directories.Select(_ => new ContentsModel.Directory
                {
                    Name = _.Name,
                    Path = FixToRelativePath(_.FullName, rootDirectory),
                    Files = _.GetFiles().Select(_ => new ContentsModel.File
                    {
                        Name = _.Name,
                        Path = FixToRelativePath(_.FullName, rootDirectory)
                    }).ToList(),
                    SubDirs = EnumerateDirectories(_, rootDirectory),
                }).ToList()
                : null;
        }

        static string FixToRelativePath(string absolutePath, string rootDirectory)
        {
            return absolutePath.Substring(rootDirectory.Length).Replace(Path.DirectorySeparatorChar, '/').TrimStart('/');
        }
    }
}
