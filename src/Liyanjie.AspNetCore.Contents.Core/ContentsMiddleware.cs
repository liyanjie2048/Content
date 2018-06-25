﻿using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.AspNetCore.Contents.Core
{
    public class ContentsMiddleware
    {
        readonly RequestDelegate next;
        readonly IServiceProvider serviceProvider;
        readonly ContentsBuilder contentsBuilder;

        public ContentsMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.contentsBuilder = serviceProvider.GetRequiredService<ContentsBuilder>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            foreach (var moduleType in contentsBuilder.ModuleTypes)
            {
                if (ActivatorUtilities.CreateInstance(serviceProvider, moduleType) is IContentsModule module)
                {
                    if (module.MatchRequesting(httpContext.Request))
                    {
                        module.HandleResponsing(httpContext.Response);
                        return;
                    }
                }
            }

            await next(httpContext);
        }
    }
}
