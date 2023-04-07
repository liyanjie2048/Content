namespace Liyanjie.Content;

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
    public static IEnumerable<ContentModel.Directory> GetContents(ExploreOptions options)
    {
        var rootDirectory = options.RootDirectory;

        return options.Directories
            .Select(_ => new DirectoryInfo(Path.Combine(rootDirectory, _)))
            .Where(_ => _.Exists)
            .Select(_ => new ContentModel.Directory
            {
                Name = _.Name,
                Path = FixToRelativePath(_.FullName, rootDirectory),
                Files = _.GetFiles().Select(__ => new ContentModel.File
                {
                    Name = __.Name,
                    Path = FixToRelativePath(__.FullName, rootDirectory),
                }).ToList(),
                SubDirs = EnumerateDirectories(_, rootDirectory),
            }).ToList();
    }

    static IEnumerable<ContentModel.Directory> EnumerateDirectories(DirectoryInfo directory, string rootDirectory)
    {
        var directories = directory.GetDirectories();
        return directories
            .Select(_ => new ContentModel.Directory
            {
                Name = _.Name,
                Path = FixToRelativePath(_.FullName, rootDirectory),
                Files = _.GetFiles().Select(_ => new ContentModel.File
                {
                    Name = _.Name,
                    Path = FixToRelativePath(_.FullName, rootDirectory)
                }).ToList(),
                SubDirs = EnumerateDirectories(_, rootDirectory),
            }).ToList();
    }

    static string FixToRelativePath(string absolutePath, string rootDirectory)
    {
        return absolutePath[rootDirectory.Length..].Replace(Path.DirectorySeparatorChar, '/').TrimStart('/');
    }
}
