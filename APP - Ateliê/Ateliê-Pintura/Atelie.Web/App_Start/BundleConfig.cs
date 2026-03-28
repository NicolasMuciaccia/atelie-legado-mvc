using System.Web.Optimization;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Atelie.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new Bundle("~/bundles/atelie-js").Include(
                      "~/Scripts/Namespaces/atelie.js",
                      "~/Scripts/Atelie/Helpers/helpers-modalmessage.js",
                      "~/Scripts/Atelie/Helpers/helpers-table.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Atelie/atelie-paleta.css",
                      "~/Content/Atelie/atelie-site.css",
                      "~/Content/Atelie/atelie-login.css",
                      "~/Content/Atelie/atelie-bootstrap.css"));
        }
    }
}
