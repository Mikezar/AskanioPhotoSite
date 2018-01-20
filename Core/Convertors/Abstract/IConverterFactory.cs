namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface IConverterFactory
    {
        TConvertor GetConverter<TConvertor>() where TConvertor : class;
    }
}
