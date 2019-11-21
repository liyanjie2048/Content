using System;
using System.Threading.Tasks;
using System.Web;

using Liyanjie.Modularization.AspNet;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Liyanjie.Contents.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        readonly static IServiceCollection services = new ServiceCollection();

        protected void Application_Start(object sender, EventArgs e)
        {
            static async Task<object> deserializeFromRequest(HttpRequest request, Type modelType)
            {
                await Task.FromResult(0);
                using var streamReader = new System.IO.StreamReader(request.InputStream);
                var _request = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject(_request, modelType);
            }
            static async Task serializeToResponse(HttpResponse response, object content)
            {
                await Task.FromResult(0);
                response.StatusCode = 200;
                response.ContentType = "application/json";
                response.Write(JsonConvert.SerializeObject(content));
                response.End();
            }
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
            static void registerServiceFactory(Type type, Func<IServiceProvider, object> implementationFactory, string lifeTime)
            {
                var _services = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(type, implementationFactory),
                    "scoped" => services.AddScoped(type, implementationFactory),
                    "transient" => services.AddTransient(type, implementationFactory),
                    _ => services,
                };
            }
            this.AddModularization(registerServiceType, registerServiceFactory)
                .AddUpload(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                    options.SerializeToResponseAsync = serializeToResponse;
                }, "fileupload")
                .AddImage(options =>
                {
                    options.RootDirectory = Server.MapPath("~/");
                    options.DeserializeFromRequestAsync = deserializeFromRequest;
                    options.SerializeToResponseAsync = serializeToResponse;
                }, resizeRouteTemplates: new[]
                {
                    "images/{filename}.{size}.{extension}",
                    "images/{folder}/{filename}.{size}.{extension}"
                });
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var serviceProvider = services.BuildServiceProvider();
            this.UseModularization(serviceProvider);
        }
    }
}