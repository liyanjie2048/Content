using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Liyanjie.Contents.Sample.AspNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Env = env;
        }

        public IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            static async Task<object> deserializeFromRequest(HttpRequest request, Type modelType)
            {
                using var streamReader = new System.IO.StreamReader(request.Body);
                var _request = await streamReader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(_request, modelType);
            }
            static async Task serializeToResponse(HttpResponse response, object content)
            {
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonConvert.SerializeObject(content));
            }
            services.AddModularization(deserializeFromRequest, serializeToResponse)
                .AddUpload(options => options.RootPath = Env.WebRootPath)
                .AddImage(options => options.RootPath = Env.WebRootPath);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseModularization();
            app.UseMvcWithDefaultRoute();
        }
    }
}
