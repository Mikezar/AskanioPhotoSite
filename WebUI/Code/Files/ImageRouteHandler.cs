using System.Configuration;
using System.Web;
using System.Web.Routing;

namespace AskanioPhotoSite.WebUI.Code.Files
{
    public class ImageRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new FileHandler(requestContext);
        }
    }

    public class ImageRouteHandlerConstaint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (routeDirection == RouteDirection.UrlGeneration)
                return false;
            if (values.ContainsKey("controller") || values.ContainsKey("action"))
                return false;

            if (httpContext.Request.FilePath.Contains("image"))
                return true;
            else
                return false;
        }
    }
}