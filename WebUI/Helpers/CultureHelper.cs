using System.Threading;

namespace AskanioPhotoSite.WebUI.Helpers
{
    public class CultureHelper
    {
        public static bool IsEnCulture() =>
            Thread.CurrentThread.CurrentCulture.Name == "en-US";
    }
}