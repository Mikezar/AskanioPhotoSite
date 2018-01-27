using System;
using System.IO;

namespace AskanioPhotoSite.WebUI.Helpers
{
    public static class ContentLoader
    {
        public static string Get(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new NullReferenceException("path");

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return sr.ReadToEnd();
                }
            }

            return null;
        }
    }
}