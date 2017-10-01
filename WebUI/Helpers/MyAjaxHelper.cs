
namespace AskanioPhotoSite.WebUI.Helpers
{
    public static class MyAjaxHelper
    {
        public static object GetSuccessResponse()
        {
            return new {success = true};
        }

        public static object GetSuccessResponse(object obj)
        {
            return new { success = true, result = obj };
        }

        public static object GetErrorResponse(string message)
        {
            return new { success = false, errorMessage =  message};
        }
    }
}