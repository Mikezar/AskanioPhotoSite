using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Entities
{
    public class Album : Entity
    {

        public int Id { get; set; }

        public int ParentId { get; set; }

        public string TitleRu { get; set; }

        public string TitleEng { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEng { get; set; }
    }
}
