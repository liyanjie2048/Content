using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            services.AddContents(JsonConvert.SerializeObject, JsonConvert.DeserializeObject)
                .AddUpload(options => options.RootPath = Env.WebRootPath)
                .AddImage(options => options.RootPath = Env.WebRootPath);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseContents();
            app.UseMvcWithDefaultRoute();
        }
    }
}
