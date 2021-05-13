using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Liyanjie.Content
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
        public static async Task<Image> FromFileOrNetworkAsync(string path)
        {
            try
            {
                if (path.IsMatch(@"^http(s)?\:\/\/[\S\s]*"))
                {
                    using var client = new HttpClient();
                    var stream = await client.GetStreamAsync(path);
                    return Image.FromStream(stream);
                }
                else
                    return File.Exists(path)
                        ? Image.FromFile(path)
                        : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
