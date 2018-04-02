using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Liyanjie.Contents.AspNetCore.Models;
using Microsoft.AspNetCore.Http;

namespace Liyanjie.Contents.AspNetCore.Extensions
{
    internal static class ExtendMethods
    {
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

        public static IEnumerable<string> Process(this IEnumerable<string> paths, string webRootPath, Settings.Settings settings)
        {
            return paths
                .Select(_ =>
                {
                    var uri = new Uri(_, UriKind.RelativeOrAbsolute);
                    return uri.IsAbsoluteUri && settings.ThisHosts.Any(__ => __.Equals(uri.Host, StringComparison.OrdinalIgnoreCase))
                        ? uri.PathAndQuery.TrimStart('/')
                        : _;
                })
                .Select(_ =>
                {
                    var uri = new Uri(_, UriKind.RelativeOrAbsolute);
                    if (uri.IsAbsoluteUri)
                        return _;
                    else if (Regex.IsMatch(_, @"^(\/)?image\/qrcode", RegexOptions.IgnoreCase))
                    {
                        var index = _.IndexOf('?');
                        var queryString = index > 0 ? _.Substring(index) : "?content=ERROR";
                        var fileName = new QueryString(queryString).GetModel<ImageQRCodeModel>().CreateQRCode(webRootPath, settings);
                        return Path.Combine(webRootPath, fileName).Replace('/', Path.DirectorySeparatorChar);
                    }
                    else
                        return Path.Combine(webRootPath, _).Replace('/', Path.DirectorySeparatorChar);
                })
                .ToList();
        }
    }
}
