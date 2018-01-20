using System;
using AskanioPhotoSite.Core.Helpers;
using System.Web;
using System.Web.Routing;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.WebUI.Properties;
using AskanioPhotoSite.WebUI.Localization;
using AskanioPhotoSite.Core.Services.Concrete;
using AskanioPhotoSite.Core.Convertors.Concrete;
using AskanioPhotoSite.Core.Services.Providers;

namespace AskanioPhotoSite.WebUI.Infrastructure.Files
{
    public class FileHandler : IHttpHandler
    {
        private readonly PhotoManager _photoManager;

        public bool IsReusable => true;

        protected RequestContext RequestContext { get; set; }


        public FileHandler() : base() { }

        public FileHandler(RequestContext  requestContext)
        {
            RequestContext = requestContext;

            var storage = new Storage();
            var factory = new ConverterFactory();
            _photoManager = new PhotoManager(new TextAttributeService(factory, new TextAttributeProvider(storage)),
                new WatermarkService(factory, new WatermarkProvider(storage)));
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {

                context.Response.ContentType = "image/jpg";
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.BufferOutput = false;
                var nvc = HttpUtility.ParseQueryString(HttpUtility.HtmlDecode(context.Server.UrlDecode(context.Request.QueryString.ToString())));
          
                byte[] image = null;
                string photoPath = null;
                if (!string.IsNullOrEmpty(context.Request.QueryString["Id"]))
                {
                    var id = Convert.ToInt32(context.Request.QueryString["Id"]);
                    photoPath = $"{Settings.Default.PhotoPath}photo_AS-S{id}.jpg";
                    image = _photoManager.GetPhoto(id, photoPath);
                }

                if (image != null)
                    context.Response.BinaryWrite(image);

                context.Response.Cache.SetETag($"{photoPath.GetHashCode()}");
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetSlidingExpiration(true);
                context.Response.Cache.SetValidUntilExpires(true);

                if (context.Response.IsClientConnected)
                    context.Response.Flush();
            }
            catch (Exception e)
            {
                Log.RegisterError(e);
                context.Response.StatusCode = 404;
                context.Response.Write(MainUI.FileNotFoundError);
                return;
            }

        } 
    }
}