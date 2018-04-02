using Liyanjie.ApiExplorer.Generator.Interfaces;
using Liyanjie.ApiExplorer.Generator.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Liyanjie.Contents.Sample.AspNetCore.Pages.View
{
    public class IndexModel : PageModel
    {
        readonly IDocGenerator DocGenerator;
        public IndexModel(IDocGenerator docGenerator)
        {
            this.DocGenerator = docGenerator;
        }

        public ApiDocument ApiDocument;

        public void OnGet()
        {
            this.ApiDocument = DocGenerator.ApiResolver.Create("default", "/");
        }
    }
}