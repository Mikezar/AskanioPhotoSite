using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
   public class PhotoListModel
    {
        public PhotoListModel()
        {
            Photos = new List<PhotoModel>();
        }

        public List<PhotoModel> Photos{ get; set; }

        public string ReturnUrl { get; set; }
    }
}
