using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Contents.Sample.AspNetCore_3_0
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
            services.AddModularization()
                .AddUpload(options => options.RootDirectory = Env.WebRootPath, "fileupload")
                .AddImage(options => options.RootDirectory = Env.WebRootPath, resizeRouteTemplates: new[]
                {
                    "images/{filename}.{size}.{extension}",
                    "images/{directory}/{filename}.{size}.{extension}"
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
