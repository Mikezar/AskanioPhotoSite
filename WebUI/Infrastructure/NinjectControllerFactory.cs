using System;
using System.Web.Mvc;
using System.Web.Routing;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.WebUI.Properties;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using Ninject;
using System.Web;
using AskanioPhotoSite.Core.Services.Providers;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Convertors.Concrete;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Services.Concrete;

namespace AskanioPhotoSite.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var controllerInstance = controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);

            if (controllerInstance == null)
                throw new HttpException(404, "Bad request");

            return controllerInstance;
        }


        public override void ReleaseController(IController controller)
        {
            _ninjectKernel.Release(controller);
        }

        private void AddBindings()
        {
            _ninjectKernel.Bind<IConverterFactory>().To<ConverterFactory>();
            _ninjectKernel.Bind<BaseProvider<Album>>().To<AlbumProvider>();
            _ninjectKernel.Bind<IAlbumService>().To<AlbumService>();
            _ninjectKernel.Bind<BaseProvider<Photo>>().To<PhotoProvider>();
            _ninjectKernel.Bind<IPhotoService>().To<PhotoService>();
            _ninjectKernel.Bind<BaseProvider<Tag>>().To<TagProvider>();
            _ninjectKernel.Bind<ITagService>().To<TagService>();
            _ninjectKernel.Bind<BaseProvider<PhotoToTag>>().To<PhotoToTagProvider>();
            _ninjectKernel.Bind<BaseProvider<TextAttributes>>().To<TextAttributeProvider>();
            _ninjectKernel.Bind<ITextAttributeService>().To<TextAttributeService>();
            _ninjectKernel.Bind<BaseProvider<Watermark>>().To<WatermarkProvider>();
            _ninjectKernel.Bind<IWatermarkService>().To<WatermarkService>();
            _ninjectKernel.Bind<IStorage>().To<Storage>().InSingletonScope();
            _ninjectKernel.Bind<IImageProcessor>().To<ImageProcessor>()
                .WithConstructorArgument("path", Settings.Default.PhotoPath)
                .WithConstructorArgument("thumbFolder", Settings.Default.ThumbPath);
        }
    }
}