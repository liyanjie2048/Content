using System;
using System.Web;

using Newtonsoft.Json;

namespace Liyanjie.Contents.Sample.AspNet
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            this.AddContents(JsonConvert.SerializeObject, JsonConvert.DeserializeObject)
                .AddImage(options => options.RootPath = Server.MapPath("~/"))
                .AddUpload(options => options.RootPath = Server.MapPath("~/"));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.UseContents();
        }
    }
}