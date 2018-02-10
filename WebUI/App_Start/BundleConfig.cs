using System.Web.Optimization;

namespace AskanioPhotoSite.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate-unobtrusive.js",
                        "~/Scripts/jquery.multi-select.js",
                        "~/Scripts/masonry.min.js",
                        "~/Scripts/imagesLoaded.min.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/select2.full.js",
                        "~/Scripts/main-carousel.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/common.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/MobileSite.css",
                      "~/Content/Gallery.css",
                      "~/Content/select2.css",
                      "~/Content/multi-select.css"
                      ));
        }
    }
}
