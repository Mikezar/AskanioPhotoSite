using System;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Helpers;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;
using AskanioPhotoSite.WebUI.Localization;
namespace AskanioPhotoSite.WebUI.Code.Files
{
    public class FileHandler : IHttpHandler
    {
        public bool IsReusable => true;

        protected RequestContext RequestContext { get; set; }


        public FileHandler() : base() { }

        public FileHandler(RequestContext  requestContext)
        {
            RequestContext = requestContext;
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
                    photoPath = $"~/Content/Gallery/Photos/photo_AS-S{id}.jpg";
                    image = PhotoManager.GetPhoto(id, photoPath);
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