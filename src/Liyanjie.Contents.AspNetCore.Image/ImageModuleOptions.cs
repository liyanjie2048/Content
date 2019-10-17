using System;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Liyanjie.Contents.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageModuleOptions : ImageOptions
    {
        public Func<HttpRequest, bool> TryMatchImageCombine { get; set; }
            = request => true
                && "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//POST请求
                && ContentsDefaults.TryMatchTemplate(request.Path, "Image/Combine");
        public Func<HttpRequest, bool> TryMatchImageConcatenate { get; set; }
            = request => true
                && "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//POST请求
                && ContentsDefaults.TryMatchTemplate(request.Path, "Image/Concatenate");
        public Func<HttpRequest, bool> TryMatchImageQRCode { get; set; }
            = request => true
                && "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//GET请求
                && ContentsDefaults.TryMatchTemplate(request.Path, "Image/QRCode");
        public Func<HttpRequest, string, bool> TryMatchImageResize { get; set; }
            = (request, rootPath) => true
                && "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase)//GET请求
                && request.Path.Value.IsMatch($@"^\S+\.(jpg|jpeg|png|gif|bmp)$", RegexOptions.IgnoreCase)//图片文件
                && !File.Exists(Path.Combine(rootPath, request.Path.Value).Replace('/', Path.DirectorySeparatorChar));

        /// <summary>
        /// 
        /// </summary>
        public bool ReturnAbsolutePath { get; set; } = true;
    }
}
