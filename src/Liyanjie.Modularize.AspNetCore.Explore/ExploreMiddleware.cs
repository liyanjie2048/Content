namespace Liyanjie.Modularize.AspNetCore;

/// <summary>
/// 
/// </summary>
public class ExploreMiddleware(
    IOptions<ExploreModuleOptions> options)
    : IMiddleware
{
    readonly ExploreModuleOptions _options = options.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_options.RestrictRequestAsync is not null)
        {
            if (!await _options.RestrictRequestAsync(context))
                return;
        }

        var request = context.Request;

        var contents = ExploreHelper.GetContents(_options);
        foreach (var item in contents)
        {
            PathToWebPath(item, request);
        }

        await _options.SerializeToResponseAsync(context.Response, contents);
    }

    void PathToWebPath(ContentModel.Directory dir, HttpRequest request)
    {
        dir.Path = _options.PathToWebPath(dir.Path, request);
        foreach (var item in dir.Files)
        {
            item.Path = _options.PathToWebPath(item.Path, request);
        }
        foreach (var item in dir.SubDirs)
        {
            PathToWebPath(item, request);
        }
    }
}
