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
    [ApiExplorerSettings(GroupName = "Contents")]
    public abstract class _Controller : ControllerBase
    {
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
