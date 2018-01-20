using AskanioPhotoSite.Core.Convertors.Abstract;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
   public class ConverterFactory : IConverterFactory
    {
        private static IConverterFactory Factory { get; set; }

        private readonly Dictionary<object, object> _convertors = new Dictionary<object, object>()
        {
            { typeof(IAlbumConverter), new AlbumConverter() },
            { typeof(IPhotoConverter), new PhotoConverter() },
            { typeof(ITagConverter), new TagConverter() },
            { typeof(ITextAttributeConverter), new TextAttributeConverter() },
            { typeof(IWatermarkConverter), new WatermarkConverter() },
        };

        public static IConverterFactory Create()
        {
            if(Factory == null)
            {
                Factory = new ConverterFactory();
            }

            return Factory;
        }

        public TConvertor GetConverter<TConvertor>() where TConvertor : class
        {
            if(_convertors.ContainsKey(typeof(TConvertor)))
                return (TConvertor)_convertors[typeof(TConvertor)];

            return default(TConvertor);
        }
    }
}
