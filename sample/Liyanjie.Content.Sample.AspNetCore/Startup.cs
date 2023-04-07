using Liyanjie.Modularization.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Content.Sample.AspNetCore
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
                .AddExplore(options =>
                {
                    options.RootDirectory = Env.WebRootPath;
                    options.Directories = new[] { "images", "temps" };
                    options.ReturnAbsolutePath = true;
                })
                .AddUpload(options => options.RootDirectory = Env.WebRootPath)
                .AddImage(options => options.RootDirectory = Env.WebRootPath, resizeRouteTemplates: new[]
                {
                    "images/{filename}_{resize}.{extension}",
                    "images/{directory}/{filename}_{resize}.{extension}"
                })
                .AddCaptcha(options =>
                {
                    options.RootDirectory = Env.WebRootPath;
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
