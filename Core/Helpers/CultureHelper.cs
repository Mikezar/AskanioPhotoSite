using System.Threading;

namespace AskanioPhotoSite.Core.Helpers
{
    public class CultureHelper
    {
        public static bool IsEnCulture() =>
            Thread.CurrentThread.CurrentCulture.Name == "en-US";
    }
}