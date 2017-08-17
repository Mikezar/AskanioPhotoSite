using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Helpers
{
   public static class LinqHelper
    {
        public static IEnumerable<SelectListItem> InsertEmptyFirst(this IEnumerable<SelectListItem> list, string emptyText = "", string emptyValue = "")
        {
            return new[] { new SelectListItem { Text = emptyText, Value = emptyValue } }.Concat(list);
        }
    }
}
