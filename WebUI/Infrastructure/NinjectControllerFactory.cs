using System;
using System.Web.Mvc;
using System.Web.Routing;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using Ninject;

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
                return base.GetControllerInstance(requestContext, controllerType);

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
            _ninjectKernel.Bind<IStorage>().To<Storage>();
        }
    }
}