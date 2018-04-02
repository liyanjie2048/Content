using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;

namespace Liyanjie.Contents.Sample.AspNetCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddContents(Configuration.GetSection("Content"))
                .AddContentsView(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Liyanjie.Contents.AspNetCore.xml"))
                .AddContentsSdk(options =>
                {
                    options.ServerUrlBase = "http://localhost:50350";
                })
                .AddCors()
                .AddAuthorization()
                .AddMvcCore()
                .AddRazorPages(options =>
                {
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseCors(builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                })
                .UseStaticFiles()
                .UseContents()
                .UseMvcWithDefaultRoute();
        }
    }
}
