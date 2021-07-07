using System;
using System.Web;

using Liyanjie.Modularization.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Content.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        readonly IServiceCollection services = new ServiceCollection();
        IServiceProvider serviceProvider;
        protected void Application_Start(object sender, EventArgs e)
        {
            services.AddModularization()
                .AddExplore(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                    options.Directories = new[] { "images", "temps" };
                    //options.ReturnAbsolutePath = true;
                })
                .AddUpload(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                }, "fileupload")
                .AddImage(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                }, resizeRouteTemplates: new[]
                {
                    "images/{filename}.{size}.{extension}",
                    "images/{folder}/{filename}.{size}.{extension}"
                })
                .AddCaptcha(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                });
            serviceProvider = services.BuildServiceProvider();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            using var scope = this.serviceProvider.CreateScope();
            this.UseModularization(scope.ServiceProvider);
        }
    }
}