using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class TagCloudModel
    {
        public int Id { get; set; }

        public string TitleEng { get; set; }

        public string TitleRu { get; set; }

        public int Count { get; set; }
    }
}
