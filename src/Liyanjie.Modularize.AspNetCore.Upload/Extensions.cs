namespace Liyanjie.Modularize.AspNetCore;

static class Extensions
{
    public static string GetDir(this IQueryCollection query, string @default = "temp")
    {
        if (query.TryGetValue("dir", out var dir) && !string.IsNullOrEmpty(dir))
            return dir!;

        return @default;
    }
}
