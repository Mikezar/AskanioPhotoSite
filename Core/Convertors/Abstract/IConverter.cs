namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface IConverter<TResult, TModel>
    {
        TResult ConvertTo(TModel model);
    }
}
