using System;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Liyanjie.Contents.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentsDefaults
    {
        public static bool TryMatchTemplate(string requestPath, string routeTemplate)
        {
            var routeValues = new RouteValueDictionary();
            var templateMatcher = new TemplateMatcher(TemplateParser.Parse(routeTemplate), routeValues);
            return templateMatcher.TryMatch(requestPath, new RouteValueDictionary());
        }

        /// <summary>
        /// 
        /// </summary>
        public static Func<object, string> JsonSerialize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static Func<string, Type, object> JsonDeserialize { get; set; }
    }
}
