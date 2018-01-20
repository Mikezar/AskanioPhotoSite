using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Services.Abstract
{
    public interface ITextAttributeService
    {
        IEnumerable<TextAttributes> GetAll();
        TextAttributes GetOne(int id);
        TextAttributes AddOne(TextAttributeModel model);
        TextAttributes UpdateOne(TextAttributeModel model);
        void DeleteOne(int id);
    }
}
