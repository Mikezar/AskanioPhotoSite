using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using AskanioPhotoSite.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AskanioPhotoSite.Core.Models
{
    public class PhotoFilterManagement
    {
        public PhotoFilterManagement()
        {
            Filters = GetElements();
            ListModel = new PhotoListModel();
        }

        public PhotoListModel ListModel { get; set; }

        [Display(Name = "Фильтр")]
        public PhotoFilter Filter { get; set; }

        public IEnumerable<SelectListItem> Filters { get; set; }

        private readonly IDictionary<PhotoFilter, string> _filters= new Dictionary<PhotoFilter, string>()
        {
            {PhotoFilter.None, "Все" },
            {PhotoFilter.NoAlbum, "Без альбома" },
            {PhotoFilter.Random, "Случайные" },
            {PhotoFilter.Background, "Фоновые" },
        };

        public IEnumerable<SelectListItem> GetElements()
        {
            return _filters.Select(x => new SelectListItem()
            {
                Value = x.Key.ToString(),
                Text = x.Value
            });
        }
    }
}
