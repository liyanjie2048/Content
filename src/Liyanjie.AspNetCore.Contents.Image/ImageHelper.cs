using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Liyanjie.AspNetCore.Contents.Image
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ImageHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<System.Drawing.Image> FromFileOrNetworkAsync(string path)
        {
            try
            {
                if (Regex.IsMatch(path, @"^http(s)?\:\/\/[\S\s]*"))
                    using (var client = new HttpClient())
                    {
                        var stream = await client.GetStreamAsync(path);
                        return System.Drawing.Image.FromStream(stream);
                    }
                else
                    return File.Exists(path)
                        ? System.Drawing.Image.FromFile(path)
                        : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
