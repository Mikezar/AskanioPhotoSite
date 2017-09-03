using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Services.Extensions
{
    public static class TagServiceExtension
    {
        public static MultiSelectList GetSelectListItem(this IEnumerable<Tag> tags, int[] selectedIds)
        {
            var items =  tags.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.TitleRu,
            });

            return new MultiSelectList(items, "Value", "Text", selectedIds);
        }
    }
}
