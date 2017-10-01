using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Threading;

namespace AskanioPhotoSite.WebUI.Helpers
{
    public class CultureHelper
    {
        public static bool IsEnCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "en-US";
        }
    }
}