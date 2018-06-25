using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Liyanjie.AspNetCore.Contents.Image
{
    internal static class ExtendMethods
    {
        public static string MD5Encode(this string input)
        {
            using (var md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", string.Empty);
            }
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

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static IEnumerable<string> Process(this IEnumerable<string> paths, string webRootPath, ImageOptions imageOptions)
        {
            return paths
                .Select(_ =>
                {
                    var uri = new Uri(_, UriKind.RelativeOrAbsolute);
                    return uri.IsAbsoluteUri ? _ : Path.Combine(webRootPath, _).Replace('/', Path.DirectorySeparatorChar);
                })
                .ToList();
        }
    }
}
