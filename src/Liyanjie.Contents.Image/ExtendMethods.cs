using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Liyanjie.Contents
{
    internal static class ExtendMethods
    {
        public static string MD5Encoded(this string input)
        {
            return input
                .Encode(Utilities.EncodeMode.MD5)
                .Replace("-", string.Empty)
                .ToLower();
        }

        public static int ToInt(this string input)
        {
            return int.TryParse(input, out int result)
                ? result
                : 0;
        }

        public static int FromRadix16(this string input)
        {
            try
            {
                return Convert.ToInt32(input, 16);
            }
            catch
            {
                return 255;
            }
        }

        public static void CreateDirectory(this string path)
        {
            if (path == null)
                return;

            Directory.CreateDirectory(path);
        }

        public static IEnumerable<string> Process(this IEnumerable<string> paths, string webRootPath)
        {
            return paths
                .Where(_ => !_.IsNullOrWhiteSpace())
                .Select(_ => new Uri(_, UriKind.RelativeOrAbsolute).IsAbsoluteUri ? _ : Path.Combine(webRootPath, _).Replace('/', Path.DirectorySeparatorChar))
                .ToList();
        }
    }
}
