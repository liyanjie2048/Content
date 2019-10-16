using Liyanjie.AspNetCore.Contents.Core;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.AspNetCore.Contents.Upload
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadModule : IContentsModule
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => nameof(UploadModule);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public bool MatchRequesting(HttpRequest httpRequest) => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        public void HandleResponsing(HttpResponse httpResponse) { }
    }
}
