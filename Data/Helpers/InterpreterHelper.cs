using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Data.Helpers
{
    public static class InterpreterHelper
    {
        public static int GetValue(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;

            return Convert.ToInt32(str);
        }
    }
}
