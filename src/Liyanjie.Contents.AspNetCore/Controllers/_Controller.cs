using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Liyanjie.Contents.AspNetCore.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Content")]
    public abstract class _Controller : ControllerBase
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public _Controller(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetService<TService>() => serviceProvider.GetService<TService>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetRequiredService<TService>() => serviceProvider.GetRequiredService<TService>();

        /// <summary>
        /// 
        /// </summary>
        protected IHostingEnvironment HostingEnvironment => GetRequiredService<IHostingEnvironment>();

        /// <summary>
        /// 
        /// </summary>
        protected string WebRootPath => HostingEnvironment?.WebRootPath;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TScope"></typeparam>
        /// <returns></returns>
        protected ILogger<TScope> GetLogger<TScope>() => GetService<ILoggerFactory>()?.CreateLogger<TScope>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        protected void CreateDirectory(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                return;

            foreach (var path in paths)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }
    }
}
