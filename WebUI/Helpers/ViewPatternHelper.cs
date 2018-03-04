using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;


namespace AskanioPhotoSite.WebUI.Helpers
{

    public enum Patterns
    {
        Columns = 1,
        Tiles = 2
    }

    public static class ViewPatternHelper
    {
        private static readonly IDictionary<int, string> _patterns = new Dictionary<int, string>()
        {
            {(int)Patterns.Columns, "Columns" },
            {(int)Patterns.Tiles, "Tiles" }
        };

        public static IEnumerable<SelectListItem> GetPatterns()
        {
            return _patterns.Select(x => new SelectListItem()
            {
                Value = x.Key.ToString(),
                Text = x.Value
            });
        }

        public static string GetLayout(HttpRequestBase request)
        {
            return request.Browser.IsMobileDevice ?
                "~/Views/Shared/_BaseLayoutMobile.cshtml" : "~/Views/Shared/_BaseLayout.cshtml";
        }

    }
}