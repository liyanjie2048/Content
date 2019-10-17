using System.Web;

namespace Liyanjie.Contents.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentsModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool TryMatchRequesting(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        void HandleResponsing(HttpContext httpContext);
    }
}
