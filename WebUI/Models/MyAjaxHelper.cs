﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AskanioPhotoSite.WebUI.Models
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