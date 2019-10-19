using System;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

namespace Liyanjie.Contents.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {
        static IServiceCollection services = new ServiceCollection();

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
            }
            void serviceRegister(object instance, string lifeTime)
                => _ = lifeTime.ToLower() switch
                {
                    "singleton" => services.AddSingleton(instance.GetType(), sp => instance),
                    "scoped" => services.AddScoped(instance.GetType(), sp => instance),
                    "transient" => services.AddTransient(instance.GetType(), sp => instance),
                    _ => services,
                };
            this.AddModularization(serviceRegister, deserializeFromRequest, serializeToResponse)
            //this.AddModularization(deserializeFromRequest, serializeToResponse)
                .AddImage(options => options.RootPath = Server.MapPath("~/"))
                .AddUpload(options => options.RootPath = Server.MapPath("~/"));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.UseModularization(services.BuildServiceProvider());
            //this.UseModularization(services.BuildServiceProvider());
        }
    }
}