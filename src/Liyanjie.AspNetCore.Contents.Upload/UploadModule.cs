using Liyanjie.AspNetCore.Contents.Core;

using Microsoft.AspNetCore.Http;

namespace Liyanjie.AspNetCore.Contents.Upload
{
    public class UploadModule : IContentsModule
    {
        public string Name => nameof(UploadModule);

        public bool MatchRequesting(HttpRequest httpRequest) => false;

        public void HandleResponsing(HttpResponse httpResponse) { }
    }
}
