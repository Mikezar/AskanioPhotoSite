using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Helpers;

namespace AskanioPhotoSite.WebUI.Helpers
{
    public static class CloudHelper
    {
        public static MvcHtmlString TagCloud(this HtmlHelper html, IEnumerable<TagCloudModel> tags)
        {
            if (tags.Count() == 0) return null;

            int min = tags.Min(t => t.Count);
            int max = tags.Max(t => t.Count);
            int dist = (max - min) / 3;
            var links = new StringBuilder();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            foreach (var tag in tags)
            {
                string tagClass;
                string title = CultureHelper.IsEnCulture() ? tag.TitleEng : tag.TitleRu;
                if (tag.Count == max)
                {
                    tagClass = "largest";
                }
                else if (tag.Count > (min + (dist * 2)))
                {
                    tagClass = "large";
                }
                else if (tag.Count > (min + dist))
                {
                    tagClass = "medium";
                }
                else if (tag.Count == min)
                {
                    tagClass = "smallest";
                }
                else
                {
                    tagClass = "small";
                }

                links.AppendFormat($"<li><a href=\"{urlHelper.Action("Tag", "Gallery", new { id = tag.Id})}\" title=\"{title}\" class=\"{tagClass}\"><span>{title}</span></a></li>{Environment.NewLine}");
            }

            var div = new TagBuilder("div");
            div.MergeAttribute("class", "tag-cloud");
            div.InnerHtml = $"<ul>{links.ToString()}</ul>";

            return MvcHtmlString.Create(div.ToString());
            
        }
    }
}