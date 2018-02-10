using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AskanioPhotoSite.WebUI.Helpers
{
    public static class BreadCrumbHelper
    {
        public static MvcHtmlString Build(this HtmlHelper html, IEnumerable<Tuple<int, string>> albums)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var elements = new StringBuilder();

            elements.AppendFormat($"<li><a href=\"{urlHelper.Action("Index", "Gallery")}\"><span class=\"glyphicon glyphicon-picture\"></span></a></li>");

            foreach (var album in albums)
            {
                elements.AppendFormat($"<li><a href=\"{urlHelper.Action("Album", "Gallery", new { id = album.Item1 })}\">{album.Item2}</a></li>{Environment.NewLine}");
            }
            var div = new TagBuilder("ul");
            div.MergeAttribute("class", "breadcrumb breadcrumb-arrow");
            div.InnerHtml = $"{elements.ToString()}";

            return MvcHtmlString.Create(div.ToString());
        }
    }
}