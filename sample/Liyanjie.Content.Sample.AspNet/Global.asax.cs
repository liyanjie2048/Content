using System;
using System.Web;

using Liyanjie.Modularization.AspNet;

using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.Content.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        readonly static IServiceCollection services = new ServiceCollection();

        protected void Application_Start(object sender, EventArgs e)
        {
            static void registerServiceType(Type type, string lifeTime)
            {
                var _services = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(type),
                    "scoped" => services.AddScoped(type),
                    "transient" => services.AddTransient(type),
                    _ => services,
                };
            }
            static void registerServiceImplementationFactory(Type type, Func<IServiceProvider, object> implementationFactory, string lifeTime)
            {
                var _services = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(type, implementationFactory),
                    "scoped" => services.AddScoped(type, implementationFactory),
                    "transient" => services.AddTransient(type, implementationFactory),
                    _ => services,
                };
            }
            this.AddModularization(registerServiceType, registerServiceImplementationFactory)
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
                .AddVerificationCode(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                });
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var serviceProvider = services.BuildServiceProvider();
            this.UseModularization(serviceProvider);
        }
    }
}