using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace AskanioPhotoSite.WebUI.Code.Files
{
    public class FileHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        public void ProcessRequest(HttpContext context)
        {
            // http://msdn.microsoft.com/ru-ru/magazine/cc163463.aspx

            // Формат: http://askanio.ru/file.aspx?Id=52&Size=P

            try
            {


                // Set up the response settings
                context.Response.ContentType = "image/jpeg";
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.BufferOutput = false;

                NameValueCollection nvc = HttpUtility.ParseQueryString(HttpUtility.HtmlDecode(context.Server.UrlDecode(context.Request.QueryString.ToString())));

                string filename = nvc.Get("Id");
                if (filename == null)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("Файл не найден"); // TODO: Ошибку необходимо локализовать
                    return;
                }

                string size = nvc.Get("Size");
                bool isOriginalSize = (size ?? String.Empty) != "P";//!((String) (context.Request.QueryString["Size"] ?? String.Empty) == "P");

                // Setup the PhotoID Parameter
                byte[] image = null;
                if (context.Request.QueryString["Id"] != null && context.Request.QueryString["Id"] != "")
                {
                    var id = Convert.ToInt32((String) context.Request.QueryString["Id"]);
                    image = PhotoManager.GetPhoto(id, isOriginalSize);
                }
                if (image != null)
                    context.Response.BinaryWrite(image);

                context.Response.Cache.SetETag($"{filename.GetHashCode()}-{size}");
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetSlidingExpiration(true);
                context.Response.Cache.SetValidUntilExpires(true);

                if (context.Response.IsClientConnected)
                    context.Response.Flush();
                context.Response.End();
            }
            catch (Exception)
            {
                // TODO: ошибку хорошо бы залогировать, а пользователю возвращать внятный ответ, а не 404-ю ошибку!
                context.Response.StatusCode = 404;
                context.Response.Write("File not found.");
                return;
            }

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}