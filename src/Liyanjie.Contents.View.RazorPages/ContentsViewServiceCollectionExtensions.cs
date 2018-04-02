using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContentsViewServiceCollectionExtensions
    {
        public static IServiceCollection AddContentsView(this IServiceCollection services, string apiXmlPath)
        {
            services
                .AddMvcCore()
                .AddApiExplorerGenerator(options =>
                {
                    options.Add("default", settings =>
                    {
                        settings.DescriptionFilter = _ => ((ApiDescription)_.Description).GroupName == "Content";
                    });
                    options.IncludeXmlDocmentation(apiXmlPath);
                });

            return services;
        }
    }
}
