namespace Liyanjie.Modularization.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ExploreMiddleware : IMiddleware
{
    readonly ILogger _logger;
    readonly ExploreModuleOptions _options;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public ExploreMiddleware(
        ILogger<ExploreMiddleware> logger,
        IOptions<ExploreModuleOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_options.RequestConstrainAsync is not null)
        {
            if (!await _options.RequestConstrainAsync(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.CompleteAsync();
                return;
            }
        }

        var request = context.Request;

        var contents = ExploreHelper.GetContents(_options);
        if (_options.ReturnAbsolutePath)
        {
            var pathPrefix = $"{request.Scheme}://{request.Host}/";
            foreach (var item in contents)
            {
                fixPath(item, pathPrefix);
            }

            static void fixPath(ContentModel.Directory dir, string pathPrefix)
            {
                dir.Path = pathPrefix + dir.Path;
                foreach (var item in dir.Files)
                {
                    item.Path = pathPrefix + item.Path;
                }
                foreach (var item in dir.SubDirs)
                {
                    fixPath(item, pathPrefix);
                }
            }
        }

        await _options.SerializeToResponseAsync(context.Response, contents);
    }
}
