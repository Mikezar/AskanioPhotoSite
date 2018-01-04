
namespace AskanioPhotoSite.WebUI.Helpers
{
    public static class MyAjaxHelper
    {
        public static object GetSuccessResponse()
            =>
                new {success = true};
        

        public static object GetSuccessResponse(object obj)
            =>
                new { success = true, result = obj };
    

        public static object GetErrorResponse(string message)
            =>
                new { success = false, errorMessage =  message};
    }
}