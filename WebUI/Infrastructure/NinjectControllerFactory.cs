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
            _ninjectKernel.Bind<BaseService<Album>>().To<AlbumService>();
            _ninjectKernel.Bind<BaseService<Photo>>().To<PhotoService>();
            _ninjectKernel.Bind<BaseService<Tag>>().To<TagService>();
            _ninjectKernel.Bind<BaseService<PhotoToTag>>().To<PhotoToTagService>();
            _ninjectKernel.Bind<BaseService<TextAttributes>>().To<TextAttributeService>();
            _ninjectKernel.Bind<BaseService<Watermark>>().To<WatermarkService>();
            _ninjectKernel.Bind<IStorage>().To<Storage>();
            _ninjectKernel.Bind<IImageProcessor>().To<ImageProcessor>()
                .WithConstructorArgument("path", Settings.Default.PhotoPath)
                .WithConstructorArgument("thumbFolder", Settings.Default.ThumbPath);
        }
    }
}