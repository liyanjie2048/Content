using System;
using System.Text.Json;
using System.Threading.Tasks;

using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Contents.Sample.AspNetCore_3_1
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Env = env;
        }

        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            static async Task<object> deserializeFromRequest(HttpRequest request, Type modelType)
            {
                using var streamReader = new System.IO.StreamReader(request.Body);
                var _request = await streamReader.ReadToEndAsync();
                return JsonSerializer.Deserialize(_request, modelType, new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    IgnoreReadOnlyProperties = true,
                    PropertyNameCaseInsensitive = true,
                });
            }
            static async Task serializeToResponse(HttpResponse response, object content)
            {
                response.StatusCode = 200;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonSerializer.Serialize(content));
            }
            services.AddModularization()
                .AddExplore(options =>
                {
                    options.RootDirectory = Env.WebRootPath;
                    options.Directories = new[] { "images", "temps" };
                    options.SerializeToResponseAsync = serializeToResponse;
                    //options.ReturnAbsolutePath = true;
                })
                .AddUpload(options =>
                {
                    options.RootDirectory = Env.WebRootPath;
                    options.SerializeToResponseAsync = serializeToResponse;
                }, "fileupload")
                .AddImage(options =>
                {
                    options.RootDirectory = Env.WebRootPath;
                    options.DeserializeFromRequestAsync = deserializeFromRequest;
                    options.SerializeToResponseAsync = serializeToResponse;
                }, resizeRouteTemplates: new[]
                {
                    "images/{filename}.{size}.{extension}",
                    "images/{folder}/{filename}.{size}.{extension}"
                });

            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapModularization();
            });
        }
    }
}
