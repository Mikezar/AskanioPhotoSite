using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class AlbumModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int PhotoCount { get; set; }
    }
}
