using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class PhotoUploadListModel
    {
        public PhotoUploadListModel()
        {
            Uploads = new List<PhotoUploadModel>();
        }

        public List<PhotoUploadModel> Uploads { get; set; }
    }
}
