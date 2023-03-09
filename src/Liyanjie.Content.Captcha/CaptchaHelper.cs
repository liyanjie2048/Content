namespace Liyanjie.Content;

internal class CaptchaHelper
{
    /// <summary>
    /// 随机的背景色前景色
    /// </summary>
    public static Color RandomColor(bool? flag = null)
    {
        var random = new Random(Guid.NewGuid().GetHashCode());

        return flag.HasValue
            ? flag.Value
                ? Color.FromArgb(random.Next(0, 128), random.Next(0, 128), random.Next(0, 128))
                : Color.FromArgb(random.Next(128, 256), random.Next(128, 256), random.Next(128, 256))
            : Color.FromArgb(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
    }
}
