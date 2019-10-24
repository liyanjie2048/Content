using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Liyanjie.Contents.Sample.AspNetCore_2_1.Pages
{
    public class ImageModel : PageModel
    {
        public ImageModel(IHostingEnvironment env)
        {
            Images = Directory
                .GetFiles(Path.Combine(env.WebRootPath, "images"), "*.jpg")
                .Select(_ => $"images/{Path.GetFileName(_).ToLower()}")
                .ToList();
        }
        public IEnumerable<string> Images { get; }
    }
}
