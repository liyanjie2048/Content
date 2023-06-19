namespace Liyanjie.Content.Models;

/// <summary>
/// 
/// </summary>
public class ImageResizeModel
{
    /// <summary>
    /// 
    /// </summary>
    public string ImagePath { private get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryResize(ImageOptions options, out string result)
    {
        result = options.EmptyImagePath;

        var path = ImagePath.TrimStart(new[] { '/', '\\' });
        var match = path.Match(options.ResizePathPattern, RegexOptions.IgnoreCase);
        if (!match.Success)
            return false;

        var _parameters = match.Groups["parameters"].Value;
        var imageSourcePath = Path.Combine(options.RootDirectory, path.Replace(_parameters, string.Empty));
        if (!File.Exists(imageSourcePath))
        {
            var dotIndex = options.EmptyImagePath.LastIndexOf(".");
            result = $"{options.EmptyImagePath[..dotIndex]}{_parameters}{options.EmptyImagePath[dotIndex..]}";
            return true;
        }

        var size = match.Groups["size"].Value.Split('x');
        var width = int.TryParse(size[0], out var w) ? w : 0;
        var height = int.TryParse(size[1], out var h) ? h : 0;
        if (width == 0 && height == 0)
            return false;

        var image = Image.FromFile(imageSourcePath);
        if (width > 0 && height > 0)
        {
            image = image.Resize(width, height);
            var _color = match.Groups["color"].Value;
            if (!string.IsNullOrEmpty(_color))
            {
                var r = _color[..2].FromRadix16();
                var g = _color[2..4].FromRadix16();
                var b = _color[4..].FromRadix16();
                var tmp = new Bitmap(width, height);
                tmp.Clear(Color.FromArgb(r, g, b));
                tmp.Combine((new Point((width - image.Width) / 2, (height - image.Height) / 2), new Size(image.Width, image.Height), image));
                image = tmp;
            }
        }
        else if (width == 0)
            image = image.Resize(null, height);
        else if (height == 0)
            image = image.Resize(width, null);

        var imageDestinationPath = Path.Combine(options.RootDirectory, path.Replace('/', Path.DirectorySeparatorChar));
        using (image)
            image.CompressSave(imageDestinationPath, options.ImageQuality);

        result = path;
        return true;
    }
}
