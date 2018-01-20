using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface IEnumerableConverter<TResult, TModel>
    {
        IEnumerable<TResult> ConvertToIEnumerable(TModel[] models);
    }
}
