using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Models
{
   public class SideBarModel
    {
        public Photo Photo { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
