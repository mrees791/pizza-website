using System.Web.Optimization;

namespace PizzaWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryplugins").Include(
                "~/Scripts/jquery.paging.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            // jQuery DM Uploader scripts
            bundles.Add(new ScriptBundle("~/bundles/jqueryplugins").Include(
                "~/Scripts/jquery.dm-uploader.js"));

            // jQuery DM Uploader CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/jQuery.FileUpload/css/jquery.dm-uploader.css"));

            // Site scripts
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/site.js",
                "~/Scripts/site-cart.js",
                "~/Scripts/site-checkout.js",
                "~/Scripts/site-manage-addresses.js",
                "~/Scripts/site-order-status.js"));

            // Site CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
        }
    }
}