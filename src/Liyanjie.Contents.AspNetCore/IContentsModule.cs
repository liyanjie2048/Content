using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.Contents.AspNetCore
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
        Task<bool> TryMatchRequestingAsync(HttpContext httpContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        Task HandleResponsingAsync(HttpContext httpContext);
    }
}
