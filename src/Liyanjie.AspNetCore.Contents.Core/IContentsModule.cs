using Microsoft.AspNetCore.Http;

namespace Liyanjie.AspNetCore.Contents.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentsModule
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        bool MatchRequesting(HttpRequest httpRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpResponse"></param>
        void HandleResponsing(HttpResponse httpResponse);
    }
}
